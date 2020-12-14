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
    public class LessonPriceManager: ManagerBase<SpecialLessonPrice>
    {
        public BusinessLayerResult<SpecialLessonPrice> UpdateLessonPrice(SpecialLessonPrice data)
        {

            BusinessLayerResult<SpecialLessonPrice> res = new BusinessLayerResult<SpecialLessonPrice>();

            res.Result = Find(x => x.Id == data.Id);
            res.Result.InstrumentName = data.InstrumentName;
            res.Result.Price = data.Price;
            res.Result.Address = data.Address;
            res.Result.Description = data.Description;
            res.Result.InstrumentCategoryId = data.InstrumentCategoryId;
            res.Result.Teacher = data.Teacher;

            if (string.IsNullOrEmpty(data.ImageFilePath) == false)
            {
                res.Result.ImageFilePath = data.ImageFilePath;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.LessonPriceCouldNotUpdated, "Özel ders bilgileri güncellenemedi.");
            }

            return res;
        }
    }
}
