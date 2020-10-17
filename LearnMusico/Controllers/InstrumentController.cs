using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnMusico.Controllers
{
    public class InstrumentController : Controller
    {
        // GET: Instrument
        public ActionResult Index()
        {
            return View();
        }
    }
}