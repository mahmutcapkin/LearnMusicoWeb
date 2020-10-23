using LearnMusico.BusinessLayer;
using LearnMusico.BusinessLayer.Result;
using LearnMusico.Entities;
using LearnMusico.Models;
using LearnMusico.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LearnMusico.Controllers
{
    public class InstrumentController : Controller
    {
        private InstrumentManager _instrumentManager = new InstrumentManager();
        private InstrumentCategoryManager categoryManager = new InstrumentCategoryManager();

        //tüm enstruman bilgileri
        public ActionResult Index()
        {
            return View(_instrumentManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        // enstruman bilgilerim eğitmenin eklediği
        public ActionResult MyInstrumentDetails()
        {
            var instruments = _instrumentManager.ListQueryable().Include("Owner").Where(
                            x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
                            x => x.ModifiedOn);
            return View(instruments.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = _instrumentManager.Find(x => x.Id == id.Value);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            return View(instrument);
        }

        public ActionResult Create()
        {
           // List<InstrumentCategory> instrumentCategories = categoryManager.List();


            ViewBag.InstrumentCategoryId = new SelectList(categoryManager.List(), "Id", "Title");
            // ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Instrument instrument)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                instrument.Owner = CurrentSession.User;
                _instrumentManager.Insert(instrument);
                return RedirectToAction("Index");
            }
            ViewBag.InstrumentCategoryId = new SelectList(categoryManager.List(), "Id", "Title");
            //ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View(instrument);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = _instrumentManager.Find(x => x.Id == id.Value);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstrumentCategoryId = new SelectList(categoryManager.List(), "Id", "Title");
            //ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View(instrument);
        }


        //Instrument Edit içinde ekleme olabilir sonradan

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Instrument instrument, HttpPostedFileBase VideoUrlPath, HttpPostedFileBase AudioUrlPath, HttpPostedFileBase ImageFilePath)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

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

                BusinessLayerResult<Instrument> res = _instrumentManager.UpdateInstrument(instrument);

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
            ViewBag.InstrumentCategoryId = new SelectList(categoryManager.List(), "Id", "Title");
            //ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View(instrument);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = _instrumentManager.Find(x => x.Id == id);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            return View(instrument);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instrument instrument = _instrumentManager.Find(x => x.Id == id);
            _instrumentManager.Delete(instrument);
            return RedirectToAction("Index");
        }


    }
}