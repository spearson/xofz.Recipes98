namespace xofz.Recipes98.Root
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using xofz.Framework;
    using xofz.Framework.Implementation;
    using xofz.Framework.Materialization;
    using xofz.Presentation;
    using xofz.Recipes98.Presentation;
    using xofz.Recipes98.Root.Commands;
    using xofz.Recipes98.UI.Forms;
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
            AppDomain.CurrentDomain.UnhandledException 
                += this.handleUnhandledException;
        }

        public virtual Form Shell => this.shell;

        public virtual void Bootstrap()
        {
            if (Interlocked.CompareExchange(ref this.bootstrappedIf1, 1, 0) == 1)
            {
                return;
            }

            this.setShell(new FormMainUi());
            var s = this.shell;
            var e = this.executor;
            Messenger fm = new FormsMessenger { Subscriber = s };
            e.Execute(new SetupMethodWebCommand(
                fm));
            var w = e.Get<SetupMethodWebCommand>().Web;

            e
                .Execute(new SetupMainCommand(
                    s,
                    w))
                .Execute(new SetupShutdownCommand(
                    s,
                    w))
                .Execute(new SetupNavCommand(
                    s.NavUi,
                    s,
                    w))
                .Execute(new SetupAddUpdateCommand(
                    new UserControlAddUpdateUi(
                        new LinkedListMaterializer()),
                    s,
                    w))
                .Execute(new SetupNutritionalInfoCommand(
                    new UserControlNutritionalInfoUi(
                        new LinkedListMaterializer()),
                    s,
                    w))
                .Execute(new SetupRecipesCommand(
                    new UserControlRecipesUi(
                        new LinkedListMaterializer()),
                    s,
                    w))
                .Execute(new SetupLogCommand(
                    new UserControlLogUi(
                        new LinkedListMaterializer()),
                    s,
                    new FormLogEditorUi(
                        s,
                        new LinkedListMaterializer()),
                    new FormLogStatisticsUi(
                        s),
                    w));

            w.Run<Navigator>(n => n.Present<RecipesPresenter>());
        }

        private void setShell(FormMainUi shell)
        {
            this.shell = shell;
        }

        private void handleUnhandledException(
            object sender,
            UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            var exceptionLog = new TextFileLog("Exceptions.log");
            if (ex == null)
            {
                exceptionLog.AddEntry(
                    "Error",
                    new[]
                    {
                        "An unhandled exception occurred, "
                        + "but the exception did not derive "
                        + "from System.Exception.",
                        "Here is the exception's type: "
                        + e.ExceptionObject.GetType()
                    });
                return;
            }

            LogHelpers.AddEntry(exceptionLog, ex);
        }

        private int bootstrappedIf1;
        private FormMainUi shell;
        private readonly CommandExecutor executor;
    }
}
