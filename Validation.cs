﻿using System.Windows.Controls;

namespace Marian_Bianca_Lab6
{
    public class StringNotEmpty : ValidationRule
    {
        public override ValidationResult Validate(object value,
        System.Globalization.CultureInfo cultureinfo)
        {
            string aString = value.ToString();
            if (aString == "")
                return new ValidationResult(false, "String cannot be empty");
            return new ValidationResult(true, null);
        }
    }
    public class StringMinLengthValidator : ValidationRule
    {
        public override ValidationResult Validate(object value,
        System.Globalization.CultureInfo cultureinfo)
        {
            string aString = value.ToString();
            if (aString.Length < 3)
                return new ValidationResult(false, "String must have at least 3 characters!");
            return new ValidationResult(true, null);
        }
    }
}