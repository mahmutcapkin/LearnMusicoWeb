using LearnMusico.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnMusico.ViewModels
{
    public class MessageUserViewModel
    {
        public List<SelectListItem> Users { get; set; }
        public List<Messages> Messages { get; set; }
    }
}