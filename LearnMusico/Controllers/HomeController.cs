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
            if (ModelState.IsValid)
            {
                BusinessLayerResult<MusicaUser> res = musicaUserManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    // Hata koduna göre özel işlem yapmamız gerekirse..
                    // Hatta hata mesajına burada müdahale edilebilir.
                    // (Login.cshtml'deki kısmında açıklama satırı şeklinden kurtarınız)
                    //
                    //if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    //{
                    //    ViewBag.SetLink = "http://Home/Activate/1234-4567-78980";
                    //}

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }

                CurrentSession.Set<MusicaUser>("login", res.Result); // Session'a kullanıcı bilgi saklama..
                return RedirectToAction("Index");   // yönlendirme..
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<MusicaUser> res = musicaUserManager.RegisterUser(register);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(register);
                }

                OkViewModel ovm = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"

                };
                ovm.Items.Add(" Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz." +
                    " Hesabını aktive etmeden not eklemeyez ve beğenme yapamazsınız");

                return View("Ok", ovm);
            }


            return View(register);
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

                return View("Error", errorViewModel);
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
        public ActionResult EditProfile(MusicaUser model, HttpPostedFileBase ProfileImage, HttpPostedFileBase cv)
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

                    ProfileImage.SaveAs(Server.MapPath($"~/img/user/{filename}"));
                    model.ProfileImageFilename = filename;
                }
                
                if(cv!=null &&
                    (cv.ContentType == "application/pdf"))
                {
                    string filenameCV = $"usercv_{model.Id}.{cv.ContentType.Split('/')[1]}";
                    cv.SaveAs(Server.MapPath($"~/document/user/{filenameCV}"));
                    model.CV = filenameCV;
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