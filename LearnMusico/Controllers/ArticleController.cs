using LearnMusico.BusinessLayer;
using LearnMusico.Entities;
using LearnMusico.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LeararticleManagerusico.Controllers
{
    public class ArticleController : Controller
    {

        private ArticleManager articleManager = new ArticleManager();
        private ArticleCategoryManager acm = new ArticleCategoryManager();

        //Tüm Yazılar
        public ActionResult Index()
        {
            return View(articleManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }


        //Yazılarım
        public ActionResult MyArticles()
        {
          
            var articles = articleManager.ListQueryable().Include("ArticleCategory").Include("Owner").Where(
                x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
                x => x.ModifiedOn);
            return View(articles.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = articleManager.Find(x => x.Id == id.Value);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }


        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetArticleCategoryFromCache(), "Id", "Title");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Article Article)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Article.Owner = CurrentSession.User;
                articleManager.Insert(Article);
                return RedirectToAction("Index");
            }
            // cm.List() bu yapı çok değişken olmadığından ve bir çok yerde çaprıldığından cacheleme işlemi yapılacak 
            ViewBag.CategoryId = new SelectList(CacheHelper.GetArticleCategoryFromCache(), "Id", "Title");
            return View(Article);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = articleManager.Find(x => x.Id == id.Value);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetArticleCategoryFromCache(), "Id", "Title");
            return View(article);
        }

        //Article Edit içinde ekleme olabilir sonradan düzeltme vb.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Article article)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                Article db_article = articleManager.Find(x => x.Id == article.Id);
                db_article.Title = article.Title;
                db_article.SubjectType = article.SubjectType;
                db_article.Description = article.Description;

                if (string.IsNullOrEmpty(article.ImageFileName) == false)
                {
                    db_article.ImageFileName = article.ImageFileName;
                }
                articleManager.Update(db_article);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetArticleCategoryFromCache(), "Id", "Title");
            return View(article);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = articleManager.Find(x => x.Id == id.Value);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = articleManager.Find(x => x.Id == id);
            articleManager.Delete(article);
            return RedirectToAction("Index");
        }


    }
}