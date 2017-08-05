namespace xofz.Recipes98.Framework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using xofz.Framework;
    using xofz.Framework.Materialization;

    public class RecipeManager : RecipeSaver, RecipeLoader
    {
        public RecipeManager(string location)
        {
            this.location = location;
        }

        bool RecipeSaver.Save(Recipe recipe)
        {
            if (recipe == null)
            {
                throw new InvalidOperationException(
                    "Recipe cannot be null.");
            }

            this.checkRecipeForReservedStrings(recipe);
            if (!Directory.Exists(this.location))
            {
                Directory.CreateDirectory(this.location);
            }

            var lines = new List<string>();
            this.addDescription(recipe, lines);
            this.addIngredients(recipe, lines);
            this.addDirections(recipe, lines);
            this.addNutritionalInfo(recipe, lines);
            var recipeLocation = Path.Combine(this.location, recipe.Name);
            var updated = File.Exists(recipeLocation);

            File.WriteAllLines(
                Path.Combine(this.location, recipe.Name),
                lines.ToArray());

            return updated;
        }

        private void checkRecipeForReservedStrings(Recipe recipe)
        {
            if (this.isReservedString(recipe.Description))
            {
                throw new InvalidOperationException(
                    "Description is a reserved string.");
            }

            foreach (var ingredient in EnumerableHelpers.SafeForEach(
                recipe.Ingredients))
            {
                if (this.isReservedString(ingredient))
                {
                    throw new InvalidOperationException(
                        "Ingredient + " + ingredient
                        + " is a reserved string.");
                }
            }

            foreach (var direction in EnumerableHelpers.SafeForEach(
                recipe.Directions))
            {
                if (this.isReservedString(direction))
                {
                    throw new InvalidOperationException(
                        "Direction + " + direction
                        + " is a reserved string.");
                }
            }

            if (recipe.NutritionalInfo != default(NutritionalInfo))
            {
                var ni = recipe.NutritionalInfo;
                if (this.isReservedString(ni.ServingSize))
                {
                    throw new InvalidOperationException(
                        "Serving size is a reserved string.");
                }

                if (this.isReservedString(ni.TotalServings))
                {
                    throw new InvalidOperationException(
                        "Total servings is a reserved string.");
                }

                this.checkNutritionItem(ni.Fat);
                this.checkNutritionItem(ni.Cholesterol);
                this.checkNutritionItem(ni.Sodium);
                this.checkNutritionItem(ni.Potassium);
                this.checkNutritionItem(ni.Carbohydrates);
                this.checkNutritionItem(ni.Protein);
                this.checkNutritionItem(ni.VitaminsAndMinerals);
            }
        }

        private void checkNutritionItem(NutritionItem item)
        {
            if (item == default(NutritionItem))
            {
                return;
            }

            this.checkItemExceptForSubItems(item);

            foreach (var subItem in EnumerableHelpers.SafeForEach(
                item.SubItems))
            {
                this.checkItemExceptForSubItems(subItem);
            }
        }

        private void checkItemExceptForSubItems(NutritionItem item)
        {
            if (this.isReservedString(item.Label))
            {
                throw new InvalidOperationException(
                    "Nutrition item label " + item.Label
                    + " is a reserved string");
            }

            if (this.isReservedString(item.Value))
            {
                throw new InvalidOperationException(
                    "Nutrition item " + item.Label
                    + " value " + item.Value
                    + " is a reserved string.");
            }

            if (this.isReservedString(item.PercentDailyValue))
            {
                throw new InvalidOperationException(
                    "Nutrition item " + item.Label
                    + " PDV " + item.PercentDailyValue
                    + " is a reserved string.");
            }
        }

        private void addDescription(Recipe recipe, List<string> lines)
        {
            lines.Add("[Description]");
            var rd = recipe.Description;
            if (rd != default(string))
            {
                lines.Add(rd);
            }
            lines.Add(string.Empty);
        }

        private void addIngredients(Recipe recipe, List<string> lines)
        {
            lines.Add("[Ingredients]");
            var ri = recipe.Ingredients;
            if (ri != default(MaterializedEnumerable<string>))
            {
                lines.AddRange(ri);
            }
            lines.Add(string.Empty);
        }

        private void addDirections(Recipe recipe, List<string> lines)
        {
            lines.Add("[Directions]");
            var rDi = recipe.Directions;
            if (rDi != default(MaterializedEnumerable<string>))
            {
                lines.AddRange(rDi);
            }
            lines.Add(string.Empty);
        }

        private void addNutritionalInfo(Recipe recipe, List<string> lines)
        {
            lines.Add("[Nutritional Info]");
            var ni = recipe.NutritionalInfo;
            if (ni != default(NutritionalInfo))
            {
                lines.Add(ni.ServingSize);
                lines.Add(ni.TotalServings);
                lines.Add(ni.Calories);
                lines.Add(ni.CaloriesFromFat);

                lines.Add("----Fat");
                this.addNutritionItem(ni.Fat, lines);
                lines.Add("----Cholesterol");
                this.addNutritionItem(ni.Cholesterol, lines);
                lines.Add("----Sodium");
                this.addNutritionItem(ni.Sodium, lines);
                lines.Add("----Potassium");
                this.addNutritionItem(ni.Potassium, lines);
                lines.Add("----Carbohydrates");
                this.addNutritionItem(ni.Carbohydrates, lines);
                lines.Add("----Protein");
                this.addNutritionItem(ni.Protein, lines);
                lines.Add("----Vitamins and Minerals");
                this.addNutritionItem(ni.VitaminsAndMinerals, lines);
            }
        }

        private void addNutritionItem(NutritionItem item, List<string> lines)
        {
            if (item != default(NutritionItem))
            {
                lines.Add(item.Label);
                lines.Add(item.Value);
                lines.Add(item.PercentDailyValue);
                var subItems = item.SubItems;
                if (subItems != default(MaterializedEnumerable<NutritionItem>))
                {
                    foreach (var si in subItems)
                    {
                        lines.Add(si.Label);
                        lines.Add(si.Value);
                        lines.Add(si.PercentDailyValue);
                    }
                }
            }
            lines.Add(string.Empty);
        }

        void RecipeSaver.Delete(string recipeName)
        {
            var filePath = Path.Combine(this.location, recipeName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        IEnumerable<Recipe> RecipeLoader.All()
        {
            if (!Directory.Exists(this.location))
            {
                Directory.CreateDirectory(this.location);
            }

            foreach (var filePath in Directory.GetFiles(this.location))
            {
                var name = Path.GetFileName(filePath);
                var lines = File.ReadAllLines(filePath);
                if (lines.Length <= 5)
                {
                    yield return new Recipe(name);
                }

                var description = lines[1];
                var indexOfIngredients = lines.Length;
                for (var i = 2; i < lines.Length; ++i)
                {
                    if (lines[i] == "[Ingredients]")
                    {
                        indexOfIngredients = i + 1;
                        break;
                    }
                }

                var indexOfDirections = lines.Length;
                var ingredients = new LinkedList<string>();
                for (var i = indexOfIngredients; i < lines.Length - 1; ++i)
                {
                    if (lines[i] == string.Empty && lines[i + 1] == "[Directions]")
                    {
                        indexOfDirections = i + 2;
                        break;
                    }

                    ingredients.AddLast(lines[i]);
                }

                var indexOfNutlInfo = lines.Length;
                var directions = new LinkedList<string>();
                for (var i = indexOfDirections; i < lines.Length - 1; ++i)
                {
                    if (lines[i] == string.Empty && lines[i + 1] == "[Nutritional Info]")
                    {
                        indexOfNutlInfo = i + 2;
                        break;
                    }

                    directions.AddLast(lines[i]);
                }


                var nutlInfo = new NutritionalInfo();
                var start = indexOfNutlInfo;
                if (start < lines.Length)
                {
                    nutlInfo.ServingSize = lines[start];
                }

                ++start;
                if (start < lines.Length)
                {
                    nutlInfo.TotalServings = lines[start];
                }

                ++start;
                if (start < lines.Length)
                {
                    nutlInfo.Calories = lines[start];
                }

                ++start;
                if (start < lines.Length)
                {
                    nutlInfo.CaloriesFromFat = lines[start];
                }

                var indexOfFat = lines.Length;
                for (var i = start + 1; i < lines.Length; ++i)
                {
                    if (lines[i] == "----Fat")
                    {
                        indexOfFat = i + 1;
                        break;
                    }
                }

                int niStart;
                var fat = new NutritionItem();
                if (indexOfFat < lines.Length - 3)
                {
                    fat.Label = lines[indexOfFat];
                    fat.Value = lines[indexOfFat + 1];
                    fat.PercentDailyValue = lines[indexOfFat + 2];
                    niStart = indexOfFat + 3;
                    var subItems = new LinkedList<NutritionItem>();
                    while (niStart < lines.Length - 3)
                    {
                        var si = new NutritionItem(fat);
                        si.Label = lines[niStart];
                        si.Value = lines[niStart + 1];
                        si.PercentDailyValue = lines[niStart + 2];
                        subItems.AddLast(si);
                        niStart += 3;
                    }

                    fat.SubItems = new LinkedListMaterializedEnumerable<
                        NutritionItem>(subItems);
                }
                nutlInfo.Fat = fat;

                var indexOfCholesterol = lines.Length;
                for (var i = indexOfFat + 3; i < lines.Length - 1; ++i)
                {
                    if (lines[i] == string.Empty
                        && lines[i + 1] == "----Cholesterol")
                    {
                        indexOfCholesterol = i + 2;
                        break;
                    }
                }

                var chol = new NutritionItem();
                if (indexOfCholesterol < lines.Length - 3)
                {
                    chol.Label = lines[indexOfCholesterol];
                    chol.Value = lines[indexOfCholesterol + 1];
                    chol.PercentDailyValue = lines[indexOfCholesterol + 2];
                    niStart = indexOfCholesterol + 3;
                    var subItems = new LinkedList<NutritionItem>();
                    while (niStart < lines.Length - 3)
                    {
                        var si = new NutritionItem(chol);
                        si.Label = lines[niStart];
                        si.Value = lines[niStart + 1];
                        si.PercentDailyValue = lines[niStart + 2];
                        subItems.AddLast(si);
                        niStart += 3;
                    }

                    chol.SubItems = new LinkedListMaterializedEnumerable<
                        NutritionItem>(subItems);
                }
                nutlInfo.Cholesterol = chol;

                var indexOfSodium = lines.Length;
                for (var i = indexOfCholesterol + 3; i < lines.Length - 1; ++i)
                {
                    if (lines[i] == string.Empty
                        && lines[i + 1] == "----Sodium")
                    {
                        indexOfSodium = i + 2;
                        break;
                    }
                }

                var sodium = new NutritionItem();
                if (indexOfSodium < lines.Length - 3)
                {
                    sodium.Label = lines[indexOfSodium];
                    sodium.Value = lines[indexOfSodium + 1];
                    sodium.PercentDailyValue = lines[indexOfSodium + 2];
                    niStart = indexOfSodium + 3;
                    var subItems = new LinkedList<NutritionItem>();
                    while (niStart < lines.Length - 3)
                    {
                        var si = new NutritionItem(sodium);
                        si.Label = lines[niStart];
                        si.Value = lines[niStart + 1];
                        si.PercentDailyValue = lines[niStart + 2];
                        subItems.AddLast(si);
                        niStart += 3;
                    }

                    sodium.SubItems = new LinkedListMaterializedEnumerable<
                        NutritionItem>(subItems);
                }
                nutlInfo.Sodium = sodium;

                var indexOfPotassium = lines.Length;
                for (var i = indexOfSodium + 3; i < lines.Length - 1; ++i)
                {
                    if (lines[i] == string.Empty
                        && lines[i + 1] == "----Potassium")
                    {
                        indexOfPotassium = i + 2;
                        break;
                    }
                }

                var k = new NutritionItem();
                if (indexOfPotassium < lines.Length - 3)
                {
                    k.Label = lines[indexOfPotassium];
                    k.Value = lines[indexOfPotassium + 1];
                    k.PercentDailyValue = lines[indexOfPotassium + 2];
                    niStart = indexOfPotassium + 3;
                    var subItems = new LinkedList<NutritionItem>();
                    while (niStart < lines.Length - 3)
                    {
                        var si = new NutritionItem(k);
                        si.Label = lines[niStart];
                        si.Value = lines[niStart + 1];
                        si.PercentDailyValue = lines[niStart + 2];
                        subItems.AddLast(si);
                        niStart += 3;
                    }

                    k.SubItems = new LinkedListMaterializedEnumerable<
                        NutritionItem>(subItems);
                }
                nutlInfo.Potassium = k;

                var indexOfCarbs = lines.Length;
                for (var i = indexOfPotassium + 3; i < lines.Length - 1; ++i)
                {
                    if (lines[i] == string.Empty
                        && lines[i + 1] == "----Carbohydrates")
                    {
                        indexOfCarbs = i + 2;
                        break;
                    }
                }

                var carbs = new NutritionItem();
                if (indexOfCarbs < lines.Length - 3)
                {
                    carbs.Label = lines[indexOfCarbs];
                    carbs.Value = lines[indexOfCarbs + 1];
                    carbs.PercentDailyValue = lines[indexOfCarbs + 2];
                    niStart = indexOfCarbs + 3;
                    var subItems = new LinkedList<NutritionItem>();
                    while (niStart < lines.Length - 3)
                    {
                        var si = new NutritionItem(carbs);
                        si.Label = lines[niStart];
                        si.Value = lines[niStart + 1];
                        si.PercentDailyValue = lines[niStart + 2];
                        subItems.AddLast(si);
                        niStart += 3;
                    }

                    carbs.SubItems = new LinkedListMaterializedEnumerable<
                        NutritionItem>(subItems);
                }
                nutlInfo.Carbohydrates = carbs;

                var indexOfProtein = lines.Length;
                for (var i = indexOfCarbs + 3; i < lines.Length - 1; ++i)
                {
                    if (lines[i] == string.Empty
                        && lines[i + 1] == "----Protein")
                    {
                        indexOfProtein = i + 2;
                        break;
                    }
                }

                var protein = new NutritionItem();
                if (indexOfProtein < lines.Length - 3)
                {
                    protein.Label = lines[indexOfProtein];
                    protein.Value = lines[indexOfProtein + 1];
                    protein.PercentDailyValue = lines[indexOfProtein + 2];
                    niStart = indexOfProtein + 3;
                    var subItems = new LinkedList<NutritionItem>();
                    while (niStart < lines.Length - 3)
                    {
                        var si = new NutritionItem(protein);
                        si.Label = lines[niStart];
                        si.Value = lines[niStart + 1];
                        si.PercentDailyValue = lines[niStart + 2];
                        subItems.AddLast(si);
                        niStart += 3;
                    }

                    protein.SubItems = new LinkedListMaterializedEnumerable<
                        NutritionItem>(subItems);
                }
                nutlInfo.Protein = protein;

                var indexOfVitamins = lines.Length;
                for (var i = indexOfProtein + 3; i < lines.Length - 1; ++i)
                {
                    if (lines[i] == string.Empty
                        && lines[i + 1] == "----Vitamins and Minerals")
                    {
                        indexOfVitamins = i + 2;
                        break;
                    }
                }

                var vitamins = new NutritionItem();
                if (indexOfVitamins < lines.Length - 3)
                {
                    vitamins.Label = lines[indexOfVitamins];
                    vitamins.Value = lines[indexOfVitamins + 1];
                    vitamins.PercentDailyValue = lines[indexOfVitamins + 2];
                    niStart = indexOfVitamins + 3;
                    var subItems = new LinkedList<NutritionItem>();
                    while (niStart < lines.Length - 3)
                    {
                        var si = new NutritionItem(vitamins);
                        si.Label = lines[niStart];
                        si.Value = lines[niStart + 1];
                        si.PercentDailyValue = lines[niStart + 2];
                        subItems.AddLast(si);
                        niStart += 3;
                    }

                    vitamins.SubItems = new LinkedListMaterializedEnumerable<
                        NutritionItem>(subItems);
                }
                nutlInfo.VitaminsAndMinerals = vitamins;

                yield return new Recipe(name)
                {
                    Description = description,
                    Ingredients = new LinkedListMaterializedEnumerable<string>(
                        ingredients),
                    Directions = new LinkedListMaterializedEnumerable<string>(
                        directions),
                    NutritionalInfo = nutlInfo
                };
            }
        }

        private bool isReservedString(string partOfRecipe)
        {
            switch (partOfRecipe)
            {
                case "[Description]":
                    return true;
                case "[Ingredients]":
                    return true;
                case "[Directions]":
                    return true;
                case "[Nutritional Info]":
                    return true;
                case "----Fat":
                    return true;
                case "----Cholesterol":
                    return true;
                case "----Sodium":
                    return true;
                case "----Potassium":
                    return true;
                case "----Carbohydrates":
                    return true;
                case "----Protein":
                    return true;
                case "----Vitamins and Minerals":
                    return true;
                default:
                    return false;
            }
        }

        private readonly string location;
    }
}
