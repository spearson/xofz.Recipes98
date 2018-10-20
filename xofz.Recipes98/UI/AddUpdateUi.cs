namespace xofz.Recipes98.UI
{
    using xofz.UI;

    public interface AddUpdateUi : Ui
    {
        event Do AddUpdateKeyTapped;

        event Do ResetKeyTapped;

        event Do LookupKeyTapped;

        Recipe RecipeToAddUpdate { get; set; }
    }
}
