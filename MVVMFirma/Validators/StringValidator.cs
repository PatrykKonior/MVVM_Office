using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static List<string> ValidateEmail(string value, string fieldName)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.Add($"{fieldName} jest wymagany.");
            }
            else
            {
                // Regularne wyrażenie do walidacji adresu e-mail
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(value, emailPattern))
                {
                    errors.Add($"{fieldName} musi być poprawnym adresem e-mail.");
                }
            }
            return errors;
        }
        public static List<string> ValidatePhoneNumber(string value, string fieldName, int maxLength)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.Add($"{fieldName} jest wymagany.");
            }
            else
            {
                if (value.Length > maxLength)
                    errors.Add($"{fieldName} nie może mieć więcej niż {maxLength} cyfr.");

                if (!Regex.IsMatch(value, @"^\d+$"))
                    errors.Add($"{fieldName} może zawierać tylko cyfry.");
            }
            return errors;
        }
        public static List<string> ValidateIsNumeric(string value, string fieldName)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.Add($"{fieldName} jest wymagany.");
            }
            else if (!Regex.IsMatch(value, @"^\d+$"))
            {
                errors.Add($"{fieldName} może zawierać tylko cyfry.");
            }
            return errors;
        }
        public static List<string> ValidateExactLength(string value, int exactLength, string fieldName)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.Add($"{fieldName} jest wymagany.");
            }
            else if (value.Length != exactLength)
            {
                errors.Add($"{fieldName} musi mieć dokładnie {exactLength} znaków.");
            }
            return errors;
        }
        public static List<string> ValidateRegon(string value, string fieldName, int exactLength)
        {
            // Wykorzystanie jednej wiadomości błędu dla REGON.
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(value) || value.Length != exactLength || !Regex.IsMatch(value, @"^\d+$"))
            {
                errors.Add($"{fieldName} jest wymagany, musi mieć dokładnie {exactLength} cyfr i zawierać tylko cyfry.");
            }
            return errors;
        }
        public static List<string> ValidateNIP(string value, string fieldName, int exactLength)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(value) || value.Length != exactLength || !Regex.IsMatch(value, @"^\d+$"))
            {
                errors.Add($"{fieldName} jest wymagany, musi mieć dokładnie {exactLength} cyfr i zawierać tylko cyfry.");
            }
            return errors;
        }
    }
}
