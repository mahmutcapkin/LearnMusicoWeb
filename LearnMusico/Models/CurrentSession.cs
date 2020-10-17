using LearnMusico.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnMusico.Models
{
    public class CurrentSession
    {
        public static MusicaUser User
        {
            get
            {
                return Get<MusicaUser>("login");
            }

        }

        //verilen bir session anahtar ismine verdiğimiz tipte objeyi set ediyoruz
        public static void Set<T>(string key, T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }
        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                return (T)HttpContext.Current.Session[key];
            }
            //sınıf verilmişse null int  döner bool ise false string ise de null döner
            return default(T);

        }
        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}