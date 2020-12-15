using LearnMusico.BusinessLayer.Abstract;
using LearnMusico.BusinessLayer.Result;
using LearnMusico.Entities;
using LearnMusico.Entities.ErrorMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnMusico.BusinessLayer
{
    public class ArticleManager: ManagerBase<Article>
    {
        public BusinessLayerResult<Article> UpdateArticle(Article data)
        {

            BusinessLayerResult<Article> res = new BusinessLayerResult<Article>();            
            res.Result = Find(x => x.Id == data.Id);

            res.Result.Title = data.Title;
            res.Result.Description = data.Description;
            res.Result.SubjectType = data.SubjectType;
            res.Result.MusicaUser = data.MusicaUser;
            res.Result.ModifiedUsername = data.ModifiedUsername;


            if (string.IsNullOrEmpty(data.ImageFileName) == false)
            {
                res.Result.ImageFileName = data.ImageFileName;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ArticleCouldNotUpdated, "Yazı güncellenemedi.");
            }

            return res;
        }
    }
}
