namespace xofz.Recipes98.Presentation
{
    using System;
    using System.Threading;
    using xofz.Framework;
    using xofz.Framework.Logging;
    using xofz.Presentation;
    using xofz.Recipes98.Framework;
    using xofz.Recipes98.UI;
    using xofz.UI;

    public sealed class NutritionalInfoPresenter : Presenter
    {
        public NutritionalInfoPresenter(
            NutritionalInfoUi ui, 
            ShellUi shell,
            MethodWeb web) 
            : base(ui, shell)
        {
            this.ui = ui;
            this.web = web;
        }

        public void Setup()
        {
            if (Interlocked.CompareExchange(ref this.setupIf1, 1, 0) == 1)
            {
                return;
            }

            this.ui.LookupKeyTapped += this.ui_LookupKeyTapped;
            this.ui.EditKeyTapped += this.ui_EditKeyTapped;
            this.ui.SaveKeyTapped += this.ui_SaveKeyTapped;
            this.ui.CancelKeyTapped += this.ui_CancelKeyTapped;
            UiHelpers.WriteSync(
                this.ui,
                () =>
                {
                    this.ui.EditKeyEnabled = false;
                    this.ui.SaveKeyEnabled = false;
                    this.ui.CancelKeyEnabled = false;
                    this.ui.Editable = false;
                    this.ui.Info = null;
                });

            var w = this.web;
            w.Run<Navigator>(
                n => n.RegisterPresenter(this));
        }

        private void ui_LookupKeyTapped()
        {
            var w = this.web;
            if (UiHelpers.Read(
                this.ui,
                () => this.ui.Editable))
            {
                var response = Response.No;
                w.Run<Messenger>(m =>
                {
                    response = UiHelpers.Read(
                        m.Subscriber,
                        () => m.Question(
                            "Discard current changes?"));
                });

                if (response != Response.Yes)
                {
                    return;
                }
            }

            UiHelpers.Write(
                this.ui,
                () =>
                {
                    this.ui.MatchRecipeName = null;
                    this.ui.Info = null;
                    this.ui.Editable = false;
                    this.ui.EditKeyEnabled = false;
                    this.ui.SaveKeyEnabled = false;
                    this.ui.CancelKeyEnabled = false;
                });

            var recipeName = UiHelpers.Read(
                this.ui,
                () => this.ui.LookupRecipeName);
            var recipe = this.getRecipe(recipeName);
            if (recipe == null)
            {
                return;
            }

            var rn = recipe.Name;
            var rNi = recipe.NutritionalInfo;
            UiHelpers.Write(
                this.ui,
                () =>
                {
                    this.ui.MatchRecipeName = rn;
                    this.ui.Info = rNi;
                    this.ui.EditKeyEnabled = true;
                });
        }

        private Recipe getRecipe(string name)
        {
            var w = this.web;
            var recipe = default(Recipe);
            w.Run<RecipeLoader>(loader =>
            {
                recipe = EnumerableHelpers.FirstOrDefault(
                    loader.All(),
                    r => string.Equals(
                        r.Name,
                        name,
                        StringComparison.CurrentCultureIgnoreCase));
            });

            return recipe;
        }

        private void ui_EditKeyTapped()
        {
            UiHelpers.Write(
                this.ui,
                () =>
                {
                    this.ui.Editable = true;
                    this.ui.EditKeyEnabled = false;
                    this.ui.SaveKeyEnabled = true;
                    this.ui.CancelKeyEnabled = true;
                });
        }

        private void ui_SaveKeyTapped()
        {
            var w = this.web;
            var recipeName = UiHelpers.Read(
                this.ui,
                () => this.ui.MatchRecipeName);
            var recipe = this.getRecipe(recipeName);
            if (recipe == null)
            {
                w.Run<Messenger>(
                    m =>
                    {
                        UiHelpers.Write(
                            m.Subscriber,
                            () => m.GiveError(
                                "Recipe not found!"
                                + Environment.NewLine
                                + "Please create the recipe "
                                + "on the Add/Update screen"));
                    });
                return;
            }

            recipe.NutritionalInfo = UiHelpers.Read(
                this.ui,
                () => this.ui.Info);

            try
            {
                w.Run<RecipeSaver>(
                    saver => saver.Save(recipe));
            }
            catch (InvalidOperationException e)
            {
                w.Run<Messenger>(
                    m => UiHelpers.Write(
                        m.Subscriber,
                        () => m.GiveError(e.Message)));
                this.finishSaving();
                return;
            }

            w.Run<Navigator, EventRaiser>((n, er) =>
            {
                var rUi = n.GetUi<RecipesPresenter, RecipesUi>();
                er.Raise(
                    rUi,
                    nameof(rUi.SearchTextChanged));
            });

            w.Run<Messenger>(m =>
            {
                UiHelpers.Write(
                    m.Subscriber,
                    () =>
                    {
                        m.Inform("Recipe updated!");
                    });
            });

            w.Run<LogEditor>(le => le.AddEntry(
                "Information",
                new[]
                {
                    "A recipe's nutritional info was updated: "
                    + recipe.Name
                }));

            this.finishSaving();
        }

        private void finishSaving()
        {
            UiHelpers.Write(
                this.ui,
                () =>
                {
                    this.ui.CancelKeyEnabled = false;
                    this.ui.SaveKeyEnabled = false;
                    this.ui.Editable = false;
                    this.ui.EditKeyEnabled = false;
                    this.ui.Info = null;
                    this.ui.MatchRecipeName = null;
                });
        }

        private void ui_CancelKeyTapped()
        {
            var w = this.web;
            var response = Response.No;
            w.Run<Messenger>(m =>
            {
                response = UiHelpers.Read(
                    m.Subscriber,
                    () => m.Question("Discard current changes?"));
            });

            if (response == Response.Yes)
            {
                UiHelpers.Write(this.ui, () =>
                {
                    this.ui.MatchRecipeName = null;
                    this.ui.Info = null;
                    this.ui.SaveKeyEnabled = false;
                    this.ui.EditKeyEnabled = false;
                    this.ui.Editable = false;
                    this.ui.CancelKeyEnabled = false;
                });
            }
        }

        private int setupIf1;
        private readonly NutritionalInfoUi ui;
        private readonly MethodWeb web;
    }
}
