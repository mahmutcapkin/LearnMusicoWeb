using LearnMusico.BusinessLayer;
using LearnMusico.BusinessLayer.Result;
using LearnMusico.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LearnMusico.Controllers
{
    public class MusicaUserController : Controller
    {

        private MusicaUserManager userManager = new MusicaUserManager();

        public ActionResult Index()
        {
            return View(userManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicaUser musicaUser = userManager.Find(x => x.Id == id.Value);
            if (musicaUser == null)
            {
                return HttpNotFound();
            }
            return View(musicaUser);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MusicaUser musicaUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<MusicaUser> res = userManager.Insert(musicaUser);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(musicaUser);
                }
                return RedirectToAction("Index");
            }

            return View(musicaUser);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicaUser musicaUser = userManager.Find(x => x.Id == id.Value);
            //todo: bakılacak
            if (musicaUser == null)
            {
                return HttpNotFound();
            }
            return View(musicaUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MusicaUser musicaUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<MusicaUser> res = userManager.Update(musicaUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(musicaUser);
                }

                return RedirectToAction("Index");
            }
            return View(musicaUser);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicaUser musicaUser = userManager.Find(x => x.Id == id.Value);
            if (musicaUser == null)
            {
                return HttpNotFound();
            }
            return View(musicaUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MusicaUser musicaUser = userManager.Find(x => x.Id == id);
            userManager.Delete(musicaUser);

            return RedirectToAction("Index");
        }


    }
}