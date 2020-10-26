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
    public class ArticleCategoryController : Controller
    {

        private ArticleCategoryManager acm = new ArticleCategoryManager();

        public ActionResult Index()
        {
            return View(CacheHelper.GetArticleCategoryFromCache());
        }

        public ActionResult Details(int? id)
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


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleCategory articlecategory)
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

        public ActionResult Edit(int? id)
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
        public ActionResult Edit(ArticleCategory articlecategory)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {

                ArticleCategory cat = acm.Find(x => x.Id == articlecategory.Id);
                cat.Title = articlecategory.Title;

                acm.Update(articlecategory);

                CacheHelper.RemoveArticleCategoryFromCache();

                return RedirectToAction("Index");
            }
            return View(articlecategory);
        }


        public ActionResult Delete(int? id)
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
        public ActionResult DeleteConfirmed(int id)
        {
            ArticleCategory articlecategory = acm.Find(x => x.Id == id);
            acm.Delete(articlecategory);

            CacheHelper.RemoveArticleCategoryFromCache();

            return RedirectToAction("Index");
        }

    }
}