using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using TrusteeApp.Domain.Dtos;

namespace Trustee_App.Validation
{
    public class CustomDateValidationAttribute : ValidationAttribute
    {
        private DateTime _MinDate;
        private DateTime _MaxDate;
        private int _MaxValue;
        private int _MinValue;
        private int _Value;
        private int _Today;
        private DateTime _CurrentDate = DateTime.Today;

        private bool _IsYear;
        private bool _IsLater;
        private string _CustomErrorMessage;
        private int _MaxLimit;
        private int _MinLimit;
        //private string _errorValueString = "";

        public CustomDateValidationAttribute(bool isYear, int maxLimit, int minLimit, bool isLater = false, string CustomErrorMessage = null)
        {
            _MinLimit = minLimit;
            _MaxLimit = maxLimit;
            _IsYear = isYear;
            _IsLater = isLater;
            _CustomErrorMessage = CustomErrorMessage;

            //if (isYear && _CustomErrorMessage != null)
            //{
            //    _errorValueString = "years";
            //}
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (_IsYear)
            {
                _Value = ((DateTime)value!).Year;

                if (_MaxLimit != 0)
                {
                    _MaxValue = DateTime.Today.AddYears(_MaxLimit).Year;

                    if (_Value > _MaxValue) return new ValidationResult(GetErrorMessage());
                }

                if (_MinLimit != 0)
                {
                    _MinValue = DateTime.Today.AddYears(_MinLimit).Year;

                    if (_Value < _MinValue) return new ValidationResult(GetErrorMessage());
                }
            }
            else
            {
                var dateValue = ((DateTime)value!).Day;

                _Today = DateTime.Now.Day;

                if (dateValue < _Today) return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            if (_CustomErrorMessage != null)
            {
                return _CustomErrorMessage;
            }

            if (_MinLimit != 0 && _MaxLimit != 0)
            {
                return $"Applicant age must be between {Math.Abs(_MinLimit)} and {Math.Abs(_MaxLimit)}.";
            }

            else if (_MinLimit != 0 || _MaxLimit != 0)
            {
                return _MinLimit != 0 ? $"Applicant must be below the age of {Math.Abs((_MinLimit))}..." : $"Applicant must be above the age of {Math.Abs((_MaxLimit))}...";
            }

            else
            {
                if (_IsLater) return $"Please enter a date later than today.";

                return $"Please enter a date not later than today.";
            }
        }

    }
}


//var contactDetails = (UserContactDetails)validationContext.ObjectInstance;

//protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//{
//    DateTime dateValue = DateTime.Parse(value.ToString(), CultureInfo.InvariantCulture);

//    int camparedResult = dateValue.CompareTo(_MaxDate);

//    if (camparedResult > 0)
//    {
//        return new ValidationResult(ErrorMessage);
//    }
//    else if (camparedResult < 0)
//    {
//        return ValidationResult.Success;
//    }
//    else
//    {
//        return ValidationResult.Success;
//    }
//}
