namespace xofz.Recipes98.Root.Commands
{
    using xofz.Framework;
    using xofz.Framework.Logging.Logs;
    using xofz.Presentation;
    using xofz.Root;
    using xofz.UI;

    public class SetupMethodWebCommand : Command
    {
        public SetupMethodWebCommand(
            Messenger messenger,
            Func<MethodWeb> createWeb)
        {
            this.messenger = messenger;
            this.createWeb = createWeb;
        }

        public virtual MethodWeb Web => this.web;

        public override void Execute()
        {
            this.setWeb(this.createWeb());

            this.registerDependencies();
        }

        private void setWeb(MethodWeb web)
        {
            this.web = web;
        }

        private void registerDependencies()
        {
            var w = this.web;
            w.RegisterDependency(new Navigator(w));
            w.RegisterDependency(new EventRaiser());
            w.RegisterDependency(this.messenger);
            w.RegisterDependency(
                new TextFileLog("Exceptions.log"),
                "Exceptions");
        }

        private MethodWeb web;
        private readonly Messenger messenger;
        private readonly Func<MethodWeb> createWeb;
    }
}
