namespace xofz.Recipes98.UI.Forms
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using xofz.UI.Forms;

    public partial class UserControlAddUpdateUi : UserControlUi, AddUpdateUi
    {
        public UserControlAddUpdateUi(Lotter Lotter)
        {
            this.Lotter = Lotter;
            this.InitializeComponent();
        }

        public event Do AddUpdateKeyTapped;

        public event Do ResetKeyTapped;

        public event Do LookupKeyTapped;

        Recipe AddUpdateUi.RecipeToAddUpdate
        {
            get
            {
                var m = this.Lotter;
                return new Recipe(this.nameTextBox.Text)
                {
                    Description = this.descriptionTextBox.Text,
                    Ingredients = m.Materialize(this.ingredientsTextBox.Lines),
                    Directions = m.Materialize(this.directionsTextBox.Lines)
                };
            }

            set
            {
                this.nameTextBox.Text = value?.Name;
                this.descriptionTextBox.Text = value?.Description;
                this.ingredientsTextBox.Lines = EnumerableHelpers.ToArray(value?.Ingredients);
                this.directionsTextBox.Lines = EnumerableHelpers.ToArray(value?.Directions);
            }
        }

        private void resetKey_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.ResetKeyTapped?.Invoke());
        }

        private void addUpdateKey_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.AddUpdateKeyTapped?.Invoke());
        }

        private void lookupKey_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.LookupKeyTapped?.Invoke());
        }

        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                ThreadPool.QueueUserWorkItem(
                    o => this.LookupKeyTapped?.Invoke());
            }
        }

        private readonly Lotter Lotter;
    }
}
