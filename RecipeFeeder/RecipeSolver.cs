namespace RecipeFeeder
{
    // Solver class that finds optimal recipe combinations using dynamic programming
    public class RecipeSolver
    {
        private readonly List<Recipe> _recipes;
        // The memoization cache. It stores results for subproblems we've already solved.
        private readonly Dictionary<Stock, (int Feeds, List<Recipe> Combination)> _memoCache;

        public RecipeSolver(IEnumerable<Recipe> recipes)
        {
            _recipes = recipes.ToList();
            _memoCache = new Dictionary<Stock, (int Feeds, List<Recipe> Combination)>();
        }

        /// <summary>
        /// Finds the optimal recipe combination for given ingredients
        /// </summary>
        public (int Feeds, List<Recipe> Combination) Solve(Stock initialStock)
        {
            return SolveRecursive(initialStock);
        }

        /// <summary>
        /// Recursive function implementing memoized dynamic programming
        /// </summary>
        private (int Feeds, List<Recipe> Combination) SolveRecursive(Stock currentStock)
        {
            // Return cached result if already calculated
            if (_memoCache.TryGetValue(currentStock, out var cachedResult))
            {
                return cachedResult;
            }

            // Base case: no recipes made
            var bestOutcome = (Feeds: 0, Combination: new List<Recipe>());

            // Try each available recipe
            foreach (var recipe in _recipes)
            {
                // Check if we have enough ingredients for this recipe
                if (currentStock.CanAfford(recipe.Needs))
                {
                    // Calculate remaining ingredients after making this recipe
                    Stock remainingStock = currentStock - recipe.Needs;
                    var subProblemOutcome = SolveRecursive(remainingStock);

                    // Calculate total people fed with this recipe choice
                    int potentialFeeds = subProblemOutcome.Feeds + recipe.Feeds;

                    // Update best outcome if this path feeds more people
                    if (potentialFeeds > bestOutcome.Feeds)
                    {
                        var newCombination = new List<Recipe>(subProblemOutcome.Combination) { recipe };
                        bestOutcome = (Feeds: potentialFeeds, Combination: newCombination);
                    }
                }
            }

            // Cache result and return
            _memoCache[currentStock] = bestOutcome;
            return bestOutcome;
        }
    }
}