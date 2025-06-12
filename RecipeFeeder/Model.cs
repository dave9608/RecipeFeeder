namespace RecipeFeeder
{
    // Immutable data structure representing a recipe with its requirements
    public record Recipe(string Name, int Feeds, Stock Needs);

    // Enumeration of all available ingredient types
    public enum Ingredient { Dough, Meat, Lettuce, Tomato, Cheese, Cucumber, Olives }

    /// <summary>
    /// Represents ingredient inventory as a value type.
    /// Uses struct for performance and record for value equality - ideal for cache keys.
    /// </summary>
    public record struct Stock(int Dough, int Meat, int Lettuce, int Tomato, int Cheese, int Cucumber, int Olives)
    {
        // Creates Stock from dictionary input for easier initialization
        public static Stock FromDictionary(Dictionary<Ingredient, int> source) => new(
            source.GetValueOrDefault(Ingredient.Dough), 
            source.GetValueOrDefault(Ingredient.Meat),
            source.GetValueOrDefault(Ingredient.Lettuce), 
            source.GetValueOrDefault(Ingredient.Tomato),
            source.GetValueOrDefault(Ingredient.Cheese), 
            source.GetValueOrDefault(Ingredient.Cucumber),
            source.GetValueOrDefault(Ingredient.Olives)
        );

        // Enables natural syntax: currentStock - recipe.Needs
        public static Stock operator -(Stock a, Stock b) => new(
            a.Dough - b.Dough, 
            a.Meat - b.Meat, 
            a.Lettuce - b.Lettuce, 
            a.Tomato - b.Tomato,
            a.Cheese - b.Cheese, 
            a.Cucumber - b.Cucumber, 
            a.Olives - b.Olives
        );

        // Returns true if current stock has enough ingredients to cover the cost
        public bool CanAfford(Stock cost) =>
            Dough >= cost.Dough && 
            Meat >= cost.Meat && 
            Lettuce >= cost.Lettuce &&
            Tomato >= cost.Tomato && 
            Cheese >= cost.Cheese && 
            Cucumber >= cost.Cucumber &&
            Olives >= cost.Olives;
    }
}