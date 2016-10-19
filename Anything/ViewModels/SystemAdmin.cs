using Anything.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Anything.ViewModels
{
    public class ServiceOptionViewModel:ServiceOption
    {
       
        public List<ServiceOption> Options { get; set; }
    }
}