using LearnMusico.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnMusico.ViewModels
{
    public class SharingCommentViewModel
    {
        public Sharing sharing { get; set; }
        public List<Comment>  comment { get; set; }
    }
}