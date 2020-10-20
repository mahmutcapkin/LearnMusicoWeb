using LearnMusico.BusinessLayer.Abstract;
using LearnMusico.BusinessLayer.Result;
using LearnMusico.Common.Helpers;
using LearnMusico.Entities;
using LearnMusico.Entities.ErrorMessage;
using LearnMusico.Entities.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnMusico.BusinessLayer
{
    public class MusicaUserManager: ManagerBase<MusicaUser>
    {
        public BusinessLayerResult<MusicaUser> RegisterUser(RegisterViewModel data)
        {
            //Kullanıcı username varmı  yok mu kontrolü
            //kullanıcı e-posta kontrolü
            // kayıt işlemi ve aktivasyon e-posta gönderimi
            MusicaUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<MusicaUser> res = new BusinessLayerResult<MusicaUser>();

            if (user != null)
            {
                //throw new Exception("Kayıtlı kullanıcı adı yada E-posta adresi");
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı kayıtlı");
                    //res.Errors.Add("Kullanıcı Adı kayıtlı");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta kayıtlı");
                }
            }
            else
            {
                int dbResult = base.Insert(new MusicaUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ProfileImageFilename = "user.png",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                    IsTeacher=false
                    //eklenecek adımlar var

                });
                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.Email && x.Username == data.Username);

                    //TODO: aktivasyon mail'ı atılacak
                    //layerResult.Result.ActivateGuid
                    string siteUri = ConfigHelper.Get<String>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username}; <br><br> Hesabınızı aktifleştirmek için <a href='{activateUri}'  target='_blank'>tıklayınız.</a>";

                    MailHelper.SendMail(body, res.Result.Email, "LearnMuscio Hesap Aktifleştirme");

                }

            }

            return res;

        }

        public BusinessLayerResult<MusicaUser> GetUserById(int id)
        {
            BusinessLayerResult<MusicaUser> res = new BusinessLayerResult<MusicaUser>();
            res.Result = Find(x => x.Id == id);
            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }
            return res;

        }


        public BusinessLayerResult<MusicaUser> LoginUser(LoginViewModel data)
        {
            //Giriş kontrolü 
            //hesap aktive edilmiş mi?
            //yönlendirme     burası UI katmanında  
            //session a kullanici bilgi saklama   burası UI katmanında 

            BusinessLayerResult<MusicaUser> res = new BusinessLayerResult<MusicaUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");
                    //res.Errors.Add("Kullanıcı aktifleştirilmemiştir.Lütfen e-posta adresinizi kontrol ediniz.");
                }

            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor");
                //res.Errors.Add("Kullanıcı adı ya da şifre uyuşmuyor");
            }
            return res;
        }


        public BusinessLayerResult<MusicaUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<MusicaUser> res = new BusinessLayerResult<MusicaUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı");
            }
            return res;
        }


        public BusinessLayerResult<MusicaUser> UpdateProfile(MusicaUser data)
        {
            MusicaUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<MusicaUser> res = new BusinessLayerResult<MusicaUser>();

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsTeacher = data.IsTeacher;
            res.Result.CV = data.CV;
            res.Result.About = data.About;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }
            if (string.IsNullOrEmpty(data.CV) == false)
            {
                res.Result.CV = data.CV;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<MusicaUser> RemoveUserById(int id)
        {
            BusinessLayerResult<MusicaUser> res = new BusinessLayerResult<MusicaUser>();
            MusicaUser user = Find(x => x.Id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }

            return res;
        }



        ////yönetim paneli için ekleme paneli işlemleri için geçerli // method hiding yapıldı
        public new BusinessLayerResult<MusicaUser> Insert(MusicaUser data)
        {
            //METHOD HIDING..

            MusicaUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<MusicaUser> res = new BusinessLayerResult<MusicaUser>();
            res.Result = data;

            if (user != null)
            {
                //throw new Exception("Kayıtlı kullanıcı adı yada E-posta adresi");
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı kayıtlı");
                    //res.Errors.Add("Kullanıcı Adı kayıtlı");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta kayıtlı");
                }
            }
            else
            {
                res.Result.ProfileImageFilename = "user_default.png";
                res.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi");
                }

            }
            return res;
        }

        public new BusinessLayerResult<MusicaUser> Update(MusicaUser data)
        {
            MusicaUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<MusicaUser> res = new BusinessLayerResult<MusicaUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
            res.Result.IsTeacher = data.IsTeacher;
            res.Result.CV = data.CV;
            res.Result.About = data.About;

            //resim güncelleme yapmak istemedi
            //if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            //{
            //    res.Result.ProfileImageFilename = data.ProfileImageFilename;
            //}

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }





    }
}
