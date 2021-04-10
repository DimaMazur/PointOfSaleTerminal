using AutoFixture;
using AutoFixture.Xunit2;
using DimaMazur.Sales.Core.Services;
using DimaMazur.Sales.Model;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DimaMazur.Sales.Core.Tests.Services
{
    public class PointOfSaleTerminalTests
    {
        [Theory, AutoData]
        public void SetPricing_NegativePricesAndNoPriceForSingleItem_ReturnsFailedOperation(Generator<PricingRule> generator)
        { 
            var rules = generator.Take(4).ToArray();

            rules[1].Price = 0;

            var sut = new PointOfSaleTerminal();
            var result = sut.SetPricing(rules);

            using (new AssertionScope())
            {
                result.Success.Should().BeFalse();
                result.Errors.Count.Should().Be(2);
                result.Errors
                    .Any(e => e.Message == "Some of products doesn't have price for single item.")
                    .Should()
                    .BeTrue();

                result.Errors
                    .Any(e => e.Message == "Some of products has not positive price.")
                    .Should()
                    .BeTrue();
            }
        }

        [Theory, AutoData]
        public void SetPricing_PricingItemsAreFine_ReturnsSucceededOperation(Generator<PricingRule> generator)
        {
            var rules = generator.Take(5).ToList();
            rules.ForEach(r => r.Count = 1);

            var sut = new PointOfSaleTerminal();
            var result = sut.SetPricing(rules);

            using (new AssertionScope())
            {
                result.Success.Should().BeTrue();
            }
        }

        [Theory, AutoData]
        public void Scan_ScanProductWithEmptyId_ReturnsFailedOperation(Generator<PricingRule> generator)
        {
            var rules = generator.Take(5).ToList();
            rules.ForEach(r => r.Count = 1);

            var sut = new PointOfSaleTerminal();
            sut.SetPricing(rules);
            var result = sut.Scan(null);

            using (new AssertionScope())
            {
                result.Success.Should().BeFalse();
                result.Errors.Count.Should().Be(1);
                result.Errors.First().Message
                    .Should()
                    .Be("Product with null or empty id could not be scanned.");
            }
        }

        [Theory, AutoData]
        public void Scan_ScanProductThatIsNotRegistered_ReturnsFailedOperation(string productId)
        {
            var sut = new PointOfSaleTerminal();
            var result = sut.Scan(productId);

            using (new AssertionScope())
            {
                result.Success.Should().BeFalse();
                result.Errors.Count.Should().Be(1);
                result.Errors.First().Message
                    .Should()
                    .Be($"Product with id {productId} could not be scanned. No info how much does it cost.");
            }
        }

        [Theory, AutoData]
        public void Scan_ScannedItemIsRegistered_ReturnsSucceededOperation(Generator<PricingRule> generator)
        {
            var rules = generator.Take(5).ToList();
            rules.ForEach(r => r.Count = 1);

            var sut = new PointOfSaleTerminal();
            sut.SetPricing(rules);
            var result = sut.Scan(rules[3].Id);

            using (new AssertionScope())
            {
                result.Success.Should().BeTrue();
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void CalculateTotal_DifferentCorrectItemsScanned_CalculatesCorrectly(string productIds, decimal expected)
        {
            var sut = new PointOfSaleTerminal();
            sut.SetPricing(Rules);

            foreach(var s in productIds)
            {
                sut.Scan(s.ToString());
            }

            var actual = sut.CalculateTotal();

            using (new AssertionScope())
            {
                actual.Should().Be(expected);
            }
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { "ABCDABA", 13.25m },
                new object[] { "CCCCCCC", 6.00m },
                new object[] { "ABCD", 7.25m },
                new object[] { "DABAABC", 13.25m },
                new object[] { "ABCDABCDABCD", 21.00m },
                new object[] { "BBBBCDBABBDCA", 35.75m },
                new object[] { "CACACACACACBADAABCA", 24.25m }
            };

        public static IEnumerable<PricingRule> Rules =>
            new PricingRule[]
            {
                new PricingRule { Id = "A", Count = 1, Price = 1.25m },
                new PricingRule { Id = "B", Count = 1, Price = 4.25m },
                new PricingRule { Id = "C", Count = 1, Price = 1.00m },
                new PricingRule { Id = "D", Count = 1, Price = 0.75m },
                new PricingRule { Id = "A", Count = 3, Price = 3m },
                new PricingRule { Id = "C", Count = 6, Price = 5m }
            };
    }
}
