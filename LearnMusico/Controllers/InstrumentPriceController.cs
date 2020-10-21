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
    public class InstrumentPriceController : Controller
    {
        private InstrumentPriceManager priceManager = new InstrumentPriceManager();
        public ActionResult Index()
        {
            return View(priceManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MyInstrumentDetails()
        {
            var Iprice = priceManager.ListQueryable().Include("Owner").Where(
                            x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
                            x => x.ModifiedOn);
            return View(Iprice.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstrumentPrice instrumentPrice = priceManager.Find(x => x.Id == id.Value);
            if (instrumentPrice == null)
            {
                return HttpNotFound();
            }
            return View(instrumentPrice);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstrumentPrice instrumentPrice)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                instrumentPrice.Owner = CurrentSession.User;
                priceManager.Insert(instrumentPrice);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View(instrumentPrice);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstrumentPrice instrumentPrice = priceManager.Find(x => x.Id == id.Value);
            if (instrumentPrice == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View(instrumentPrice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InstrumentPrice instrumentPrice, HttpPostedFileBase ImageFilePath)
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
                    string filenameI = $"instrumentP_{instrumentPrice.Id}.{ImageFilePath.ContentType.Split('/')[1]}";

                    ImageFilePath.SaveAs(Server.MapPath($"~/img/instrument/{filenameI}"));
                    instrumentPrice.ImageFilePath = filenameI;
                }

                BusinessLayerResult<InstrumentPrice> res = priceManager.UpdateInstrumentPrice(instrumentPrice);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Enstrüman satış bilgileri güncellenemedi.",
                        RedirectingUrl = "/InstrumentPrice/Edit"
                    };

                    return View("Error", errorNotifyObj);
                }

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View(instrumentPrice);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstrumentPrice instrumentPrice = priceManager.Find(x => x.Id == id);
            if (instrumentPrice == null)
            {
                return HttpNotFound();
            }
            return View(instrumentPrice);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstrumentPrice instrumentPrice = priceManager.Find(x => x.Id == id);
            priceManager.Delete(instrumentPrice);
            return RedirectToAction("Index");
        }


    }
}