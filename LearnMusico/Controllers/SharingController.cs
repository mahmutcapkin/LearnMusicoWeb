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
using LearnMusico.Entities.ErrorMessage;
using LearnMusico.Models;
using LearnMusico.ViewModels;

namespace LearnMusico.Controllers
{
    public class SharingController : Controller
    {
        private SharingManager sharingManager = new SharingManager();
        private LikedManager likedManager = new LikedManager();

        //Gönderilerim
        public ActionResult Index()
        {
            var sharings = sharingManager.ListQueryable().Include("Owner").Where(
                           x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
                           x => x.ModifiedOn);
            return View(sharings.ToList());
        }


        //Tüm Gönderiler
        public ActionResult AllSharings()
        {
            return View(sharingManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            SharingManager sm = new SharingManager();
            return View("AllSharings", sm.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult MyLikedSharings()
        {
            var sharings = likedManager.ListQueryable().Include("LikedUser").Include("Sharing").Where(
                x => x.LikedUser.Id == CurrentSession.User.Id).Select(
                x => x.Sharing).OrderByDescending(
                x => x.ModifiedOn);

            return View("AllSharings", sharings.ToList());
        }



        // GET: Sharing/Details/5
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

        // GET: Sharing/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sharing sharing, HttpPostedFileBase VideoUrlPath, HttpPostedFileBase ImageUrlPath)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (VideoUrlPath != null &&
                                     (VideoUrlPath.ContentType == "video/mp4"))
                {
                    string filenameV = $"sharingV_{sharing.Id}.{VideoUrlPath.ContentType.Split('/')[1]}";

                    VideoUrlPath.SaveAs(Server.MapPath($"~/videos/sharing/{filenameV}"));
                    sharing.VideoUrlPath = filenameV;
                }
                if (ImageUrlPath != null &&
                      (ImageUrlPath.ContentType == "image/jpeg" ||
                       ImageUrlPath.ContentType == "image/jpg" ||
                       ImageUrlPath.ContentType == "image/png"))
                {
                    string filenameI = $"sharingI_{sharing.Id}.{ImageUrlPath.ContentType.Split('/')[1]}";

                    ImageUrlPath.SaveAs(Server.MapPath($"~/img/sharing/{filenameI}"));
                    sharing.ImageUrlPath = filenameI;
                }

                sharing.Owner = CurrentSession.User;
                sharingManager.Insert(sharing);
                return RedirectToAction("Index");
            }
            return View(sharing);
        }

        // GET: Sharing/Edit/5
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
        public ActionResult Edit(Sharing sharing, HttpPostedFileBase VideoUrlPath, HttpPostedFileBase ImageUrlPath)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (VideoUrlPath != null &&
                        (VideoUrlPath.ContentType == "video/mp4"))
                {
                    string filename = $"sharingV_{sharing.Id}.{VideoUrlPath.ContentType.Split('/')[1]}";

                    VideoUrlPath.SaveAs(Server.MapPath($"~/videos/sharing/{filename}"));
                    sharing.VideoUrlPath = filename;
                }
                if (ImageUrlPath != null &&
                     (ImageUrlPath.ContentType == "image/jpeg" ||
                      ImageUrlPath.ContentType == "image/jpg" ||
                      ImageUrlPath.ContentType == "image/png"))
                {
                    string filenameI = $"sharingI_{sharing.Id}.{ImageUrlPath.ContentType.Split('/')[1]}";

                    ImageUrlPath.SaveAs(Server.MapPath($"~/img/sharing/{filenameI}"));
                    sharing.ImageUrlPath = filenameI;
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

        // GET: Sharing/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sharing sharing = sharingManager.Find(x => x.Id == id);
            if (sharing == null)
            {
                return HttpNotFound();
            }
            return View(sharing);
        }

        // POST: Sharing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sharing sharing = sharingManager.Find(x => x.Id == id);
            sharingManager.Delete(sharing);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            //List<int> likedSharingIds = likedManager.List(x => x.LikedUser.Id == CurrentSession.User.Id && ids.Contains(x.Sharing.Id)).Select(x => x.Sharing.Id).ToList();

            //return Json( new {result=likedSharingIds});
            if (CurrentSession.User != null)
            {
                int userid = CurrentSession.User.Id;
                List<int> likedNoteIds = new List<int>();

                if (ids != null)
                {
                    likedNoteIds = likedManager.List(
                        x => x.LikedUser.Id == userid && ids.Contains(x.Sharing.Id)).Select(
                        x => x.Sharing.Id).ToList();
                }
                else
                {
                    likedNoteIds = likedManager.List(
                        x => x.LikedUser.Id == userid).Select(
                        x => x.Sharing.Id).ToList();
                }

                return Json(new { result = likedNoteIds });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }
        }

        public ActionResult LikedState(int sharingid,bool liked)
        {
            int res = 0;

            if (CurrentSession.User == null)
                return Json(new { hasError = true, errorMessage = "Beğenme işlemi için giriş yapmalısınız.", result = 0 });

            Liked like = likedManager.Find(x => x.Sharing.Id == sharingid && x.LikedUser.Id == CurrentSession.User.Id);
            Sharing sharing = sharingManager.Find(x => x.Id == sharingid);

            if(like !=null && liked==false)
            {
                res = likedManager.Delete(like);
            }
            else if(like==null && liked==true)
            {
                res = likedManager.Insert(new Liked()
                {
                    LikedUser = CurrentSession.User,
                    Sharing = sharing
                }); 
            }

            if(res>0)
            {
                if(liked)
                {
                    sharing.LikeCount++;
                }
                else
                {
                    sharing.LikeCount--;
                }
                res = sharingManager.Update(sharing);
                return Json(new { hasError = false, errorMessage = string.Empty, result = sharing.LikeCount });

            }
            return Json(new { hasError = true, errorMessage = "Beğenme işlemi gerçekleştirilemedi.", result = sharing.LikeCount });

        }


    }
}
