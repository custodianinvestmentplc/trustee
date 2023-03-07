using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json.Linq;

namespace Trustee_App.Validation;

public class DateOfBirthAttribute : ValidationAttribute, IClientModelValidator
{
    public int AgeLimit { get; }
    public int YearLimit { get; }

    public DateOfBirthAttribute(int year)
    {
        AgeLimit = year;
        YearLimit = DateTime.Today.AddYears(AgeLimit).Year;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-DateOfBirth", GetErrorMessage());

        //var year = Year.ToString(CultureInfo.InvariantCulture);
        var year = YearLimit.ToString(CultureInfo.InvariantCulture);

        MergeAttribute(context.Attributes, "data-val-DateOfBirth-year", year);
    }

    public string GetErrorMessage() => $"Birth year must be no later than {Math.Abs(YearLimit)}. \r\n Applicant must be above the age of {Math.Abs(AgeLimit)}.";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var birthYear = ((DateTime)value!).Year;

        if (birthYear > YearLimit)
        {
            return new ValidationResult(GetErrorMessage());
        }

        return ValidationResult.Success;
    }

    private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
    {
        if (attributes.ContainsKey(key))
        {
            return false;
        }

        attributes.Add(key, value);
        return true;
    }
}
