using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;



    public static class DropDownListExtensions
    {
        public static MvcHtmlString DropDownItemList(this HtmlHelper htmlHelper,
        string name,
        IEnumerable<DropDownListItem> listInfo,
             object htmlAttributes)
        {
            return htmlHelper.DropDownItemList(name, listInfo, (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString DropDownItemList(this HtmlHelper htmlHelper,
        string name,
        IEnumerable<DropDownListItem> listInfo,
        IDictionary<string, object> htmlAttributes
        )
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("必須給這些CheckBoxList一個Tag Name", "name");
            }
            if (listInfo == null)
            {
                throw new ArgumentNullException("listInfo", "必須要給List<SelectListItem> listInfo");
            }
            if (listInfo.Count() < 1)
            {
                throw new ArgumentException("List<SelectListItem> listInfo 至少要有一組資料", "listInfo");
            }

            StringBuilder sb = new StringBuilder();
            
            TagBuilder dropdown = new TagBuilder("select");
            dropdown.MergeAttributes<string, object>(htmlAttributes);
            dropdown.MergeAttribute("name", name);
            dropdown.MergeAttribute("id", name);
            StringBuilder options = new StringBuilder();
            foreach (var item in listInfo)
            {


                var selected = item.Selected ? "selected" : string.Empty;
                if (item.Selected)
                {
                    options = options.Append("<option data-id='"+item.DataAttr+"' selected value='" + item.Value + "'>" + item.Text + "</option>");
                }
                else
                {
                    options = options.Append("<option data-id='" + item.DataAttr + "'value='" + item.Value + "'>" + item.Text + "</option>");
                }







                dropdown.InnerHtml = options.ToString();
                //Assigning the attributes passed as a htmlAttributes object.
                dropdown.MergeAttributes(new RouteValueDictionary(htmlAttributes));
                dropdown.ToString(TagRenderMode.Normal);

                
            }
            return MvcHtmlString.Create(dropdown.ToString());
        }
    }

    public  class DropDownListItem{
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
        public string DataAttr { get; set; }
    }
