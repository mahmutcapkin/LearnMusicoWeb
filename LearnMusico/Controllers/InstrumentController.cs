using LearnMusico.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnMusico.Controllers
{
    public class InstrumentController : Controller
    {
        private InstrumentManager _instrumentManager = new InstrumentManager();
        public ActionResult Index()
        {
            return View(_instrumentManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }
    }
}