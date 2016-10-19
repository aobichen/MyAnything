using Anything.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.ViewModels
{
    public class AdViewModel
    {
    }

    public class AdvImage
    {
        public int ID { get; set; }
        public byte[] Image { get; set; }
        public string Text { get; set; }

        public string Url { get; set; }

    }
}