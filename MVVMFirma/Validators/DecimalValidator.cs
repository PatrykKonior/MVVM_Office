using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Validators
{
    public static class DecimalValidator
    {
        public static List<string> ValidateGreaterThanZero(decimal? value, string fieldName)
        {
            var errors = new List<string>();
            if (value == null || value <= 0)
                errors.Add($"{fieldName} musi być większa od zera.");
            return errors;
        }

        public static List<string> ValidateNotNegative(decimal? value, string fieldName)
        {
            var errors = new List<string>();
            if (value < 0)
                errors.Add($"{fieldName} nie może być ujemne.");
            return errors;
        }

        public static List<string> ValidateNotGreaterThan(decimal? value, decimal? limit, string fieldName, string limitFieldName)
        {
            var errors = new List<string>();
            if (value != null && limit != null && value > limit)
                errors.Add($"{fieldName} nie może być większe niż {limitFieldName}.");
            return errors;
        }

        public static List<string> ValidateInRange(decimal? value, decimal min, decimal max, string fieldName)
        {
            var errors = new List<string>();
            if (value == null || value < min || value > max)
                errors.Add($"{fieldName} musi być w zakresie od {min} do {max}.");
            return errors;
        }
    }
}
