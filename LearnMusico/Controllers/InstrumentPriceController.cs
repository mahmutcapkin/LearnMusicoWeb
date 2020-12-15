﻿using System;
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
    public class InstrumentPriceController : Controller
    {
        private InstrumentPriceManager priceManager = new InstrumentPriceManager();

        // benim satılık ürünlerim
        public ActionResult Index()
        {
            var instrumentPrices = priceManager.ListQueryable().Include("MusicaUser").Where(
                            x => x.MusicaUser.Id == CurrentSession.User.Id).OrderByDescending(
                            x => x.ModifiedOn);
            return View(instrumentPrices.ToList());
        }


        public ActionResult InstrumentPriceAll()
        {
            return View(priceManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByInstPriceCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<InstrumentPrice> instPrices = priceManager.ListQueryable().Where(x => x.InstrumentCategoryId == id).OrderByDescending(x => x.ModifiedOn).ToList();

            return View("InstrumentPriceAll", instPrices);

        }


        // GET: InstrumentPrice/Details/5
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

        // GET: InstrumentPrice/Create
        public ActionResult Create()
        {
            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstrumentPrice instrumentPrice, HttpPostedFileBase ImageFilePath)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                instrumentPrice.MusicaUser = CurrentSession.User;
                instrumentPrice.ModifiedUsername = CurrentSession.User.Username;
                if (ImageFilePath != null &&
                       (ImageFilePath.ContentType == "image/jpeg" ||
                        ImageFilePath.ContentType == "image/jpg" ||
                        ImageFilePath.ContentType == "image/png"))
                {
                    string filenameI = $"instrumentP_{Guid.NewGuid()}.{ImageFilePath.ContentType.Split('/')[1]}";

                    ImageFilePath.SaveAs(Server.MapPath($"~/img/instprice/{filenameI}"));
                    instrumentPrice.ImageFilePath = filenameI;
                }

                priceManager.Insert(instrumentPrice);
                return RedirectToAction("Index");
            }

            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title", instrumentPrice.InstrumentCategoryId);
            return View(instrumentPrice);
        }

        // GET: InstrumentPrice/Edit/5
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
            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title", instrumentPrice.InstrumentCategoryId);
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
                instrumentPrice.MusicaUser = CurrentSession.User;
                instrumentPrice.ModifiedUsername = CurrentSession.User.Username;
                if (ImageFilePath != null &&
                       (ImageFilePath.ContentType == "image/jpeg" ||
                        ImageFilePath.ContentType == "image/jpg" ||
                        ImageFilePath.ContentType == "image/png"))
                {
                    string filenameI = $"instrumentP_{Guid.NewGuid()}.{ImageFilePath.ContentType.Split('/')[1]}";

                    ImageFilePath.SaveAs(Server.MapPath($"~/img/instprice/{filenameI}"));
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
            ViewBag.InstrumentCategoryId = new SelectList(CacheHelper.GetInstrumentCategoryFromCache(), "Id", "Title", instrumentPrice.InstrumentCategoryId);
            return View(instrumentPrice);
        }

        // GET: InstrumentPrice/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: InstrumentPrice/Delete/5
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
