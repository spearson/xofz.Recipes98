namespace xofz.Recipes2k.Presentation
{
    using System;
    using System.Threading;
    using xofz.Framework;
    using xofz.Presentation;
    using xofz.Recipes2k.Framework;
    using xofz.Recipes2k.UI;
    using xofz.UI;

    public sealed class AddUpdatePresenter : Presenter
    {
        public AddUpdatePresenter(
            AddUpdateUi ui, 
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

            this.ui.AddUpdateKeyTapped += this.ui_AddUpdateKeyTapped;
            this.ui.ResetKeyTapped += this.ui_ResetKeyTapped;
            this.ui.LookupKeyTapped += this.ui_LookupKeyTapped;

            this.web.Run<Navigator>(n => n.RegisterPresenter(this));
        }

        private void ui_AddUpdateKeyTapped()
        {
            var w = this.web;
            var recipe = UiHelpers.Read(this.ui, () => this.ui.RecipeToAddUpdate);
            var allWhitespace = true;
            foreach (var c in recipe.Name)
            {
                if (!char.IsWhiteSpace(c))
                {
                    allWhitespace = false;
                }
            }

            if (string.IsNullOrEmpty(recipe.Name) || allWhitespace)
            {
                w.Run<Messenger>(
                    m => UiHelpers.Write(
                        m.Subscriber,
                        () => m.GiveError("Please enter a recipe name.")));
                return;
            }

            bool updated;
            try
            {
                updated = w.Run<RecipeSaver, bool>(saver => saver.Save(recipe));
            }
            catch (ArgumentException)
            {
                w.Run<Messenger>(m =>
                    UiHelpers.Write(
                        m.Subscriber,
                        () => m.GiveError("Please enter a valid file name" +
                                          " for the recipe name.")));
                return;
            }

            this.resetRecipe();
            this.ui.WriteFinished.WaitOne();

            w.Run<Messenger>(m =>
                UiHelpers.Write(m.Subscriber, () =>
                    m.Inform(
                        updated
                            ? "Recipe updated!"
                            : "Recipe added!")));

            w.Run<LogEditor>(le => le.AddEntry(
                "Information",
                new[]
                {
                    updated
                        ? "A recipe was updated: " + recipe.Name
                        : "A new recipe was added: " + recipe.Name
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
                this.resetRecipe();
            }
        }

        private void ui_LookupKeyTapped()
        {
            var w = this.web;

            var recipeName = UiHelpers.Read(this.ui, () => this.ui.RecipeToAddUpdate.Name);
            var recipe = w.Run<RecipeLoader, Recipe>(loader =>
            {
                var recipes = loader.All();
                var match = default(Recipe);
                foreach (var r in recipes)
                {
                    if (string.Equals(
                        r.Name,
                        recipeName,
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        match = r;
                        break;
                    }
                }

                return match;
            });

            if (recipe != default(Recipe))
            {
                UiHelpers.Write(this.ui, () => this.ui.RecipeToAddUpdate = recipe);
            }
        }

        private void resetRecipe()
        {
            UiHelpers.Write(this.ui, () => this.ui.RecipeToAddUpdate = new Recipe());
        }

        private int setupIf1;
        private readonly AddUpdateUi ui;
        private readonly MethodWeb web;
    }
}
