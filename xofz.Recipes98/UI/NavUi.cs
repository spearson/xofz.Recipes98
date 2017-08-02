namespace xofz.Recipes2k.UI
{
    using xofz.UI;

    public interface NavUi : Ui
    {
        event Action RecipesKeyTapped;

        event Action AddKeyTapped;

        event Action LogKeyTapped;

        event Action ExitKeyTapped;
    }
}
