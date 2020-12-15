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
    public class InstrumentController : Controller
    {
        private InstrumentManager instrumentManager = new InstrumentManager();

        [Auth]
        [AuthTeacher]
        //Enstrumanlarım
        public ActionResult Index()
        {
            var instrument = instrumentManager.ListQueryable().Include("InstrumentCategory").Include("MusicaUser").Where(
               x => x.MusicaUser.Id == CurrentSession.User.Id).OrderByDescending(
               x => x.ModifiedOn);
            return View(instrument.ToList());
        }

        [Auth]
        public ActionResult ByInstrumentCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Instrument> instrument = instrumentManager.ListQueryable().Where(x => x.InstrumentCategoryId == id).OrderByDescending(x => x.ModifiedOn).ToList();

            return View("InstrumanAll", instrument);

        }


        [Auth]
        //tüm enstruman bilgileri
        public ActionResult InstrumanAll(string searchUserName)
        {
            var result = instrumentManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList();
            if (!string.IsNullOrEmpty(searchUserName))
            {
                result = instrumentManager.ListQueryable().Where(x => x.InstrumentName.ToLower().Contains(searchUserName.ToLower())).ToList();
            }
            //ViewBag.Search = "Girdiğiniz enstrüman bulunamadı.";
            return View(result);

        }

        [Auth]
        [AuthTeacher]
        // GET: Instrument/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = instrumentManager.Find(x=>x.Id==id.Value);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            return View(instrument);
        }

        [Auth]
        [AuthTeacher]
        // GET: Instrument/Create
        public ActionResult Create()
        {
            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View();
        }

        [Auth]
        [AuthTeacher]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Instrument instrument, HttpPostedFileBase VideoUrlPath, HttpPostedFileBase AudioUrlPath, HttpPostedFileBase ImageFilePath)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                instrument.MusicaUser = CurrentSession.User;
                instrument.ModifiedUsername = CurrentSession.User.Username;

                if (VideoUrlPath != null &&
                        (VideoUrlPath.ContentType == "video/mp4"))
                {
                    string filenameV = $"instrument_{Guid.NewGuid()}.{VideoUrlPath.ContentType.Split('/')[1]}";

                    VideoUrlPath.SaveAs(Server.MapPath($"~/videos/instrument/{filenameV}"));
                    instrument.VideoUrlPath = filenameV;
                }
                if (AudioUrlPath != null &&
                       (AudioUrlPath.ContentType == "audio/mpeg"))
                {
                    string filenameA = $"instrument_{Guid.NewGuid()}.{AudioUrlPath.ContentType.Split('/')[1]}";

                    AudioUrlPath.SaveAs(Server.MapPath($"~/audio/instrument/{filenameA}"));
                    instrument.AudioUrlPath = filenameA;
                }
                if (ImageFilePath != null &&
                       (ImageFilePath.ContentType == "image/jpeg" ||
                        ImageFilePath.ContentType == "image/jpg" ||
                        ImageFilePath.ContentType == "image/png"))
                {
                    string filenameI = $"instrument_{Guid.NewGuid()}.{ImageFilePath.ContentType.Split('/')[1]}";

                    ImageFilePath.SaveAs(Server.MapPath($"~/img/instrument/{filenameI}"));
                    instrument.ImageFilePath = filenameI;
                }
                instrumentManager.Insert(instrument);
                return RedirectToAction("Index");
            }

            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title", instrument.InstrumentCategoryId);
            return View(instrument);
        }

        // GET: Instrument/Edit/5
        [Auth]
        [AuthTeacher]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = instrumentManager.Find(x => x.Id == id.Value);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title", instrument.InstrumentCategoryId);
            return View(instrument);
        }

        [Auth]
        [AuthTeacher]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Instrument instrument, HttpPostedFileBase VideoUrlPath, HttpPostedFileBase AudioUrlPath, HttpPostedFileBase ImageFilePath)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                instrument.MusicaUser = CurrentSession.User;
                instrument.ModifiedUsername = CurrentSession.User.Username;
                if (VideoUrlPath != null &&
                       (VideoUrlPath.ContentType == "video/mp4"))
                {
                    string filenameV = $"instrument_{Guid.NewGuid()}.{VideoUrlPath.ContentType.Split('/')[1]}";

                    VideoUrlPath.SaveAs(Server.MapPath($"~/videos/instrument/{filenameV}"));
                    instrument.VideoUrlPath = filenameV;
                }
                if (AudioUrlPath != null &&
                       (AudioUrlPath.ContentType == "audio/mpeg"))
                {
                    string filenameA = $"instrument_{Guid.NewGuid()}.{AudioUrlPath.ContentType.Split('/')[1]}";

                    AudioUrlPath.SaveAs(Server.MapPath($"~/audio/instrument/{filenameA}"));
                    instrument.AudioUrlPath = filenameA;
                }
                if (ImageFilePath != null &&
                       (ImageFilePath.ContentType == "image/jpeg" ||
                        ImageFilePath.ContentType == "image/jpg" ||
                        ImageFilePath.ContentType == "image/png"))
                {
                    string filenameI = $"instrument_{Guid.NewGuid()}.{ImageFilePath.ContentType.Split('/')[1]}";

                    ImageFilePath.SaveAs(Server.MapPath($"~/img/instrument/{filenameI}"));
                    instrument.ImageFilePath = filenameI;
                }

                BusinessLayerResult<Instrument> res = instrumentManager.UpdateInstrument(instrument);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Enstrüman bilgileri güncellenemedi.",
                        RedirectingUrl = "/Instrument/Edit"
                    };

                    return View("Error", errorNotifyObj);
                }
                return RedirectToAction("Index");
            }
            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title", instrument.InstrumentCategoryId);
            return View(instrument);
        }

        // GET: Instrument/Delete/5
        [Auth]
        [AuthTeacher]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = instrumentManager.Find(x => x.Id == id.Value);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            return View(instrument);
        }

        // POST: Instrument/Delete/5
        [Auth]
        [AuthTeacher]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instrument instrument = instrumentManager.Find(x => x.Id == id);
            instrumentManager.Delete(instrument);
            return RedirectToAction("Index");
        }

    }
}
