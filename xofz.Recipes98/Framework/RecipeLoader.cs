namespace xofz.Recipes98.Framework
{
    using System.Collections.Generic;

    public interface RecipeLoader
    {
        IEnumerable<Recipe> All();
    }
}
