using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Validators
{
    public static class DateValidator
    {
        public static List<string> ValidateNotInFuture(DateTime? value, string fieldName)
        {
            var errors = new List<string>();
            if (value == null)
                errors.Add($"{fieldName} jest wymagana.");
            else if (value > DateTime.Now)
                errors.Add($"{fieldName} nie może być w przyszłości.");
            return errors;
        }
    }
}
