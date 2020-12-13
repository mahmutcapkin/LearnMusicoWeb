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
    public class SharingManager: ManagerBase<Sharing>
    {
        public BusinessLayerResult<Sharing> UpdateSharing(Sharing data)
        {

            BusinessLayerResult<Sharing> res = new BusinessLayerResult<Sharing>();

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Title = data.Title;
            res.Result.Description = data.Description;
            //gerekirse buraya diğer video resim ya da ses dosyası linki eklenecek

            if (string.IsNullOrEmpty(data.VideoUrlPath) == false)
            {
                res.Result.VideoUrlPath = data.VideoUrlPath;
            }
            if (string.IsNullOrEmpty(data.ImageUrlPath) == false)
            {
                res.Result.ImageUrlPath = data.ImageUrlPath;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.SharingCouldNotUpdated, "Paylaşım güncellenemedi.");
            }

            return res;
        }
    }

}

