using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace JobCartAPI.Validation
{
    public static class JobCardRules
    {
        public static string Validate(string customerName, string modelNo, string mobileNo, int? serviceTypeId, DateTime serviceDate, DateTime deliveryDate)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return "Customer name is required.";

            if (serviceTypeId == null || serviceTypeId < 1)
                return "Select a type of service.";

            if (string.IsNullOrWhiteSpace(modelNo))
                return "Model number is required.";

            if (!IsValidMobile(mobileNo))
                return "Enter a valid mobile number, or leave it blank.";

            if (deliveryDate.Date < serviceDate.Date)
                return "Delivery date cannot be earlier than the service date.";

            return null;
        }

        public static bool IsValidMobile(string mobileNo)
        {
            if (string.IsNullOrWhiteSpace(mobileNo))
                return true;

            var digits = 0;
            foreach (var character in mobileNo)
            {
                if (char.IsDigit(character))
                    digits++;
            }

            return digits >= 7 && digits <= 15;
        }

        public static DateTime ParseDateOrToday(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DateTime.Today;

            var formats = new[]
            {
                "yyyy-MM-dd",
                "yyyy-MM-dd HH:mm",
                "M/d/yyyy",
                "MM/dd/yyyy",
            };

            if (DateTime.TryParseExact(value.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var parsed))
                return parsed.Date;

            if (DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out parsed))
                return parsed.Date;

            return DateTime.Today;
        }

        public static IReadOnlyList<T> PrepareVisibleJobs<T>(IEnumerable<T> jobs, int statusFilterId, Func<T, int> statusSelector, Func<T, string> nameSelector)
        {
            if (statusSelector == null)
                throw new ArgumentNullException(nameof(statusSelector));
            if (nameSelector == null)
                throw new ArgumentNullException(nameof(nameSelector));

            IEnumerable<T> query = jobs ?? Array.Empty<T>();
            if (statusFilterId >= 0)
                query = query.Where(job => statusSelector(job) == statusFilterId);

            return query
                .OrderBy(job => nameSelector(job) ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
