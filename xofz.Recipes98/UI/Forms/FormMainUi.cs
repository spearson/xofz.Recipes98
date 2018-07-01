﻿namespace xofz.Recipes98.UI.Forms
{
    using System.Threading;
    using System.Windows.Forms;
    using xofz.UI;
    using xofz.UI.Forms;

    public partial class FormMainUi : FormUi, MainUi, ShellUi
    {
        public FormMainUi()
        {
            this.InitializeComponent();
        }

        public event Action ShutdownRequested;

        public virtual NavUi NavUi => this.navUi;

        void ShellUi.SwitchUi(Ui newUi)
        {
            var control = newUi as Control;
            ControlHelpers.SafeReplace(control, this.screenPanel);
            if (control is UserControlLogUi)
            {
                control.Dock = DockStyle.Fill;
            }
        }

        private void this_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            ThreadPool.QueueUserWorkItem(
                o => this.ShutdownRequested?.Invoke());
        }
    }
}
