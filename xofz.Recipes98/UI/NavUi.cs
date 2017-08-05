namespace xofz.Recipes98.UI
{
    using xofz.UI;

    public interface NavUi : Ui
    {
        event Action RecipesKeyTapped;

        event Action AddKeyTapped;

        event Action NutlInfoKeyTapped;

        event Action LogKeyTapped;

        event Action ExitKeyTapped;
    }
}
