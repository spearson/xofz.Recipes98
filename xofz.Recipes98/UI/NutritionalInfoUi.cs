namespace xofz.Recipes98.UI
{
    using xofz.UI;

    public interface NutritionalInfoUi : Ui
    {
        event Do LookupKeyTapped;

        event Do EditKeyTapped;

        event Do SaveKeyTapped;

        event Do CancelKeyTapped;

        // would the user ever want auto-save disabled?
        // event Do<bool> AutoSaveToggled;
        
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
