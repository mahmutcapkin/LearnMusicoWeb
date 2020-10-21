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
    public class LessonPriceController : Controller
    {
        private LessonPriceManager LpriceManager = new LessonPriceManager();
        public ActionResult Index()
        {
            return View(LpriceManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MyInstrumentDetails()
        {
            var Iprice = LpriceManager.ListQueryable().Include("Teacher").Where(
                            x => x.Teacher.Id == CurrentSession.User.Id).OrderByDescending(
                            x => x.ModifiedOn);
            return View(Iprice.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialLessonPrice specialLessonPrice = LpriceManager.Find(x => x.Id == id.Value);
            if (specialLessonPrice == null)
            {
                return HttpNotFound();
            }
            return View(specialLessonPrice);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SpecialLessonPrice specialLessonPrice)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                specialLessonPrice.Teacher = CurrentSession.User;
                LpriceManager.Insert(specialLessonPrice);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View(specialLessonPrice);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialLessonPrice instrumentPrice = LpriceManager.Find(x => x.Id == id.Value);
            if (instrumentPrice == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View(instrumentPrice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SpecialLessonPrice specialLessonPrice, HttpPostedFileBase ImageFilePath)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {

                if (ImageFilePath != null &&
                       (ImageFilePath.ContentType == "image/jpeg" ||
                        ImageFilePath.ContentType == "image/jpg" ||
                        ImageFilePath.ContentType == "image/png"))
                {
                    string filenameI = $"lessonP_{specialLessonPrice.Id}.{ImageFilePath.ContentType.Split('/')[1]}";

                    ImageFilePath.SaveAs(Server.MapPath($"~/img/lessonprice/{filenameI}"));
                    specialLessonPrice.ImageFilePath = filenameI;
                }

                BusinessLayerResult<SpecialLessonPrice> res = LpriceManager.UpdateLessonPrice(specialLessonPrice);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Özel ders bilgileri güncellenemedi.",
                        RedirectingUrl = "/LessonPrice/Edit"
                    };

                    return View("Error", errorNotifyObj);
                }

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View(specialLessonPrice);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialLessonPrice specialLessonPrice = LpriceManager.Find(x => x.Id == id);
            if (specialLessonPrice == null)
            {
                return HttpNotFound();
            }
            return View(specialLessonPrice);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SpecialLessonPrice specialLessonPrice = LpriceManager.Find(x => x.Id == id);
            LpriceManager.Delete(specialLessonPrice);
            return RedirectToAction("Index");
        }
    }
}