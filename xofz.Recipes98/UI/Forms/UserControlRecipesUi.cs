namespace xofz.Recipes98.UI.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;
    using xofz.UI.Forms;

    public partial class UserControlRecipesUi : UserControlUi, RecipesUi
    {
        public UserControlRecipesUi(
            Lotter Lotter)
        {
            this.Lotter = Lotter;

            this.InitializeComponent();
        }

        public event Do SearchTextChanged;

        public event Do ClearSearchKeyTapped;

        public event Do<string> OpenRequested;

        public event Do<string> NutlInfoRequested;

        public event Do<string> DeleteRequested;
        
        string RecipesUi.NameSearchText
        {
            get => this.nameSearchTextBox.Text;

            set => this.nameSearchTextBox.Text = value;
        }

        string RecipesUi.DescriptionSearchText
        {
            get => this.descriptionSearchTextBox.Text;

            set => this.descriptionSearchTextBox.Text = value;
        }

        Lot<string> RecipesUi.IngredientsSearchText
        {
            get => this.Lotter.Materialize(this.ingredientsSearchTextBox.Lines);

            set => this.ingredientsSearchTextBox.Lines = EnumerableHelpers.ToArray(value);
        }

        Lot<string> RecipesUi.DirectionsSearchText
        {
            get => this.Lotter.Materialize(this.directionsSearchTextBox.Lines);

            set => this.directionsSearchTextBox.Lines = EnumerableHelpers.ToArray(value);
        }

        Lot<Recipe> RecipesUi.MatchingRecipes
        {
            get
            {
                var m = this.Lotter;
                var ll = new LinkedList<Recipe>();
                foreach (DataGridViewRow row in this.recipesGrid.Rows)
                {
                    ll.AddLast(
                        new Recipe(row.Cells[0].ToString())
                        {
                            Description = row.Cells[1].ToString(),
                            Directions = m.Materialize(
                                row.Cells[2]
                                    .ToString()
                                    .Split(
                                        new[] { Environment.NewLine },
                                        StringSplitOptions.RemoveEmptyEntries))
                        });
                }

                return m.Materialize(ll);
            }

            set
            {
                var rg = this.recipesGrid;
                rg.Rows.Clear();
                foreach (var recipe in value)
                {
                    rg.Rows.Add(
                        recipe.Name,
                        recipe.Description);
                    var calories = recipe
                        .NutritionalInfo
                        ?.Calories;
                    if (!StringHelpers.NullOrWhiteSpace(calories))
                    {
                        var c = rg.Rows[rg.Rows.Count - 2].Cells[3];
                        c.Value
                            = calories + " Calories";
                        c.ToolTipText
                            = calories + " Calories Per Serving";
                    }
                }
            }
        }

        private void clearSearchKey_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.ClearSearchKeyTapped?.Invoke());
        }

        private void nameSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.SearchTextChanged?.Invoke());
        }

        private void descriptionSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.SearchTextChanged?.Invoke());
        }

        private void ingredientsSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.SearchTextChanged?.Invoke());
        }

        private void directionsSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.SearchTextChanged?.Invoke());
        }

        private void recipesGrid_CellClick(object sender,
            DataGridViewCellEventArgs e)
        {
            var rg = this.recipesGrid;
            if (rg.Columns[e.ColumnIndex] is DataGridViewButtonColumn
                && e.RowIndex >= 0)
            {
                var row = rg.Rows[e.RowIndex];
                var recipeName = row.Cells[0].Value?.ToString();
                if (StringHelpers.NullOrWhiteSpace(recipeName))
                {
                    return;
                }

                var buttonType = rg.Columns[e.ColumnIndex].HeaderText;
                switch (buttonType)
                {
                    case "Open":
                        ThreadPool.QueueUserWorkItem(
                            o => this.OpenRequested?.Invoke(recipeName));
                        break;
                    case "Delete":
                        ThreadPool.QueueUserWorkItem(
                            o => this.DeleteRequested?.Invoke(recipeName));
                        break;
                    case "Nut.'l Info":
                        ThreadPool.QueueUserWorkItem(
                            o => this.NutlInfoRequested?.Invoke(recipeName));
                        break;
                }
            }
        }

        private readonly Lotter Lotter;
    }
}
