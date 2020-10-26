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
    public class SpecialLessonPriceController : Controller
    {
        private LessonPriceManager LpriceManager = new LessonPriceManager();

        // özel derslerim 
        public ActionResult Index()
        {
            var specialLessonPrices = LpriceManager.ListQueryable().Include("Teacher").Where(
                            x => x.Teacher.Id == CurrentSession.User.Id).OrderByDescending(
                            x => x.ModifiedOn);
            return View(specialLessonPrices.ToList());
        }

        //Tüm özeldersler 

        public ActionResult AllSpecialLesson()
        {
            return View(LpriceManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }


        // GET: SpecialLessonPrice/Details/5
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

        // GET: SpecialLessonPrice/Create
        public ActionResult Create()
        {
            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SpecialLessonPrice specialLessonPrice,HttpPostedFileBase ImageFilePath)
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
                    string filenameL = $"lessonP_{specialLessonPrice.Id}.{ImageFilePath.ContentType.Split('/')[1]}";

                    ImageFilePath.SaveAs(Server.MapPath($"~/img/lessonprice/{filenameL}"));
                    specialLessonPrice.ImageFilePath = filenameL;
                }
                return RedirectToAction("Index");
            }

            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title", specialLessonPrice.InstrumentCategoryId);
            return View(specialLessonPrice);
        }

        // GET: SpecialLessonPrice/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title", specialLessonPrice.InstrumentCategoryId);
            return View(specialLessonPrice);
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
                    string filenameL = $"lessonP_{specialLessonPrice.Id}.{ImageFilePath.ContentType.Split('/')[1]}";

                    ImageFilePath.SaveAs(Server.MapPath($"~/img/lessonprice/{filenameL}"));
                    specialLessonPrice.ImageFilePath = filenameL;
                }

                BusinessLayerResult<SpecialLessonPrice> res = LpriceManager.UpdateLessonPrice(specialLessonPrice);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Özel ders bilgileri güncellenemedi.",
                        RedirectingUrl = "/SpecialLessonPrice/Edit"
                    };

                    return View("Error", errorNotifyObj);
                }
                return RedirectToAction("Index");
            }
            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title", specialLessonPrice.InstrumentCategoryId);
            return View(specialLessonPrice);
        }

        // GET: SpecialLessonPrice/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: SpecialLessonPrice/Delete/5
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
