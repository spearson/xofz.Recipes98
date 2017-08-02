﻿namespace xofz.Recipes2k.Root.Commands
{
    using xofz.Framework;
    using xofz.Recipes2k.Presentation;
    using xofz.Recipes2k.UI;
    using xofz.Root;
    using xofz.UI;

    public class SetupRecipesCommand : Command
    {
        public SetupRecipesCommand(
            RecipesUi ui,
            ShellUi shell,
            MethodWeb web)
        {
            this.ui = ui;
            this.shell = shell;
            this.web = web;
        }

        public override void Execute()
        {
            new RecipesPresenter(
                    this.ui,
                    this.shell,
                    this.web)
                .Setup();
        }

        private readonly RecipesUi ui;
        private readonly ShellUi shell;
        private readonly MethodWeb web;
    }
}
