using LearnMusico.Common;
using LearnMusico.Entities;
using LearnMusico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnMusico.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            MusicaUser musicaUser = CurrentSession.User;
            if (musicaUser != null)
                return musicaUser.Username;
            else
                return "system";
        }
    }
}