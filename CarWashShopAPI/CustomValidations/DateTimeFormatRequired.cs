using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CarWashShopAPI.CustomValidations
{
    public class DateTimeFormatRequired : ValidationAttribute
    {
        private readonly string _format;

        public DateTimeFormatRequired(string format) : base($"Insert date in {format.ToUpper()} format")
        {
            _format = format;
        }
        public override bool IsValid(object value)
        {
            if (value is not string dateStr)
            {
                return false;
            }
            return DateTime.TryParseExact(dateStr, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
