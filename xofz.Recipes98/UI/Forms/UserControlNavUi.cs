namespace xofz.Recipes98.UI.Forms
{
    using System;
    using System.Threading;
    using xofz.UI.Forms;

    public partial class UserControlNavUi : UserControlUi, NavUi
    {
        public UserControlNavUi()
        {
            this.InitializeComponent();
        }

        public event Action RecipesKeyTapped;

        public event Action AddKeyTapped;

        public event Action NutlInfoKeyTapped;

        public event Action LogKeyTapped;

        public event Action ExitKeyTapped;

        private void recipesKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.RecipesKeyTapped?.Invoke()).Start();
        }

        private void addKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.AddKeyTapped?.Invoke()).Start();
        }

        private void nutlInfoKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.NutlInfoKeyTapped?.Invoke()).Start();
        }

        private void logKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.LogKeyTapped?.Invoke()).Start();
        }

        private void exitKey_Click(object sender, EventArgs e)
        {
            new Thread(() => this.ExitKeyTapped?.Invoke()).Start();
        }
    }
}
