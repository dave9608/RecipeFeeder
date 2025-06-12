using System.Diagnostics;

namespace RecipeFeeder
{
    /// <summary>
    /// Main program class for the Recipe Feeder application.
    /// Calculates the optimal combination of recipes to feed the maximum number of people
    /// given available ingredient inventory.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point for the Recipe Feeder application.
        /// Orchestrates the recipe optimization process and displays results.
        /// </summary>
        /// <param name="args">Command line arguments (not used).</param>
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing Recipe Feeder...");

            // 1. Load our data with the models
            var recipes = GetRecipes();
            var initialStock = Stock.FromDictionary(GetInitialStockDictionary());

            // 2. Create recipe solver, start timer and run calculation
            var solver = new RecipeSolver(recipes);
            var stopwatch = Stopwatch.StartNew();
            var result = solver.Solve(initialStock);
            stopwatch.Stop();

            // 3. Display the results in a clean, verifiable format
            PrintReport(result, initialStock, stopwatch.Elapsed.TotalMilliseconds);

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }

        #region Reporting and Data Setup

        /// <summary>
        /// Prints a comprehensive report of the optimization results including
        /// recipe combinations, feeding capacity, and inventory verification.
        /// </summary>
        /// <param name="result">The optimization result containing feed count and recipe combination.</param>
        /// <param name="initialStock">The starting inventory before recipe preparation.</param>
        /// <param name="elapsedMs">Time taken for the calculation in milliseconds.</param>
        private static void PrintReport((int Feeds, List<Recipe> Combination) result, Stock initialStock, double elapsedMs)
        {
            Console.WriteLine("\n--- Optimal Recipe Plan ---");
            Console.WriteLine($"With our inventory we can feed a maximum of {result.Feeds} people.");

            // Group the recipes for better readability
            var recipeGroups = result.Combination
                .GroupBy(r => r.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() });

            foreach (var group in recipeGroups)
            {
                Console.WriteLine($"  - {group.Name}: {group.Count}");
            }
            Console.WriteLine($"\nCalculation completed in {elapsedMs:F2} ms.");

            // To confirm the solution, we will verify the inventory usage
            Console.WriteLine("\n--- Inventory Verification ---");

            // Calculate total ingredient usage across all selected recipes
            Stock totalUsed = new(0, 0, 0, 0, 0, 0, 0);
            foreach (var recipe in result.Combination)
            {
                // We don't have an addition operator, so we do it manually
                totalUsed = new Stock(
                    totalUsed.Dough + recipe.Needs.Dough,
                    totalUsed.Meat + recipe.Needs.Meat,
                    totalUsed.Lettuce + recipe.Needs.Lettuce,
                    totalUsed.Tomato + recipe.Needs.Tomato,
                    totalUsed.Cheese + recipe.Needs.Cheese,
                    totalUsed.Cucumber + recipe.Needs.Cucumber,
                    totalUsed.Olives + recipe.Needs.Olives
                );
            }

            // Calculate remaining inventory after recipe preparation
            Stock finalStock = initialStock - totalUsed;

            // Display inventory table with start, used, and remaining quantities
            Console.WriteLine($"{"Ingredient",-12} | {"Start",-7} | {"Used",-7} | {"End",-7}");
            Console.WriteLine(new string('-', 45));
            Console.WriteLine($"{"Dough",-12} | {initialStock.Dough,-7} | {totalUsed.Dough,-7} | {finalStock.Dough,-7}");
            Console.WriteLine($"{"Meat",-12} | {initialStock.Meat,-7} | {totalUsed.Meat,-7} | {finalStock.Meat,-7}");
            Console.WriteLine($"{"Lettuce",-12} | {initialStock.Lettuce,-7} | {totalUsed.Lettuce,-7} | {finalStock.Lettuce,-7}");
            Console.WriteLine($"{"Tomato",-12} | {initialStock.Tomato,-7} | {totalUsed.Tomato,-7} | {finalStock.Tomato,-7}");
            Console.WriteLine($"{"Cheese",-12} | {initialStock.Cheese,-7} | {totalUsed.Cheese,-7} | {finalStock.Cheese,-7}");
            Console.WriteLine($"{"Cucumber",-12} | {initialStock.Cucumber,-7} | {totalUsed.Cucumber,-7} | {finalStock.Cucumber,-7}");
            Console.WriteLine($"{"Olives",-12} | {initialStock.Olives,-7} | {totalUsed.Olives,-7} | {finalStock.Olives,-7}");
            Console.WriteLine(new string('-', 45));
        }

        /// <summary>
        /// Defines the available recipes with their serving sizes and ingredient requirements.
        /// Each recipe specifies how many people it feeds and what ingredients are needed.
        /// </summary>
        /// <returns>A list of all available recipes for optimization.</returns>
        private static List<Recipe> GetRecipes() => new()
        {
            new("Burger", 1, Stock.FromDictionary(new() { [Ingredient.Meat]=1, [Ingredient.Lettuce]=1, [Ingredient.Tomato]=1, [Ingredient.Cheese]=1, [Ingredient.Dough]=1 })),
            new("Pie", 1, Stock.FromDictionary(new() { [Ingredient.Dough]=2, [Ingredient.Meat]=2 })),
            new("Sandwich", 1, Stock.FromDictionary(new() { [Ingredient.Dough]=1, [Ingredient.Cucumber]=1 })),
            new("Pasta", 2, Stock.FromDictionary(new() { [Ingredient.Dough]=2, [Ingredient.Tomato]=1, [Ingredient.Cheese]=2, [Ingredient.Meat]=1 })),
            new("Salad", 3, Stock.FromDictionary(new() { [Ingredient.Lettuce]=2, [Ingredient.Tomato]=2, [Ingredient.Cucumber]=1, [Ingredient.Cheese]=2, [Ingredient.Olives]=1 })),
            new("Pizza", 4, Stock.FromDictionary(new() { [Ingredient.Dough]=3, [Ingredient.Tomato]=2, [Ingredient.Cheese]=3, [Ingredient.Olives]=1 }))
        };

        /// <summary>
        /// Defines the initial ingredient inventory available for recipe preparation.
        /// This represents the starting quantities of each ingredient type.
        /// </summary>
        /// <returns>A dictionary mapping each ingredient type to its initial quantity.</returns>
        private static Dictionary<Ingredient, int> GetInitialStockDictionary() => new()
        {
            [Ingredient.Dough] = 10,
            [Ingredient.Meat] = 6,
            [Ingredient.Lettuce] = 3,
            [Ingredient.Tomato] = 6,
            [Ingredient.Cheese] = 8,
            [Ingredient.Cucumber] = 2,
            [Ingredient.Olives] = 2
        };

        #endregion
    }
}