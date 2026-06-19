using System;
using System.ComponentModel.DataAnnotations;

namespace MIDTest.Validation
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null) return new ValidationResult($"Không tìm thấy thuộc tính {_comparisonProperty}");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);

            if (value is DateTime endDate && comparisonValue is DateTime startDate)
            {
                if (endDate <= startDate)
                {
                    return new ValidationResult(ErrorMessage ?? "Ngày kết thúc phải lớn hơn ngày bắt đầu.");
                }
            }
            return ValidationResult.Success;
        }
    }
}