namespace xofz.Recipes2k.Framework
{
    using System.Collections.Generic;

    public interface RecipeLoader
    {
        IEnumerable<Recipe> All();
    }
}
