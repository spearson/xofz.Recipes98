﻿namespace xofz.Recipes98.Framework
{
    public interface RecipeSaver
    {
        bool Save(Recipe recipe); // returns true if the recipe was updated (overwritten)

        void Delete(string recipeName);
    }
}
