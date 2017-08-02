namespace xofz.Recipes2k.UI.Forms
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using xofz.UI.Forms;

    public partial class UserControlAddUpdateUi : UserControlUi, AddUpdateUi
    {
        public UserControlAddUpdateUi(Materializer materializer)
        {
            this.materializer = materializer;
            this.InitializeComponent();
        }

        public event Action AddUpdateKeyTapped;

        public event Action ResetKeyTapped;

        public event Action LookupKeyTapped;

        Recipe AddUpdateUi.RecipeToAddUpdate
        {
            get
            {
                var m = this.materializer;
                return new Recipe
                {
                    Name = this.nameTextBox.Text,
                    Description = this.descriptionTextBox.Text,
                    Ingredients = m.Materialize(this.ingredientsTextBox.Lines),
                    Directions = m.Materialize(this.directionsTextBox.Lines)
                };
            }

            set
            {
                this.nameTextBox.Text = value?.Name;
                this.descriptionTextBox.Text = value?.Description;
                this.ingredientsTextBox.Lines = MEHelpers.ToArray(value?.Ingredients);
                this.directionsTextBox.Lines = MEHelpers.ToArray(value?.Directions);
            }
        }

        private void resetKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.ResetKeyTapped?.Invoke()).Start();
        }

        private void addUpdateKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.AddUpdateKeyTapped?.Invoke()).Start();
        }

        private void lookupKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.LookupKeyTapped?.Invoke()).Start();
        }

        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                new Thread(() => this.LookupKeyTapped?.Invoke()).Start();
            }
        }

        private readonly Materializer materializer;
    }
}
