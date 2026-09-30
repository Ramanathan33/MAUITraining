using System.Collections.Generic;
using System.Linq;
using JobCartAPI.Entities;

namespace JobCartAPI.Helpers
{
    public static class JobLookups
    {
        public const int AllStatusesId = -1;

        public static IReadOnlyList<LookupItem> ServiceTypes { get; } = new[]
        {
            new LookupItem(1, "Repair"),
            new LookupItem(2, "Installation"),
            new LookupItem(3, "Maintenance"),
            new LookupItem(4, "Inspection"),
            new LookupItem(5, "Warranty"),
        };

        public static IReadOnlyList<LookupItem> Statuses { get; } = new[]
        {
            new LookupItem(0, "Received"),
            new LookupItem(1, "In Progress"),
            new LookupItem(2, "Ready"),
            new LookupItem(3, "Delivered"),
            new LookupItem(4, "Cancelled"),
        };

        public static IReadOnlyList<LookupItem> StatusFilters { get; } =
            new[] { new LookupItem(AllStatusesId, "All statuses") }
                .Concat(Statuses)
                .ToList();

        public static LookupItem FindServiceType(int id) =>
            ServiceTypes.FirstOrDefault(item => item.Id == id);

        public static LookupItem FindStatus(int id) =>
            Statuses.FirstOrDefault(item => item.Id == id);

        public static string GetServiceTypeName(int id) =>
            FindServiceType(id)?.Name ?? "Not set";

        public static string GetStatusName(int id) =>
            FindStatus(id)?.Name ?? "Unknown";
    }
}
