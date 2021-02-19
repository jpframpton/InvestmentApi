using Microsoft.AspNetCore.Http;
using System;

namespace InvestmentApi.Common
{
    public static class Validation
    {
        public static DateTime? GetStartDateValue(string startDate)
        {
            if (string.IsNullOrEmpty(startDate)) return null;

            if (DateTime.TryParse(startDate, out var result)) return result;

            throw new BadHttpRequestException("Start date is invalid");
        }

        public static DateTime? GetEndDateValue(string endDate)
        {
            if (string.IsNullOrEmpty(endDate)) return null;

            if (DateTime.TryParse(endDate, out var result)) return result;

            throw new BadHttpRequestException("End date is invalid");
        }

        public static void CheckDateRange(DateTime startDate, DateTime endDate, int maxDaysInRange)
        {
            if ((endDate - startDate).TotalDays > maxDaysInRange)
                throw new BadHttpRequestException(
                    $"Date range between start and end date is greater than {maxDaysInRange} days.");
        }

        public static (DateTime? StartDateValue, DateTime? EndDateValue) ValidateDates(string startDate, string endDate, int maxDaysInDateRange)
        {
            var startDateValue = Validation.GetStartDateValue(startDate);
            var endDateValue = Validation.GetEndDateValue(endDate);
            if (startDateValue.HasValue && endDateValue.HasValue)
            {
                Validation.CheckDateRange(startDateValue.Value, endDateValue.Value, maxDaysInDateRange);
            }
            return (startDateValue, endDateValue);
        }
    }
}