using DimaMazur.Core.DomainPrimitives;
using DimaMazur.Sales.Model;
using System.Collections.Generic;
using System.Linq;

namespace DimaMazur.Sales.Core.Services
{
    public class PointOfSaleTerminal
    {
        private Dictionary<string, IEnumerable<PricingRule>> rules = new();
        private readonly List<string> productsInBacket = new();

        /// <summary>
        /// Override all pricings with rules.
        /// </summary>
        /// <param name="rules">Rules which will be used to calculate receipt.</param>
        /// <returns>Result if operation was successfully done.</returns>
        public OperationResult SetPricing(IEnumerable<PricingRule> rules)
        { 
            this.rules = rules
                .GroupBy(r => r.Id)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(i => i.Count).AsEnumerable());

            List<ExecutionError> errors = new();
            var allProductsHaveCountForSingleItem = this.rules.All(c => c.Value.Any(r => r.Count == 1));
            var allRulesAreWithPositivePrices= rules.All(r => r.Price > 0);

            if (allProductsHaveCountForSingleItem && allRulesAreWithPositivePrices)
            {
                return OperationResult.Succeeded;
            }
            
            if (!allProductsHaveCountForSingleItem)
            {
                errors.Add(new ExecutionError("Some of products doesn't have price for single item."));
            }

            if (!allRulesAreWithPositivePrices)
            {
                errors.Add(new ExecutionError("Some of products has not positive price."));
            }

            return OperationResult.Failed(errors);
        }

        /// <summary>
        /// Soft scanning behavior in which we don't scan and track items which rules/prices don't exist.
        /// </summary>
        /// <param name="productId">Id of the product to be scanned.</param>
        /// <returns>Is product scanned succesfully?</returns>
        public virtual OperationResult Scan(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return OperationResult.Failed("Product with null or empty id could not be scanned.");
            }

            if (!rules.ContainsKey(productId))
            {
                return OperationResult.Failed($"Product with id {productId} could not be scanned. No info how much does it cost.");
            }

            productsInBacket.Add(productId);
            return OperationResult.Succeeded;
        }

        /// <summary>
        /// Calculates final amount to be paid.
        /// </summary>
        /// <returns>Amount of money to be paid.</returns>
        public decimal CalculateTotal()
        {
            var result = 0m;

            var groupedProducts = productsInBacket.GroupBy(p => p);

            foreach (var group in groupedProducts)
            {
                var productId = group.Key;
                var countToBeCalculated = (uint)group.Count();

                while (countToBeCalculated > 0)
                {
                    foreach (var rule in rules[productId])
                    {
                        var numberOfBatches = countToBeCalculated / rule.Count;

                        if (numberOfBatches == 0)
                        {
                            continue;
                        }

                        result += numberOfBatches * rule.Price;
                        countToBeCalculated = countToBeCalculated % rule.Count;
                    }
                }
            }

            return result;
        }
    }
}
