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

        public event Do RecipesKeyTapped;

        public event Do AddKeyTapped;

        public event Do NutlInfoKeyTapped;

        public event Do LogKeyTapped;

        public event Do ExitKeyTapped;

        private void recipesKey_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.RecipesKeyTapped?.Invoke());
        }

        private void addKey_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.AddKeyTapped?.Invoke());
        }

        private void nutlInfoKey_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.NutlInfoKeyTapped?.Invoke());
        }

        private void logKey_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.LogKeyTapped?.Invoke());
        }

        private void exitKey_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                o => this.ExitKeyTapped?.Invoke());
        }
    }
}
