using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Validators
{
    public static class StringValidator
    {
        public static List<string> ValidateRequired(string value, string fieldName)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(value))
                errors.Add($"{fieldName} jest wymagany.");
            return errors;
        }

        public static List<string> ValidateMaxLength(string value, int maxLength, string fieldName)
        {
            var errors = new List<string>();
            if (!string.IsNullOrWhiteSpace(value) && value.Length > maxLength)
                errors.Add($"{fieldName} nie może mieć więcej niż {maxLength} znaków.");
            return errors;
        }
    }
}
