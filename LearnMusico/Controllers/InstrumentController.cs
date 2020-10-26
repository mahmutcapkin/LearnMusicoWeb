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
    public class InstrumentController : Controller
    {
        private InstrumentManager instrumentManager = new InstrumentManager();

        //Enstrumanlarım
        public ActionResult Index()
        {
            var instrument = instrumentManager.ListQueryable().Include("InstrumentCategory").Include("Owner").Where(
               x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
               x => x.ModifiedOn);
            return View(instrument.ToList());
        }

        //tüm enstruman bilgileri
        public ActionResult InstrumanAll()
        {
            return View(instrumentManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }


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

        // GET: Instrument/Create
        public ActionResult Create()
        {
            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Instrument instrument, HttpPostedFileBase VideoUrlPath, HttpPostedFileBase AudioUrlPath, HttpPostedFileBase ImageFilePath)
        {
            if (ModelState.IsValid)
            {
                if (VideoUrlPath != null &&
                        (VideoUrlPath.ContentType == "video/mp4"))
                {
                    string filenameV = $"instrument_{instrument.Id}.{VideoUrlPath.ContentType.Split('/')[1]}";

                    VideoUrlPath.SaveAs(Server.MapPath($"~/videos/instrument/{filenameV}"));
                    instrument.VideoUrlPath = filenameV;
                }
                if (AudioUrlPath != null &&
                       (AudioUrlPath.ContentType == "audio/mp3"))
                {
                    string filenameA = $"instrument_{instrument.Id}.{AudioUrlPath.ContentType.Split('/')[1]}";

                    AudioUrlPath.SaveAs(Server.MapPath($"~/audio/instrument/{filenameA}"));
                    instrument.AudioUrlPath = filenameA;
                }
                if (ImageFilePath != null &&
                       (ImageFilePath.ContentType == "image/jpeg" ||
                        ImageFilePath.ContentType == "image/jpg" ||
                        ImageFilePath.ContentType == "image/png"))
                {
                    string filenameI = $"instrument_{instrument.Id}.{ImageFilePath.ContentType.Split('/')[1]}";

                    ImageFilePath.SaveAs(Server.MapPath($"~/img/instrument/{filenameI}"));
                    instrument.ImageFilePath = filenameI;
                }

                instrument.Owner = CurrentSession.User;
                instrumentManager.Insert(instrument);
                return RedirectToAction("Index");
            }

            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title", instrument.InstrumentCategoryId);
            return View(instrument);
        }

        // GET: Instrument/Edit/5
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Instrument instrument, HttpPostedFileBase VideoUrlPath, HttpPostedFileBase AudioUrlPath, HttpPostedFileBase ImageFilePath)
        {
            if (ModelState.IsValid)
            {
                if (VideoUrlPath != null &&
                       (VideoUrlPath.ContentType == "video/mp4"))
                {
                    string filenameV = $"instrument_{instrument.Id}.{VideoUrlPath.ContentType.Split('/')[1]}";

                    VideoUrlPath.SaveAs(Server.MapPath($"~/videos/instrument/{filenameV}"));
                    instrument.VideoUrlPath = filenameV;
                }
                if (AudioUrlPath != null &&
                       (AudioUrlPath.ContentType == "audio/mp3"))
                {
                    string filenameA = $"instrument_{instrument.Id}.{AudioUrlPath.ContentType.Split('/')[1]}";

                    AudioUrlPath.SaveAs(Server.MapPath($"~/audio/instrument/{filenameA}"));
                    instrument.AudioUrlPath = filenameA;
                }
                if (ImageFilePath != null &&
                       (ImageFilePath.ContentType == "image/jpeg" ||
                        ImageFilePath.ContentType == "image/jpg" ||
                        ImageFilePath.ContentType == "image/png"))
                {
                    string filenameI = $"instrument_{instrument.Id}.{ImageFilePath.ContentType.Split('/')[1]}";

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
