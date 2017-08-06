namespace xofz.Recipes98.UI
{
    using xofz.UI;

    public interface NutritionalInfoUi : Ui
    {
        event Action LookupKeyTapped;

        event Action EditKeyTapped;

        event Action SaveKeyTapped;

        event Action CancelKeyTapped;

        // would the user ever want auto-save disabled?
        // event Action<bool> AutoSaveToggled;
        
        NutritionalInfo Info { get; set; }

        string LookupRecipeName { get; set; }

        string MatchRecipeName { get; set; }

        bool EditKeyEnabled { get; set; }

        bool SaveKeyEnabled { get; set; }

        bool CancelKeyEnabled { get; set; }

        // bool LookupKeyEnabled { get; set; }

        bool Editable { get; set; }
    }
}
