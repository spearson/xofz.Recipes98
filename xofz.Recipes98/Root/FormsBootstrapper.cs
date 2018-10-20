namespace xofz.Recipes98.Root
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using xofz.Framework;
    using xofz.Framework.Logging;
    using xofz.Framework.Lotters;
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
        }

        public virtual Form Shell => this.shell;

        public virtual void Bootstrap()
        {
            if (Interlocked.CompareExchange(ref this.bootstrappedIf1, 1, 0) == 1)
            {
                return;
            }

            
            var e = this.executor;
            Messenger fm = new FormsMessenger();
            e.Execute(new SetupMethodWebCommand(
                fm,
                () => new MethodWeb()));
            AppDomain.CurrentDomain.UnhandledException
                += this.handleUnhandledException;

            var w = e.Get<SetupMethodWebCommand>().Web;
            this.setShell(new FormMainUi());
            var s = this.shell;
            fm.Subscriber = s;

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
                        new LinkedListLotter()),
                    s,
                    w))
                .Execute(new SetupNutritionalInfoCommand(
                    new UserControlNutritionalInfoUi(
                        new LinkedListLotter()),
                    s,
                    w))
                .Execute(new SetupRecipesCommand(
                    new UserControlRecipesUi(
                        new LinkedListLotter()),
                    s,
                    w))
                .Execute(new SetupLogCommand(
                    new UserControlLogUi(),
                    s,
                    new FormLogEditorUi(
                        s),
                    new FormLogStatisticsUi(
                        s),
                    w,                    
                    "Log.log",
                    null,
                    AccessLevel.None,
                    AccessLevel.None,
                    false,
                    () =>
                    {
                        var now = DateTime.Now;
                        return "Log backup "
                               + now.ToString("yyyy-MM-dd HH.mm.ss")
                               + ".log";
                    }));

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
            var w = this.executor.Get<SetupMethodWebCommand>().Web;
            w.Run<LogEditor>(le => LogHelpers.AddEntry(
                    le,
                    e),
                "Exceptions");
        }

        private int bootstrappedIf1;
        private FormMainUi shell;
        private readonly CommandExecutor executor;
    }
}
