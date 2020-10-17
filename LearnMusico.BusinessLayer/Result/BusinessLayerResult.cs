using LearnMusico.Entities.ErrorMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnMusico.BusinessLayer.Result
{
    public class BusinessLayerResult<T> where T:class
    {
        public List<ErrorMessageObject> Errors { get; set; }
        public T Result { get; set; }
        public BusinessLayerResult()
        {
            Errors = new List<ErrorMessageObject>();
            //aşağıdaki yapı çok uzun ve kullanışlı olmadğından bu sınıfa bir method oluşturduk
            //Errors.Add(new KeyValuePair<ErrorMessageCode, string>(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı."));
        }
        public void AddError(ErrorMessageCode code, string message)
        {
            Errors.Add(new ErrorMessageObject() { Code = code, Message = message });
        }
    }
}
