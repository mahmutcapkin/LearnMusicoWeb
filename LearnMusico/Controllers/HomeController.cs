using LearnMusico.BusinessLayer;
using LearnMusico.BusinessLayer.Result;
using LearnMusico.Entities;
using LearnMusico.Entities.ValueObject;
using LearnMusico.Models;
using LearnMusico.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnMusico.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private MusicaUserManager musicaUserManager = new MusicaUserManager();

        public ActionResult Index()
        {
            Test test = new Test();
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel deger)
        {
            return View();
        }

        public ActionResult UserActivate(Guid id)
        {
            //Kullanıcı aktivasyonu sağlanacak          
            BusinessLayerResult<MusicaUser> res = musicaUserManager.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors;
                ErrorViewModel errorViewModel = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };

                return View("Errror", errorViewModel);
            }
            OkViewModel ovm = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/Login"
            };
            ovm.Items.Add("Hesabınız aktifleştirildi. Artık not paylaşabilir beğeni yapabilirsiniz.");

            return View("Ok", ovm);
        }

        public ActionResult ShowProfile()
        {

            BusinessLayerResult<MusicaUser> res = musicaUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                //TODO: kullanıcıyı bir hata ekranına yönlendirmek gerekiyor..
                ErrorViewModel errorViewModel = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorViewModel);
            }

            return View(res.Result);
        }

        public ActionResult EditProfile()
        {


            BusinessLayerResult<MusicaUser> res = musicaUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                //TODO: kullanıcıyı bir hata ekranına yönlendirmek gerekiyor..
                ErrorViewModel errorViewModel = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorViewModel);
            }

            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(MusicaUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                        (ProfileImage.ContentType == "image/jpeg" ||
                        ProfileImage.ContentType == "image/jpg" ||
                        ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }
                BusinessLayerResult<MusicaUser> res = musicaUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }
                //Profil güncellendiği için session güncellendi
                CurrentSession.Set<MusicaUser>("login", res.Result);

                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }



    }
}