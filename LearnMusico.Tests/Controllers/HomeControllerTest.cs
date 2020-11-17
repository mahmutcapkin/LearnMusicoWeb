using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearnMusico;
using LearnMusico.Controllers;
using LearnMusico.Entities.ValueObject;
using LearnMusico.ViewModels;
//using LearnMusico.Controllers;

namespace LearnMusico.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
           
           HomeController controller = new HomeController();

          
           ViewResult result = controller.Index() as ViewResult;

            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Login()
        {

            HomeController controller = new HomeController();


            ViewResult result = controller.Login() as ViewResult;


            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Register()
        {

            HomeController controller = new HomeController();


            ViewResult result = controller.Register() as ViewResult;


            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void Register(RegisterViewModel model)
        {

            HomeController controller = new HomeController();
            OkViewModel okv = new OkViewModel();
            model.Email = "talha@gmail.com";
            model.Username = "talha123";
            model.Password = "12345";
            model.RePassword = "12345";

            var result = controller.Register(model) as ViewResult;
            if (result != null)
            {

                okv.Title = "Kayıt Başarılı";
                okv.RedirectingUrl = "/Home/Index";               
                okv.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz." +
                    " Hesabını aktive etmeden gönderi eklemeyez ve beğeni yapamazsınız");          
            }
            var deger = okv;

            Assert.AreEqual(deger.Title, "Kayıt Başarılı");
        }

        [TestMethod]
        public void Login(LoginViewModel model)
        {

            HomeController controller = new HomeController();
            
            model.Username = "talha123";
            model.Password = "12345";


            var result = controller.Login(model) as RedirectToRouteResult;
               
            Assert.AreEqual(result.RouteValues["action"], "Index");
        }



    }
}
