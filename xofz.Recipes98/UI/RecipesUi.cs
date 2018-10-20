namespace xofz.Recipes98.UI
{
    using System;
    using xofz.UI;

    public interface RecipesUi : Ui
    {
        event Do SearchTextChanged;

        event Do ClearSearchKeyTapped;

        event Do<string> OpenRequested;

        event Do<string> NutlInfoRequested;

        event Do<string> DeleteRequested;

        string NameSearchText { get; set; }

        string DescriptionSearchText { get; set; }

        Lot<string> IngredientsSearchText { get; set; }

        Lot<string> DirectionsSearchText { get; set; }

        Lot<Recipe> MatchingRecipes { get; set; }
    }
}
