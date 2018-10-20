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
            var updatedRecipe = UiHelpers.Read(
                this.ui,
                () => this.ui.RecipeToAddUpdate);
            if (StringHelpers.NullOrWhiteSpace(updatedRecipe.Name))
            {
                w.Run<Messenger>(
                    m => UiHelpers.Write(
                        m.Subscriber,
                        () => m.GiveError("Please enter a recipe name.")));
                return;
            }

            var recipe = this.getRecipe(updatedRecipe.Name);
            if (recipe != null)
            {
                recipe.Description = updatedRecipe.Description;
                recipe.Ingredients = updatedRecipe.Ingredients;
                recipe.Directions = updatedRecipe.Directions;
                goto save;
            }

            recipe = updatedRecipe;

            save:
            bool? updated = null;
            w.Run<RecipeSaver>(saver =>
            {
                try
                {
                    updated = saver.Save(recipe);
                }
                catch (ArgumentException)
                {
                    w.Run<Messenger>(m =>
                        UiHelpers.Write(
                            m.Subscriber,
                            () => m.GiveError("Please enter a valid file name" +
                                              " for the recipe name.")));
                }
            });

            if (updated == null)
            {
                return;
            }

            this.resetRecipe();
            var u = updated.Value;
            w.Run<Messenger>(m =>
            {
                UiHelpers.Write(
                    m.Subscriber,
                    () =>
                    {
                        m.Inform(
                            u
                                ? "Recipe updated!"
                                : "Recipe added!");
                    });
            });

            w.Run<LogEditor>(le => le.AddEntry(
                "Information",
                new[]
                {
                    u
                        ? "A recipe was updated: " + updatedRecipe.Name
                        : "A new recipe was added: " + updatedRecipe.Name
                }));

            w.Run<Navigator, EventRaiser>((n, er) =>
            {
                var rUi = n.GetUi<RecipesPresenter, RecipesUi>();
                er.Raise(
                    rUi,
                    nameof(rUi.SearchTextChanged));
            });
        }

        private void ui_ResetKeyTapped()
        {
            var w = this.web;
            var response = Response.No;
            w.Run<Messenger>(m =>
            {
                response = UiHelpers.Read(
                    m.Subscriber,
                    () => m.Question("Really clear all fields?"));
            });

            if (response == Response.Yes)
            {
                this.resetRecipe();
            }
        }

        private void ui_LookupKeyTapped()
        {
            var w = this.web;

            var recipeName = UiHelpers.Read(
                this.ui, 
                () => this.ui.RecipeToAddUpdate.Name);
            w.Run<RecipeLoader>(loader =>
            {
                var recipes = loader.All();
                Recipe match = null;
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

                if (match != null)
                {
                    UiHelpers.Write(
                        this.ui,
                        () => this.ui.RecipeToAddUpdate = match);
                }
            });
        }

        private void resetRecipe()
        {
            var recipe = new Recipe();
            UiHelpers.WriteSync(
                this.ui, 
                () => this.ui.RecipeToAddUpdate = recipe);
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

        private int setupIf1;
        private readonly AddUpdateUi ui;
        private readonly MethodWeb web;
    }
}
