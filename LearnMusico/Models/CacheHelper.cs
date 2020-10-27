using LearnMusico.BusinessLayer;
using LearnMusico.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace LearnMusico.Models
{
    public class CacheHelper
    {
        public static List<ArticleCategory> GetArticleCategoryFromCache()
        {
            var result = WebCache.Get("acategory-cache");
            if (result == null)
            {
                ArticleCategoryManager acm = new ArticleCategoryManager();
                result = acm.List();
                WebCache.Set("acategory-cache", result, 20, true);
            }
            return result;
        }

        // anahtar değerin hatırlanmasıyla uğraşmak istenmezse bu yapılır
        public static void RemoveArticleCategoryFromCache()
        {
            Remove("acategory-cache");
        }
        // kategoriler güncellendiğinde eklendiğinde silindiğinde cache yapısının da güncellenmesi için 
        //cache temizlenir ve yukarıdaki GetCategoriesFromCache ile tekrar doldurlur
        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }

        public static List<InstrumentCategory> GetInstrumentCategoryFromCache()
        {
            var result = WebCache.Get("icategory-cache");
            if (result == null)
            {
                InstrumentCategoryManager icm = new InstrumentCategoryManager();
                result = icm.List();
                WebCache.Set("icategory-cache", result, 20, true);
            }
            return result;
        }

        // anahtar değerin hatırlanmasıyla uğraşmak istenmezse bu yapılır
        public static void RemoveInstrumentCategoryFromCache()
        {
            Remove("icategory-cache");
        }
        // kategoriler güncellendiğinde eklendiğinde silindiğinde cache yapısının da güncellenmesi için 
        //cache temizlenir ve yukarıdaki GetCategoriesFromCache ile tekrar doldurlur

    }
}