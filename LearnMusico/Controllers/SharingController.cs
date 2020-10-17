using LearnMusico.BusinessLayer;
using LearnMusico.BusinessLayer.Result;
using LearnMusico.Entities;
using LearnMusico.Models;
using LearnMusico.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LearnMusico.Controllers
{
    public class SharingController : Controller
    {

        private SharingManager sharingManager = new SharingManager();
        private LikedManager likedManager = new LikedManager();

        public ActionResult Index()
        {
            return View(sharingManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }


        public ActionResult MySharings()
        {
            var sharings = sharingManager.ListQueryable().Include("Owner").Where(
                            x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
                            x => x.ModifiedOn);
            return View(sharings.ToList());
        }



        public ActionResult MostLiked()
        {
            SharingManager sm = new SharingManager();
            return View("Index", sm.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult MyLikedNotes()
        {
            var sharings = likedManager.ListQueryable().Include("LikedUser").Include("Sharing").Where(
                x => x.LikedUser.Id == CurrentSession.User.Id).Select(
                x => x.Sharing).OrderByDescending(
                x => x.ModifiedOn);

            return View("Index", sharings.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sharing sharing = sharingManager.Find(x => x.Id == id.Value);
            if (sharing == null)
            {
                return HttpNotFound();
            }
            return View(sharing);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sharing sharing)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                sharing.Owner = CurrentSession.User;
                sharingManager.Insert(sharing);
                return RedirectToAction("Index");
            }
            
            return View(sharing);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sharing sharing = sharingManager.Find(x => x.Id == id.Value);
            if (sharing == null)
            {
                return HttpNotFound();
            }

            return View(sharing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sharing sharing, HttpPostedFileBase VideoUrlPath)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            //if (ModelState.IsValid)
            //{
            //    Sharing db_sharing = sharingManager.Find(x => x.Id == sharing.Id);

            //    db_sharing.Description = sharing.Description;
            //    db_sharing.Title = sharing.Title;
            //    db_sharing.Title = sharing.Title;
            //    db_sharing.Title = sharing.Title;
            //    db_sharing.Title = sharing.Title;
            //    db_sharing.Title = sharing.Title;



            //    sharingManager.Update(db_sharing);

            //    return RedirectToAction("Index");
            //}

            //return View(sharing);
            if (ModelState.IsValid)
            {
                if (VideoUrlPath != null &&
                       (VideoUrlPath.ContentType == "video/mp4"))
                {
                    string filename = $"user_{sharing.Id}.{VideoUrlPath.ContentType.Split('/')[1]}";

                    VideoUrlPath.SaveAs(Server.MapPath($"~/videos/{filename}"));
                    sharing.VideoUrlPath = filename;
                }
                BusinessLayerResult<Sharing> res = sharingManager.UpdateSharing(sharing);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Paylaşım Güncellenemedi.",
                        RedirectingUrl = "/Sharing/Edit"
                    };

                    return View("Error", errorNotifyObj);
                }

                return RedirectToAction("Index");
            }
            return View(sharing);


        }



    }
}