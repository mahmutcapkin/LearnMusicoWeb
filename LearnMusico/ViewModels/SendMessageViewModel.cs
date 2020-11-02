using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnMusico.ViewModels
{
    public class SendMessageViewModel
    { 
        public string Subject { get; set; }
        public string MessageBody { get; set; }

        public int ToUserId { get; set; }
    }
}