namespace xofz.Recipes2k.Root
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using xofz.Framework;
    using xofz.Framework.Implementation;
    using xofz.Framework.Materialization;
    using xofz.Presentation;
    using xofz.Recipes2k.Presentation;
    using xofz.Recipes2k.Root.Commands;
    using xofz.Recipes2k.UI.Forms;
    using xofz.Root;
    using xofz.Root.Commands;
    using xofz.UI;
    using xofz.UI.Forms;

    public class FormsBootstrapper
    {
        public FormsBootstrapper()
            : this(new CommandExecutor())
        {
        }

        public FormsBootstrapper(CommandExecutor executor)
        {
            this.executor = executor;
        }

        public virtual Form MainForm => this.mainForm;

        public virtual void Bootstrap()
        {
            if (Interlocked.CompareExchange(ref this.bootstrappedIf1, 1, 0) == 1)
            {
                return;
            }

            this.setMainForm(new FormMainUi());
            var mf = this.mainForm;
            var e = this.executor;
            Messenger fm = new FormsMessenger { Subscriber = mf };

            try
            {


                e.Execute(new SetupMethodWebCommand(
                    fm));
                var w = e.Get<SetupMethodWebCommand>().Web;

                e
                    .Execute(new SetupMainCommand(
                        mf,
                        w))
                    .Execute(new SetupShutdownCommand(
                        mf,
                        w))
                    .Execute(new SetupNavCommand(
                        mf.NavUi,
                        mf,
                        w))
                    .Execute(new SetupAddUpdateCommand(
                        new UserControlAddUpdateUi(
                            new LinkedListMaterializer()),
                        mf,
                        w))
                    .Execute(new SetupRecipesCommand(
                        new UserControlRecipesUi(
                            new LinkedListMaterializer()),
                        mf,
                        w))
                    .Execute(new SetupLogCommand(
                        new UserControlLogUi(
                            new LinkedListMaterializer()),
                        mf,
                        new FormLogEditorUi(
                            mf,
                            new LinkedListMaterializer()),
                        w));

                w.Run<Navigator>(n => n.Present<RecipesPresenter>());
            }
            catch (Exception ex)
            {
                var log = new TextFileLog("Exceptions.log");
                LogHelpers.AddEntry(log, ex);
                fm.GiveError("Error bootstrapping");
            }
        }

        private void setMainForm(FormMainUi mainForm)
        {
            this.mainForm = mainForm;
        }

        private int bootstrappedIf1;
        private FormMainUi mainForm;
        private readonly CommandExecutor executor;
    }
}
