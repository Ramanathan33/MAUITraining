using JobCartAPI.Validation;

namespace JobCartAPI.Tests
{
    public class JobCardRulesTests
    {
        private sealed record Row(string Name, int Status);

        [Fact]
        public void Validate_AcceptsACompleteJob()
        {
            var today = DateTime.Today;
            var error = JobCardRules.Validate("Ada Lovelace", "MX-10", "(415) 555-1212", 1, today, today);

            Assert.Null(error);
        }

        [Fact]
        public void Validate_RequiresCustomerName()
        {
            var error = JobCardRules.Validate("  ", "MX-10", null, 1, DateTime.Today, DateTime.Today);

            Assert.Equal("Customer name is required.", error);
        }

        [Fact]
        public void Validate_RequiresServiceType()
        {
            var error = JobCardRules.Validate("Ada", "MX-10", null, 0, DateTime.Today, DateTime.Today);

            Assert.Equal("Select a type of service.", error);
        }

        [Fact]
        public void Validate_RequiresModelNumber()
        {
            var error = JobCardRules.Validate("Ada", " ", null, 2, DateTime.Today, DateTime.Today);

            Assert.Equal("Model number is required.", error);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("abc")]
        [InlineData("1234567890123456")]
        public void Validate_RejectsInvalidMobile(string mobile)
        {
            var error = JobCardRules.Validate("Ada", "MX-10", mobile, 1, DateTime.Today, DateTime.Today);

            Assert.Equal("Enter a valid mobile number, or leave it blank.", error);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123-4567")]
        [InlineData("+1 (415) 555-1212")]
        public void Validate_AllowsBlankOrDialableMobile(string mobile)
        {
            var error = JobCardRules.Validate("Ada", "MX-10", mobile, 1, DateTime.Today, DateTime.Today);

            Assert.Null(error);
        }

        [Fact]
        public void Validate_RejectsDeliveryBeforeService()
        {
            var serviceDate = new DateTime(2024, 5, 10);
            var error = JobCardRules.Validate("Ada", "MX-10", null, 1, serviceDate, serviceDate.AddDays(-1));

            Assert.Equal("Delivery date cannot be earlier than the service date.", error);
        }

        [Fact]
        public void ParseDateOrToday_ReadsStoredIsoDate()
        {
            var parsed = JobCardRules.ParseDateOrToday("2024-03-15");

            Assert.Equal(new DateTime(2024, 3, 15), parsed);
        }

        [Fact]
        public void ParseDateOrToday_UsesTodayWhenMissing()
        {
            Assert.Equal(DateTime.Today, JobCardRules.ParseDateOrToday(null));
            Assert.Equal(DateTime.Today, JobCardRules.ParseDateOrToday("not-a-date"));
        }

        [Fact]
        public void PrepareVisibleJobs_OrdersByNameWithoutDuplicating()
        {
            var source = new List<Row>
            {
                new Row("B", 0),
                new Row("a", 1),
                new Row(null, 2),
            };

            var visible = JobCardRules.PrepareVisibleJobs(source, -1, row => row.Status, row => row.Name);

            Assert.Equal(3, source.Count);
            Assert.Equal(new[] { null, "a", "B" }, visible.Select(row => row.Name).ToArray());
        }

        [Fact]
        public void PrepareVisibleJobs_StatusZeroKeepsReceivedJobs()
        {
            var source = new[]
            {
                new Row("A", 0),
                new Row("B", 1),
            };

            var visible = JobCardRules.PrepareVisibleJobs(source, 0, row => row.Status, row => row.Name);

            Assert.Equal(new[] { "A" }, visible.Select(row => row.Name).ToArray());
        }

        [Fact]
        public void PrepareVisibleJobs_NegativeFilterReturnsEveryJob()
        {
            var source = new[]
            {
                new Row("B", 3),
                new Row("A", 0),
            };

            var visible = JobCardRules.PrepareVisibleJobs(source, -1, row => row.Status, row => row.Name);

            Assert.Equal(new[] { "A", "B" }, visible.Select(row => row.Name).ToArray());
        }
    }
}
