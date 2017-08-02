namespace xofz.Recipes2k.UI
{
    using xofz.UI;

    public interface AddUpdateUi : Ui
    {
        event Action AddUpdateKeyTapped;

        event Action ResetKeyTapped;

        event Action LookupKeyTapped;

        Recipe RecipeToAddUpdate { get; set; }
    }
}
