using Anything.Areas.System.Models;
using Anything.Helpers;
using Anything.Models;
using Anything.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace Anything.WebApi
{
    public class BonusController : ApiController
    {
        [HttpPost]
        [Route("Bonus/Update")]
        public void UpdateBonus()
        {
            var Today = DateTime.Now;
            var CanUserStatus = BonusStatusEnum.CanUse.ToString();
            var db = new MyAnythingEntities();
            
            var CanUserModel = db.MyBonus.Where(o => (o.UseMonth.Year == Today.Year && o.UseMonth.Month == Today.Month) && o.BonusStatus != CanUserStatus).ToList();
                       
            if (CanUserModel != null && CanUserModel.Count > 0)
            {
                foreach (var item in CanUserModel)
                {
                    item.BonusStatus = CanUserStatus;
                }
                db.SaveChanges();
            }

           

            var unPaid = OrderType.Unpaid.ToString();
            var ExpiredOrder = db.OrderMaster.Where(o => o.Status == unPaid &&
                    (o.ExpireDate ==null || DateTime.Compare(o.ExpireDate.Value, Today) < 0 )).ToList();
            if (ExpiredOrder != null && ExpiredOrder.Count > 0)
            {
                foreach (var item in ExpiredOrder)
                {
                    item.Status = OrderType.Expired.ToString();
                }
                db.SaveChanges();
            }
            
            db.Dispose();

        }

        [HttpPost]
        [Route("Order/ExpireDate")]
        public void ExpireDate()
        {
            var Today = DateTime.Now;
            var db = new MyAnythingEntities();
            var unPaid = OrderType.Unpaid.ToString();
            var ExpiredOrder = db.OrderMaster.Where(o => o.Status == unPaid &&
                    (o.ExpireDate == null || DateTime.Compare(o.ExpireDate.Value, Today) < 0)).ToList();
            if (ExpiredOrder != null && ExpiredOrder.Count > 0)
            {
                foreach (var item in ExpiredOrder)
                {
                    item.Status = OrderType.Expired.ToString();
                }
                db.SaveChanges();
            }

            db.Dispose();
        }

        [HttpPost]
        [Route("Bonus/Notice")]
        public void BonusNotice()
        {
            var Today = DateTime.Now;

            var CurrentDate = Today.ToString("dd");
            var Date = "25";
            var NextDate = "26";
            if (!CurrentDate.Equals(Date) && !CurrentDate.Equals(NextDate))
            {
                return;
            }
            var db = new MyAnythingEntities();
            var unPaid = OrderType.Unpaid.ToString();
            var Notice = new BonusNoticeViewModel().QueryFor25Date();
            var NoticeContent = Notice.ItemDescription;
            var Notices = db.MyBonus.Where(o => o.Notified == false &&
                o.Created.Year == Today.Year &&
                o.Created.Month == Today.Month).ToList();
            var result = db.MyBonus.GroupBy(o => o.UserID)
                   .Select(g => new { UserID = g.Key, total = g.Sum(i => i.Bonus) });

            var Noticed = new List<int>();

            foreach (var notice in Notices)
            {
                try {
                    if (Noticed == null || !Noticed.Any(o => o == notice.UserID))
                    {

                        var User = new ApplicationDbContext2().Users.Where(o => o.Id == notice.UserID).FirstOrDefault();

                        var UserOfSum = result.Where(o => o.UserID == notice.UserID).FirstOrDefault();
                        var Sum = 0M;
                        if (UserOfSum != null)
                        {
                            Sum = UserOfSum.total;
                        }
                        if (User != null)
                        {
                            NoticeContent = NoticeContent.Replace("{{姓名}}", User.UserName);
                            NoticeContent = NoticeContent.Replace("{{當月份}}", notice.Created.Month.ToString());
                            NoticeContent = NoticeContent.Replace("{{使用月份}}", notice.UseMonth.Month.ToString());
                            NoticeContent = NoticeContent.Replace("{{總紅利}}", Sum.ToString("#.##0"));
                            NoticeContent = NoticeContent.Replace("{{紅利入帳需消費金額}}", notice.AmtMinLimit.ToString());
                        }

                        var From = ConfigurationManager.AppSettings["From"];
                        var Password = ConfigurationManager.AppSettings["SmtpPassword"];
                        var SmtpUserName = ConfigurationManager.AppSettings["SmtpUserName"];
                        var Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                        var SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];

                        SmtpClient smtpClient = new SmtpClient(SmtpServer, Port);


                        smtpClient.Credentials = new System.Net.NetworkCredential(From, Password);
                        //smtpClient.UseDefaultCredentials = true;              
                        smtpClient.EnableSsl = true;

                        MailMessage mail = new MailMessage();
                        mail.Body = NoticeContent;
                        mail.Subject = Notice.ItemValue;
                        mail.IsBodyHtml = true;
                        mail.SubjectEncoding = System.Text.Encoding.UTF8;
                        mail.Priority = MailPriority.Normal;
                        mail.From = new MailAddress(From, SmtpUserName);
                        mail.To.Add(new MailAddress(User.Email));

                        smtpClient.Send(mail);
                        smtpClient = null;
                        mail.Dispose();

                        Noticed.Add(UserOfSum.UserID);
                    }
                }catch(Exception ex)
                {
                    db.SystemLog.Add(new SystemLog { Created = DateTime.Now,Creator="Sys", LogDescription ="紅利通知發送錯誤", LogValue = ex.Message.ToString(), LogCode = "Error", LogType = "Error", IP = string.Empty});
                    db.SaveChanges();
                }
                finally
                {
                    foreach (var item in Notices)
                    {
                        item.Notified = true;
                        db.SaveChanges();
                    }
                }
            
                
            }
            db.Dispose();
        }

        [HttpPost]
        [Route("Bonus/BonusNoticeFor1day")]
        public void BonusNoticeFor1day()
        {
            var Today = DateTime.Now;

            var CurrentDate = Today.ToString("dd");
            var Date = "01";
            var NextDate = "1";
            if (!CurrentDate.Equals(Date) && !CurrentDate.Equals(NextDate))
            {
                return;
            }
            var db = new MyAnythingEntities();
            var unPaid = OrderType.Unpaid.ToString();
            var Notice = new BonusNoticeViewModel().QueryFor1Date();
            var NoticeContent = Notice.ItemDescription;
            var Notices = db.MyBonus.Where(o => o.BonusStatus.Equals("CanUse") && 
                o.Created.Year == Today.Year &&
                o.UseMonth.Month == Today.Month && o.NoticedFor1Date != true).ToList();

            var result = db.MyBonus.GroupBy(o => o.UserID)
                   .Select(g => new { UserID = g.Key, total = g.Sum(i => i.Bonus) });

            var Noticed = new List<int>();

            foreach (var notice in Notices)
            {
                try
                {
                    if (Noticed == null || !Noticed.Any(o => o == notice.UserID))
                    {

                        var User = new ApplicationDbContext2().Users.Where(o => o.Id == notice.UserID).FirstOrDefault();

                        var UserOfSum = result.Where(o => o.UserID == notice.UserID).FirstOrDefault();
                        var Sum = 0M;
                        if (UserOfSum != null)
                        {
                            Sum = UserOfSum.total;
                        }
                        if (User != null)
                        {
                            NoticeContent = NoticeContent.Replace("{{姓名}}", User.UserName);
                            NoticeContent = NoticeContent.Replace("{{當月份}}", notice.Created.Month.ToString());
                            NoticeContent = NoticeContent.Replace("{{使用月份}}", notice.UseMonth.Month.ToString());
                            NoticeContent = NoticeContent.Replace("{{總紅利}}", Sum.ToString("#.##0"));
                            NoticeContent = NoticeContent.Replace("{{紅利入帳需消費金額}}", notice.AmtMinLimit.ToString());
                        }

                        var From = ConfigurationManager.AppSettings["From"];
                        var Password = ConfigurationManager.AppSettings["SmtpPassword"];
                        var SmtpUserName = ConfigurationManager.AppSettings["SmtpUserName"];
                        var Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                        var SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];

                        SmtpClient smtpClient = new SmtpClient(SmtpServer, Port);


                        smtpClient.Credentials = new System.Net.NetworkCredential(From, Password);
                        //smtpClient.UseDefaultCredentials = true;              
                        smtpClient.EnableSsl = true;

                        MailMessage mail = new MailMessage();
                        mail.Body = NoticeContent;
                        mail.Subject = Notice.ItemValue;
                        mail.IsBodyHtml = true;
                        mail.SubjectEncoding = System.Text.Encoding.UTF8;
                        mail.Priority = MailPriority.Normal;
                        mail.From = new MailAddress(From, SmtpUserName);
                        mail.To.Add(new MailAddress(User.Email));

                        smtpClient.Send(mail);
                        smtpClient = null;
                        mail.Dispose();

                        Noticed.Add(UserOfSum.UserID);
                    }
                }
                catch (Exception ex)
                {
                    db.SystemLog.Add(new SystemLog { Created = DateTime.Now, Creator = "Sys", LogDescription = "紅利通知發送錯誤", LogValue = ex.Message.ToString(), LogCode = "Error", LogType = "Error", IP = string.Empty });
                    db.SaveChanges();
                }
                finally
                {
                    foreach (var item in Notices)
                    {
                        item.NoticedFor1Date = true;
                        db.SaveChanges();
                    }
                }


            }
            db.Dispose();
        }
    }


}
