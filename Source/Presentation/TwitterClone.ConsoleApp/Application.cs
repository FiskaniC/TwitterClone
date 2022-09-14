namespace TwitterClone.ConsoleApp
{
    using TwitterClone.Application.Interfaces;
    using TwitterClone.ConsoleApp.Interfaces;

    internal class Application : IRunnable
    {
        private readonly IFeedService _service;

        public Application(IFeedService service)
        {
            _service = service;
        }
        public void Run(string[] arguments)
        {
            _service.SimulateFeed(arguments);
        }
    }
}
