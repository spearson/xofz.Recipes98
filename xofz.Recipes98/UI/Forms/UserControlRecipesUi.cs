﻿namespace xofz.Recipes2k.UI.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;
    using xofz.UI.Forms;

    public partial class UserControlRecipesUi : UserControlUi, RecipesUi
    {
        public UserControlRecipesUi(
            Materializer materializer)
        {
            this.materializer = materializer;

            this.InitializeComponent();
        }

        public event Action SearchTextChanged;

        public event Action ClearSearchKeyTapped;

        public event Action<string> OpenRequested;

        public event Action<string> DeleteRequested;
        
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

        MaterializedEnumerable<string> RecipesUi.IngredientsSearchText
        {
            get => this.materializer.Materialize(this.ingredientsSearchTextBox.Lines);

            set => this.ingredientsSearchTextBox.Lines = MEHelpers.ToArray(value);
        }

        MaterializedEnumerable<string> RecipesUi.DirectionsSearchText
        {
            get => this.materializer.Materialize(this.directionsSearchTextBox.Lines);

            set => this.directionsSearchTextBox.Lines = MEHelpers.ToArray(value);
        }

        MaterializedEnumerable<Recipe> RecipesUi.MatchingRecipes
        {
            get
            {
                var m = this.materializer;
                var ll = new LinkedList<Recipe>();
                foreach (DataGridViewRow row in this.recipesGrid.Rows)
                {
                    ll.AddLast(
                        new Recipe
                        {
                            Name = row.Cells[0].ToString(),
                            Description = row.Cells[1].ToString(),
                            Directions = m.Materialize(
                                row.Cells[2].ToString().Split(
                                    new[] { Environment.NewLine },
                                    StringSplitOptions.RemoveEmptyEntries))
                        });
                }

                return m.Materialize(ll);
            }

            set
            {
                this.recipesGrid.Rows.Clear();
                foreach (var recipe in value)
                {
                    this.recipesGrid.Rows.Add(
                        recipe.Name,
                        recipe.Description);
                }
            }
        }

        private void clearSearchKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.ClearSearchKeyTapped?.Invoke()).Start();
        }

        private void nameSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            new Thread(() => this.SearchTextChanged?.Invoke()).Start();
        }

        private void descriptionSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            new Thread(() => this.SearchTextChanged?.Invoke()).Start();
        }

        private void ingredientsSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            new Thread(() => this.SearchTextChanged?.Invoke()).Start();
        }

        private void directionsSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            new Thread(() => this.SearchTextChanged?.Invoke()).Start();
        }

        private void recipesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
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
                if (buttonType == "Open")
                {
                    new Thread(() => this.OpenRequested?.Invoke(recipeName)).Start();
                }
                else
                {
                    new Thread(() => this.DeleteRequested?.Invoke(recipeName)).Start();
                }
                
            }
        }

        private readonly Materializer materializer;
    }
}
