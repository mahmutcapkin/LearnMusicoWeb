using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LearnMusico.BusinessLayer;
using LearnMusico.Entities;
using LearnMusico.Models;

namespace LearnMusico.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleManager articleManager = new ArticleManager();
        ArticleCategoryManager articleCategoryManager = new ArticleCategoryManager();

        public ActionResult Index()
        {
            var articles = articleManager.ListQueryable().Include(a => a.ArticleCategory);
            return View(articles.ToList());
        }

        // GET: Article/Details/5
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

        // GET: Article/Create
        public ActionResult Create()
        {
            List<ArticleCategory> articleCategories = articleCategoryManager.List().ToList();

            ViewBag.ArticleCategoryId = new SelectList(articleCategoryManager.List(), "Id", "Title");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Article article)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                article.Owner = CurrentSession.User;
                articleManager.Insert(article);
                return RedirectToAction("Index");
            }

            ViewBag.ArticleCategoryId = new SelectList(articleCategoryManager.List(), "Id", "Title", article.ArticleCategoryId);
            return View(article);
        }

        // GET: Article/Edit/5
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
            ViewBag.ArticleCategoryId = new SelectList(articleCategoryManager.List(), "Id", "Title", article.ArticleCategoryId);
            return View(article);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(article).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleCategoryId = new SelectList(articleCategoryManager.List(), "Id", "Title", article.ArticleCategoryId);
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
