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
using LearnMusico.Filters;
using LearnMusico.Models;
using LearnMusico.ViewModels;

namespace LearnMusico.Controllers
{
    [Auth]
    public class ArticleController : Controller
    {
        private ArticleManager articleManager = new ArticleManager();

        
        [AuthTeacher]
        // GET: Article
        public ActionResult Index()
        {
            var articles = articleManager.ListQueryable().Include("ArticleCategory").Include("Owner").Where(
               x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
               x => x.ModifiedOn);
            return View(articles.ToList());
        }

        public ActionResult AllArticle(string deger)
        {
            var result = articleManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList();
            if (!string.IsNullOrEmpty(deger))
            {
                result = articleManager.ListQueryable().Where(x => x.Title.ToLower().Contains(deger.ToLower())).ToList();
                                      
            }
            //ViewBag.Search = "Girdiğiniz haber/yazı bulunamadı.";
            return View(result);
        }
        public ActionResult ByArticleCategory(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Article> article = articleManager.ListQueryable().Where(x => x.ArticleCategoryId == id).OrderByDescending(x => x.ModifiedOn).ToList();

            return View("AllArticle", article);

        }

        [AuthTeacher]
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

        [AuthTeacher]
        public ActionResult Create()
        {
            ViewBag.ArticleCategoryId = new SelectList(CacheHelper.GetArticleCategoryFromCache(), "Id", "Title");
            return View();
        }

        [AuthTeacher]
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

        [AuthTeacher]
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

        [AuthTeacher]
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

        [AuthTeacher]
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

        [AuthTeacher]
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
