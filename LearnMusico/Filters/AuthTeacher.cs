using LearnMusico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnMusico.Filters
{
    public class AuthTeacher: FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.User != null && CurrentSession.User.IsTeacher == false)
            {
                filterContext.Result = new RedirectResult("/Home/AccessDenied");
            }
        }
    }
}