namespace xofz.Recipes98.Root.Commands
{
    using xofz.Framework;
    using xofz.Recipes98.Presentation;
    using xofz.Recipes98.UI;
    using xofz.Root;
    using xofz.UI;

    public class SetupNavCommand : Command
    {
        public SetupNavCommand(
            NavUi ui, 
            ShellUi shell,
            MethodWeb web)
        {
            this.ui = ui;
            this.shell = shell;
            this.web = web;
        }

        public override void Execute()
        {
            new NavPresenter(
                this.ui, 
                this.shell, 
                this.web)
                .Setup();
        }

        private readonly NavUi ui;
        private readonly ShellUi shell;
        private readonly MethodWeb web;
    }
}
