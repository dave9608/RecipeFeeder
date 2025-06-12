# Recipe Feeder Optimization Challenge

This project finds the optimal recipe combination to feed the maximum number of people with available ingredients.

## The Problem

This is the classic Unbounded Knapsack Problem from computer science:

* **Capacity:** Available ingredients (10 Dough, 6 Meat, etc.)
* **Items:** Recipes (Burger, Pizza, etc.) - can make unlimited quantities
* **Weight:** Ingredients required per recipe
* **Value:** Number of people each recipe feeds

Goal: Use ingredients optimally to feed the most people.

## Solution Approach

**Dynamic Programming with Memoization**

The algorithm breaks down the problem into smaller subproblems and caches results to avoid recalculation.

* `SolveRecursive` function tries all possible recipes with current ingredients
* `_memoCache` stores previously calculated results
* Each unique ingredient combination is solved only once

## Key Implementation Details

* **`record struct Stock`**: Uses value-based equality for efficient caching and avoids heap allocations during recursion by being a value type rather than a reference type
* **Operator Overloading (`-`)**: Enables clean, readable syntax like currentStock - recipe.Needs instead of unnecessary dictionary operations
* **Immutability**: All data structures are immutable, preventing accidental state changes which can cause incorrect results

## Note on Alternative Approaches

Initial research explored multiple approaches including brute-force and greedy algorithms. 
Brute-force was too slow, while greedy algorithms proved both inaccurate (no guarantee of optimal solutions) and slower than expected. 
The current dynamic programming solution with memoization provides optimal results efficiently by caching previously calculated results and avoiding redundant computations.