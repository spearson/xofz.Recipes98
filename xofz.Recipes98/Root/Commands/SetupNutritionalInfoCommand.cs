namespace xofz.Recipes98.Root.Commands
{
    using xofz.Framework;
    using xofz.Recipes98.Presentation;
    using xofz.Recipes98.UI;
    using xofz.Root;
    using xofz.UI;

    public class SetupNutritionalInfoCommand : Command
    {
        public SetupNutritionalInfoCommand(
            NutritionalInfoUi ui,
            ShellUi shell,
            MethodWeb web)
        {
            this.ui = ui;
            this.shell = shell;
            this.web = web;
        }

        public override void Execute()
        {
            new NutritionalInfoPresenter(
                    this.ui,
                    this.shell,
                    this.web)
                .Setup();
        }

        private readonly NutritionalInfoUi ui;
        private readonly ShellUi shell;
        private readonly MethodWeb web;
    }
}
