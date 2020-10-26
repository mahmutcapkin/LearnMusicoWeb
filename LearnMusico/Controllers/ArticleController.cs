using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LearnMusico.BusinessLayer;
using LearnMusico.BusinessLayer.Result;
using LearnMusico.Entities;
using LearnMusico.Models;
using LearnMusico.ViewModels;

namespace LearnMusico.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleManager articleManager = new ArticleManager();

        // GET: Article
        public ActionResult Index()
        {
            var articles =articleManager.ListQueryable().Include(a => a.ArticleCategory);
            return View(articles.ToList());
        }

        // GET: Article/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = articleManager.Find(x=>x.Id==id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Article/Create
        public ActionResult Create()
        {
            ViewBag.ArticleCategoryId = new SelectList(CacheHelper.GetArticleCategoryFromCache(), "Id", "Title");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Article article,HttpPostedFileBase ImageFileName)
        {
            if (ModelState.IsValid)
            {
                if (ImageFileName != null &&
                      (ImageFileName.ContentType == "image/jpeg" ||
                       ImageFileName.ContentType == "image/jpg" ||
                       ImageFileName.ContentType == "image/png"))
                {
                    string filenameA = $"article_{article.Id}.{ImageFileName.ContentType.Split('/')[1]}";

                    ImageFileName.SaveAs(Server.MapPath($"~/img/instrument/{filenameA}"));
                    article.ImageFileName = filenameA;
                }

                article.Owner = CurrentSession.User;
                articleManager.Insert(article);
                return RedirectToAction("Index");
            }

            ViewBag.ArticleCategoryId = new SelectList(CacheHelper.GetArticleCategoryFromCache(), "Id", "Title", article.ArticleCategoryId);
            return View(article);
        }

        // GET: Article/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = articleManager.Find(x => x.Id == id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleCategoryId = new SelectList(CacheHelper.GetArticleCategoryFromCache(), "Id", "Title", article.ArticleCategoryId);
            return View(article);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Article article,HttpPostedFile ImageFileName)
        {
            if (ModelState.IsValid)
            {
                if (ImageFileName != null &&
                         (ImageFileName.ContentType == "image/jpeg" ||
                         ImageFileName.ContentType == "image/jpg" ||
                         ImageFileName.ContentType == "image/png"))
                {
                    string filename = $"article_{article.Id}.{ImageFileName.ContentType.Split('/')[1]}";

                    ImageFileName.SaveAs(Server.MapPath($"~/img/article/{filename}"));
                    article.ImageFileName = filename;
                }
                BusinessLayerResult<Article> res = articleManager.UpdateArticle(article);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Yazı Güncellenemedi.",
                        RedirectingUrl = "/Article/Edit"
                    };

                    return View("Error", errorNotifyObj);
                }

                return RedirectToAction("Index");
            }
            ViewBag.ArticleCategoryId = new SelectList(CacheHelper.GetArticleCategoryFromCache(), "Id", "Title", article.ArticleCategoryId);
            return View(article);
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = articleManager.Find(x => x.Id == id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Article/Delete/5
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
