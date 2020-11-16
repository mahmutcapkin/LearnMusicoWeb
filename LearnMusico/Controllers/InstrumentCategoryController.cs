using LearnMusico.BusinessLayer;
using LearnMusico.Entities;
using LearnMusico.Filters;
using LearnMusico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LearnMusico.Controllers
{
    [Auth]
    [AuthTeacher]
    public class InstrumentCategoryController : Controller
    {
        private InstrumentCategoryManager icm = new InstrumentCategoryManager();
        public ActionResult Index()
        {
            return View(icm.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstrumentCategory instrumentCategory = icm.Find(x => x.Id == id.Value);
            if (instrumentCategory == null)
            {
                return HttpNotFound();
            }
            return View(instrumentCategory);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstrumentCategory instrumentcategory)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                icm.Insert(instrumentcategory);

                CacheHelper.RemoveInstrumentCategoryFromCache();

                return RedirectToAction("Index");
            }

            return View(instrumentcategory);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstrumentCategory instrumentcategory = icm.Find(x => x.Id == id.Value);
            if (instrumentcategory == null)
            {
                return HttpNotFound();
            }
            return View(instrumentcategory);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InstrumentCategory instrumentcategory)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {

                InstrumentCategory cat = icm.Find(x => x.Id == instrumentcategory.Id);
                cat.Title = instrumentcategory.Title;

                icm.Update(instrumentcategory);

                CacheHelper.RemoveInstrumentCategoryFromCache();

                return RedirectToAction("Index");
            }
            return View(instrumentcategory);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstrumentCategory instrumentcategory = icm.Find(x => x.Id == id.Value);
            if (instrumentcategory == null)
            {
                return HttpNotFound();
            }
            return View(instrumentcategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstrumentCategory instrumentcategory = icm.Find(x => x.Id == id);
            icm.Delete(instrumentcategory);

            CacheHelper.RemoveInstrumentCategoryFromCache();

            return RedirectToAction("Index");
        }


    }
}