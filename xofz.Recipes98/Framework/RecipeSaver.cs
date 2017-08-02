namespace xofz.Recipes2k.Framework
{
    public interface RecipeSaver
    {
        bool Save(Recipe recipe); // returns true if the recipe was updated (overwritten)

        void Delete(string recipeName);
    }
}
