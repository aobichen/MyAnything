using Anything.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Anything.Helpers
{
   
   
    public class BaseDLL
    {

        public string GetUserCode(string Name)
        {
            var result = (Name + DateTime.Now.ToString()).GetHashCode().ToString("x");
            using (var db = new ApplicationDbContext())
            {
                if (db.Users.Any(o => o.UserCode == result))
                {
                    GetUserCode(Name);
                }
            }
            return result;
        }


        
    }

    public class VerificationCode
    {

        public byte[] bytes { get; set; }
        public Image image { get; set; }

        public VerificationCode()
        {
            CreateCheckCodeImage(4);
        }

        public VerificationCode(int codeLength)
        {
            CreateCheckCodeImage(codeLength);
        }

        public void CreateCheckCodeImage(int length)
        {

            if (ConfigurationManager.AppSettings["VerificationCode"] == null)
            {
                throw new Exception("請設定ConfigurationManager.AppSettings['VerificationCode']");
            }

            string text = ConfigurationManager.AppSettings["VerificationCode"];

            string letters = "1,2,3,4,5,6,7,8,9,Q,W,E,R,T,Y,U,A,S,D,F,G,H,J,K,L,Z,X,C,V,B,N,M,w,e,r,t,y,u,i,p,a,s,d,f,g,h,j,k,z,x,c,v,b,n,m";
            var chars = letters.Split(',');
            var charRandom = new Random();
            var code = string.Empty;
            for (var i = 0; i < length; i++)
            {
                code += chars[charRandom.Next(chars.Length)];
            }

            HttpContext.Current.Session[text] = code;
            System.Drawing.Bitmap img = new System.Drawing.Bitmap((code.Length * 20), 40);//產生圖片，寬20*位數，高40像素
            System.Drawing.Graphics g = Graphics.FromImage(img);


            //生成隨機生成器
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int int_Red = 0;
            int int_Green = 0;
            int int_Blue = 0;
            int_Red = random.Next(256);//產生0~255
            int_Green = random.Next(256);//產生0~255
            int_Blue = (int_Red + int_Green > 400 ? 0 : 400 - int_Red - int_Green);
            int_Blue = (int_Blue > 255 ? 255 : int_Blue);

            //清空圖片背景色
            g.Clear(Color.FromArgb(int_Red, int_Green, int_Blue));

            //畫圖片的背景噪音線
            for (int i = 0; i <= 24; i++)
            {


                int x1 = random.Next(img.Width);
                int x2 = random.Next(img.Width);
                int y1 = random.Next(img.Height);
                int y2 = random.Next(img.Height);

                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);

                g.DrawEllipse(new Pen(Color.DarkViolet), new System.Drawing.Rectangle(x1, y1, x2, y2));
            }

            Font font = new System.Drawing.Font("Arial", 20, (System.Drawing.FontStyle.Bold));
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, img.Width, img.Height), Color.Blue, Color.DarkRed, 1.2F, true);

            g.DrawString(code, font, brush, 2, 2);
            for (int i = 0; i <= 99; i++)
            {

                //畫圖片的前景噪音點
                int x = random.Next(img.Width);
                int y = random.Next(img.Height);

                img.SetPixel(x, y, Color.FromArgb(random.Next()));
            }

            //畫圖片的邊框線
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, img.Width - 1, img.Height - 1);

            bytes = imageToByteArray(img);
            image = img;

            font.Dispose();
            g.Dispose();
            image.Dispose();

            //return img_byte;

        }

        private byte[] imageToByteArray(System.Drawing.Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }

    public class HtmlContent{

        private string urlAddress { get; set; }
        public string Text { get { return render(); } }
        public HtmlContent(string url)
        {
            urlAddress = HttpContext.Current.Server.MapPath(url);
        }

        private string render()
        {           
            return File.ReadAllText(urlAddress);
        }
    }

    
}