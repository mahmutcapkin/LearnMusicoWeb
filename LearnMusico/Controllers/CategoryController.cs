using LearnMusico.BusinessLayer;
using LearnMusico.Entities;
using LearnMusico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LearnMusico.Controllers
{
    public class CategoryController : Controller
    {
        private ArticleCategoryManager acm = new ArticleCategoryManager();
        private InstrumentCategoryManager icm = new InstrumentCategoryManager();
        public ActionResult ArticleCIndex()
        {
            return View(acm.List());
        }

        public ActionResult InsturmentCIndex()
        {
            return View(icm.List());
        }
        public ActionResult ArticleCDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleCategory articleCategory = acm.Find(x => x.Id == id.Value);
            if (articleCategory == null)
            {
                return HttpNotFound();
            }
            return View(articleCategory);
        }

        public ActionResult InstrumentCDetails(int? id)
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

        public ActionResult ArticleCCreate()
        {
            return View();
        }

        public ActionResult InstrumentCCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArticleCCreate(ArticleCategory articlecategory)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                acm.Insert(articlecategory);

                CacheHelper.RemoveArticleCategoryFromCache();

                return RedirectToAction("Index");
            }

            return View(articlecategory);
        }

        public ActionResult ArticleCEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleCategory articlecategory = acm.Find(x => x.Id == id.Value);
            if (articlecategory == null)
            {
                return HttpNotFound();
            }
            return View(articlecategory);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArticleCEdit(ArticleCategory articlecategory)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                //TODO: incelenicek..
                ArticleCategory cat = acm.Find(x => x.Id == articlecategory.Id);
                cat.Title = articlecategory.Title;

                acm.Update(articlecategory);

                CacheHelper.RemoveArticleCategoryFromCache();

                return RedirectToAction("Index");
            }
            return View(articlecategory);
        }


        public ActionResult ArticleCDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleCategory articlecategory = acm.Find(x => x.Id == id.Value);
            if (articlecategory == null)
            {
                return HttpNotFound();
            }
            return View(articlecategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ArticleCDeleteConfirmed(int id)
        {
            ArticleCategory articlecategory = acm.Find(x => x.Id == id);
            acm.Delete(articlecategory);

            CacheHelper.RemoveArticleCategoryFromCache();

            return RedirectToAction("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstrumentCCreate(InstrumentCategory instrumentcategory)
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

        public ActionResult InstrumentCCEdit(int? id)
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
        public ActionResult InstrumentCCEdit(InstrumentCategory instrumentcategory)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                //TODO: incelenicek..
                InstrumentCategory cat = icm.Find(x => x.Id == instrumentcategory.Id);
                cat.Title = instrumentcategory.Title;

                icm.Update(instrumentcategory);

                CacheHelper.RemoveArticleCategoryFromCache();

                return RedirectToAction("Index");
            }
            return View(instrumentcategory);
        }


        public ActionResult InstrumentCDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleCategory articlecategory = acm.Find(x => x.Id == id.Value);
            if (articlecategory == null)
            {
                return HttpNotFound();
            }
            return View(articlecategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult InstrumentCDeleteConfirmed(int id)
        {
            ArticleCategory articlecategory = acm.Find(x => x.Id == id);
            acm.Delete(articlecategory);

            CacheHelper.RemoveArticleCategoryFromCache();

            return RedirectToAction("Index");
        }

    }
}