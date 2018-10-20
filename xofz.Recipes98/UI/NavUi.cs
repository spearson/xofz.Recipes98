namespace xofz.Recipes98.UI
{
    using xofz.UI;

    public interface NavUi : Ui
    {
        event Do RecipesKeyTapped;

        event Do AddKeyTapped;

        event Do NutlInfoKeyTapped;

        event Do LogKeyTapped;

        event Do ExitKeyTapped;
    }
}
