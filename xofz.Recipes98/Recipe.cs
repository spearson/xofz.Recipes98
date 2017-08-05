namespace xofz.Recipes98
{
    public class Recipe
    {
        public Recipe(string name = null)
        {
            this.Name = name;
        }

        public virtual string Name { get; }

        public virtual string Description { get; set; }

        public virtual MaterializedEnumerable<string> Ingredients { get; set; }

        public virtual MaterializedEnumerable<string> Directions { get; set; }

        public virtual NutritionalInfo NutritionalInfo { get; set; }

        public override bool Equals(object other)
        {
            if (!(other is Recipe))
            {
                return false;
            }

            var otherRecipe = (Recipe)other;
            return this.Name == otherRecipe.Name;
        }

        public override int GetHashCode()
        {
            if (this.Name == default(string))
            {
                return 0;
            }

            return this.Name.GetHashCode();
        }
    }
}
