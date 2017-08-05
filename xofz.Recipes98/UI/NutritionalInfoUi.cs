namespace xofz.Recipes98.UI
{
    using xofz.UI;

    public interface NutritionalInfoUi : Ui
    {
        event Action LookupKeyTapped;

        event Action EditKeyTapped;

        event Action SaveKeyTapped;

        event Action ResetKeyTapped;
        
        NutritionalInfo Info { get; set; }

        string LookupRecipeName { get; set; }

        string MatchRecipeName { get; set; }

        bool EditKeyEnabled { get; set; }

        bool SaveKeyEnabled { get; set; }

        bool Editable { get; set; }
    }
}
