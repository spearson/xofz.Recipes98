namespace xofz.Recipes98.Presentation
{
    using System.Collections.Generic;
    using System.Threading;
    using xofz.Framework;
    using xofz.Framework.Logging;
    using xofz.Framework.Materialization;
    using xofz.Presentation;
    using xofz.Recipes98.Framework;
    using xofz.Recipes98.UI;
    using xofz.UI;

    public sealed class RecipesPresenter : Presenter
    {
        public RecipesPresenter(
            RecipesUi ui, 
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

            this.ui.SearchTextChanged += this.ui_SearchTextChanged;
            this.ui.ClearSearchKeyTapped += this.ui_ClearSearchKeyTapped;
            this.ui.OpenRequested += this.ui_OpenRequested;
            this.ui.DeleteRequested += this.ui_DeleteRequested;
            this.ui.NutlInfoRequested += this.ui_NutlInfoRequested;
            this.web.Run<RecipeLoader>(loader =>
            {
                var recipes = loader.All();
                UiHelpers.Write(this.ui, () => this.ui.MatchingRecipes
                    = new LinkedListMaterializedEnumerable<Recipe>(recipes));
            });

            this.web.Run<Navigator>(n => n.RegisterPresenter(this));
        }

        private void ui_OpenRequested(string recipeName)
        {
            var w = this.web;
            this.pingOtherPresenters(recipeName);
            w.Run<Navigator>(n => n.Present<AddUpdatePresenter>());
        }

        private void ui_NutlInfoRequested(string recipeName)
        {
            var w = this.web;
            this.pingOtherPresenters(recipeName);
            w.Run<Navigator>(n => n.Present<NutritionalInfoPresenter>());
        }

        private void pingOtherPresenters(string recipeName)
        {
            var w = this.web;
            w.Run<Navigator, EventRaiser>((n, er) =>
            {
                var addUi = n.GetUi<AddUpdatePresenter, AddUpdateUi>();
                var recipe = new Recipe(recipeName);

                UiHelpers.Write(
                    addUi,
                    () => addUi.RecipeToAddUpdate = recipe);
                addUi.WriteFinished.WaitOne();
                er.Raise(
                    addUi,
                    nameof(addUi.LookupKeyTapped));

                var niUi = n.GetUi<NutritionalInfoPresenter,
                    NutritionalInfoUi>();
                UiHelpers.Write(
                    niUi,
                    () => niUi.LookupRecipeName = recipeName);
                niUi.WriteFinished.WaitOne();
                er.Raise(niUi, nameof(niUi.LookupKeyTapped));
            });
        }

        private void ui_DeleteRequested(string recipeName)
        {
            var w = this.web;
            var response = Response.No;
            w.Run<Messenger>(m =>
            {
                UiHelpers.Write(
                    m.Subscriber,
                    () => response = m.Question(
                        "Really delete " + recipeName + "?"));
                m.Subscriber.WriteFinished.WaitOne();
            });

            if (response == Response.Yes)
            {
                w.Run<RecipeSaver>(saver => saver.Delete(recipeName));
                w.Run<LogEditor>(le => le.AddEntry(
                    "Information",
                    new[]
                    {
                        "A recipe was deleted: " + recipeName
                    }));
                this.ui_SearchTextChanged();
            }
        }

        private void ui_ClearSearchKeyTapped()
        {
            UiHelpers.Write(this.ui, () =>
            {
                this.ui.NameSearchText = null;
                this.ui.DescriptionSearchText = null;
                this.ui.IngredientsSearchText = null;
                this.ui.DirectionsSearchText = null;
            });
        }

        private void ui_SearchTextChanged()
        {
            var w = this.web;
            var nst = UiHelpers.Read(this.ui, () => this.ui.NameSearchText);
            var dest = UiHelpers.Read(this.ui, () => this.ui.DescriptionSearchText);
            var ingst = UiHelpers.Read(this.ui, () => this.ui.IngredientsSearchText);
            var dist = UiHelpers.Read(this.ui, () => this.ui.DirectionsSearchText);
            var filledIngredients = new LinkedList<string>();
            var filledDirections = new LinkedList<string>();
            foreach (var ingString in ingst)
            {
                if (!StringHelpers.NullOrWhiteSpace(ingString))
                {
                    filledIngredients.AddLast(ingString);
                }
            }

            foreach (var diString in dist)
            {
                if (!StringHelpers.NullOrWhiteSpace(diString))
                {
                    filledDirections.AddLast(diString);
                }
            }

            var matches = new LinkedList<Recipe>();
            w.Run<RecipeLoader>(loader =>
            {
                var allRecipes = loader.All();
                foreach (var recipe in allRecipes)

                {
                    bool match = true;
                    if (!StringHelpers.NullOrWhiteSpace(nst))
                    {
                        if (!recipe.Name.ToLowerInvariant()
                            .Contains(nst.ToLowerInvariant()))
                        {
                            match = false;
                        }
                    }

                    if (!StringHelpers.NullOrWhiteSpace(dest))
                    {
                        if (!recipe.Description
                            .ToLowerInvariant()
                            .Contains(dest.ToLowerInvariant()))
                        {
                            match = false;
                        }
                    }

                    if (filledIngredients.Count > 0)
                    {
                        var overallContains = true;
                        foreach (var fi in filledIngredients)
                        {
                            var containsThisOne = false;
                            foreach (var ri in recipe.Ingredients)
                            {
                                if (ri.ToLowerInvariant()
                                    .Contains(fi.ToLowerInvariant()))
                                {
                                    containsThisOne = true;
                                }
                            }

                            if (!containsThisOne)
                            {
                                overallContains = false;
                                break;
                            }
                        }

                        if (!overallContains)
                        {
                            match = false;
                        }
                    }

                    if (filledDirections.Count > 0)
                    {
                        var overallContains = true;
                        foreach (var fd in filledDirections)
                        {
                            var containsThisOne = false;
                            foreach (var rd in recipe.Directions)
                            {
                                if (rd.ToLowerInvariant()
                                    .Contains(fd.ToLowerInvariant()))
                                {
                                    containsThisOne = true;
                                }
                            }

                            if (!containsThisOne)
                            {
                                overallContains = false;
                                break;
                            }
                        }

                        if (!overallContains)
                        {
                            match = false;
                        }
                    }

                    if (match)
                    {
                        matches.AddLast(recipe);
                    }
                }
            });

            var uiMatches = new LinkedListMaterializedEnumerable<Recipe>(
                matches);
            UiHelpers.Write(
                this.ui,
                () => this.ui.MatchingRecipes = uiMatches);
        }

        private int setupIf1;
        private readonly RecipesUi ui;
        private readonly MethodWeb web;
    }
}
