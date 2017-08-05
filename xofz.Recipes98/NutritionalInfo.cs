namespace xofz.Recipes98
{
    public class NutritionalInfo
    {
        public virtual string ServingSize { get; set; }

        public virtual string TotalServings { get; set; }


        // the rest of these items are per serving
        public virtual string Calories { get; set; }

        public virtual string CaloriesFromFat { get; set; }
        
        public virtual NutritionItem Fat { get; set; }

        public virtual NutritionItem Cholesterol { get; set; }

        public virtual NutritionItem Sodium { get; set; }

        public virtual NutritionItem Potassium { get; set; }

        public virtual NutritionItem Carbohydrates { get; set; }

        public virtual NutritionItem Protein { get; set; }

        public virtual NutritionItem VitaminsAndMinerals { get; set; }
    }
}
