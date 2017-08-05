namespace xofz.Recipes98.Presentation
{
    using System;
    using System.Threading;
    using xofz.Framework;
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
            this.ui.ResetKeyTapped += this.ui_ResetKeyTapped;
            UiHelpers.Write(
                this.ui,
                () =>
                {
                    this.ui.EditKeyEnabled = false;
                    this.ui.SaveKeyEnabled = false;
                    this.ui.Editable = false;
                    this.ui.Info = null;
                });
            this.ui.WriteFinished.WaitOne();

            var w = this.web;
            w.Run<Navigator>(
                n => n.RegisterPresenter(this));
        }

        private void ui_LookupKeyTapped()
        {
            UiHelpers.Write(
                this.ui,
                () =>
                {
                    this.ui.MatchRecipeName = null;
                    this.ui.Info = null;
                    this.ui.Editable = false;
                    this.ui.EditKeyEnabled = false;
                    this.ui.SaveKeyEnabled = false;
                });

            var recipeName = UiHelpers.Read(
                this.ui,
                () => this.ui.LookupRecipeName);
            var recipe = this.getRecipe(recipeName);

            if (recipe != null)
            {
                UiHelpers.Write(
                    this.ui,
                    () =>
                    {
                        this.ui.MatchRecipeName = recipe.Name;
                        this.ui.Info = recipe.NutritionalInfo;
                        this.ui.EditKeyEnabled = true;
                    });
            }
        }

        private Recipe getRecipe(string name)
        {
            var w = this.web;
            return w.Run<RecipeLoader, Recipe>(loader =>
            {
                var recipes = loader.All();
                Recipe match = null;
                foreach (var r in recipes)
                {
                    if (string.Equals(
                        r.Name,
                        name,
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        match = r;
                        break;
                    }
                }

                return match;
            });
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
                return;
            }

            w.Run<Messenger>(m =>
                UiHelpers.Write(m.Subscriber, () =>
                    m.Inform("Recipe updated!")));

            w.Run<LogEditor>(le => le.AddEntry(
                "Information",
                new[]
                {
                    "A recipe's nutritional info was updated: "
                    + recipe.Name
                }));

            var rui = w.Run<Navigator, RecipesUi>(
                n => n.GetUi<RecipesPresenter, RecipesUi>());
            w.Run<EventRaiser>(er =>
            {
                er.Raise(rui, "SearchTextChanged");
            });
        }

        private void ui_ResetKeyTapped()
        {
            var w = this.web;
            var response = Response.No;
            w.Run<Messenger>(m =>
            {
                UiHelpers.Write(
                    m.Subscriber,
                    () => response = m.Question("Really clear all fields?"));
                m.Subscriber.WriteFinished.WaitOne();
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
                });
            }
        }

        private int setupIf1;
        private readonly NutritionalInfoUi ui;
        private readonly MethodWeb web;
    }
}
