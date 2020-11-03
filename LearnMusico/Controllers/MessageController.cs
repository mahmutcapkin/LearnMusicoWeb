using LearnMusico.BusinessLayer;
using LearnMusico.Entities;
using LearnMusico.Models;
using LearnMusico.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnMusico.Controllers
{
    public class MessageController : Controller
    {
        private MusicaUserManager userManager = new MusicaUserManager();
        private MessagesManager messagesManager = new MessagesManager();
        private MessageRepliesManager mRepliesManager = new MessageRepliesManager();

        public ActionResult Index()
        {
            MessageUserViewModel model = new MessageUserViewModel();

            //SelectList
            model.Users = new List<SelectListItem>();
            var userid = CurrentSession.User.Id;
            var users = userManager.List().Where(x => x.IsActive && x.Id!=userid).ToList();
            model.Users = users.Select(x => new SelectListItem()
            {
                Value=x.Id.ToString(),
                Text= string.Format("{0} {1} ({2})",x.Name,x.Surname,x.Username)
            }).ToList();
            //Get Select Message List
            var mList = messagesManager.List().Where(x => x.ToMusicaUserId == userid || x.MessageReplies.Any(y=>y.MusicaUserId==userid)).ToList();
            model.Messages = mList;



            return View(model);
        }


        public ActionResult SendMessage(SendMessageViewModel message)
        {
            Messages mesaj = new Messages()
            {
                Id=Guid.NewGuid(),
                AddedDate=DateTime.Now,
                Subject= message.Subject,
                ToMusicaUserId=message.ToUserId,

            };
            var mReplies = new MessageReplies()
            {
                    Id = Guid.NewGuid(),
                    AddedDate=DateTime.Now,
                    MusicaUserId=CurrentSession.User.Id,
                    Text=message.MessageBody
            };
            mesaj.MessageReplies.Add(mReplies);
            messagesManager.Insert(mesaj);

            return RedirectToAction("Index","Message");
        }

        [HttpGet]
        public ActionResult MessageReplies(string id)
        {
            MRepliesViewModel model = new MRepliesViewModel();
            var guid = new Guid(id);
            model.MReplies = mRepliesManager.List().Where(x => x.MessagesId == guid).OrderBy(x => x.AddedDate).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult MessageReplies(MessageReplies message)
        {
            message.AddedDate = DateTime.Now;
            message.Id = Guid.NewGuid();
            message.MusicaUserId = CurrentSession.User.Id;
            mRepliesManager.Insert(message);

            return RedirectToAction("MessageReplies","Message",new {id=message.MessagesId });
        }


        public ActionResult RemoveMessage(string id)
        {
            var guid = new Guid(id);
            var mReplies = mRepliesManager.List().Where(x => x.MessagesId == guid);
            foreach (MessageReplies rep  in mReplies)
            {
                mRepliesManager.Delete(rep);
            }
            var message = messagesManager.Find(s => s.Id == guid);
            messagesManager.Delete(message);


            return RedirectToAction("Index","Message");
        }


    }
}