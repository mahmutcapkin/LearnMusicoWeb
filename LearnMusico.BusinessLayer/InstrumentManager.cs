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
    public class InstrumentManager: ManagerBase<Instrument>
    {
        public BusinessLayerResult<Instrument> UpdateInstrument(Instrument data)
        {

            BusinessLayerResult<Instrument> res = new BusinessLayerResult<Instrument>();

            res.Result = Find(x => x.Id == data.Id);
            res.Result.InstrumentName = data.InstrumentName;
            res.Result.Description = data.Description;
            res.Result.InstrumentCategoryId = data.InstrumentCategoryId;
            //gerekirse buraya diğer video resim ya da ses dosyası linki eklenecek

            if (string.IsNullOrEmpty(data.AudioUrlPath) == false)
            {
                res.Result.AudioUrlPath = data.AudioUrlPath;
            }
            if (string.IsNullOrEmpty(data.ImageFilePath) == false)
            {
                res.Result.ImageFilePath = data.ImageFilePath;
            }

            if (string.IsNullOrEmpty(data.VideoUrlPath) == false)
            {
                res.Result.VideoUrlPath = data.VideoUrlPath;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.SharingCouldNotUpdated, "Enstrüman bilgileri güncellenemedi.");
            }

            return res;
        }
    }
}
