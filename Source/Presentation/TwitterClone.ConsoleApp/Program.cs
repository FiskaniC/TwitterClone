namespace TwitterClone.ConsoleApp
{
    using Microsoft.Extensions.DependencyInjection;
    using TwitterClone.Application.Interfaces;
    using TwitterClone.Application.Models;
    using TwitterClone.Application.Services;
    using TwitterClone.Infrastructure;
    using TwitterClone.Repository.Repositories;

    internal class Program
    {
        private static ServiceProvider? _serviceProvider;

        static void Main(string[] args)
        {
            try
            {
                Initialize();
                Application application = new Application(service: _serviceProvider.GetService<IFeedService>());
                application.Run(args);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private static void Initialize()
        {
            IServiceCollection services = new ServiceCollection()
                .AddSingleton<IFeedService, FeedService>()
                .AddSingleton<IRepository<User>, UsersRepository>()
                .AddSingleton<IRepository<Post>, PostsRepository>()
                .AddSingleton<IFileReader, FileReader>()
                .AddSingleton<IStreamReader, FileStreamReader>();
            _serviceProvider = services.BuildServiceProvider();
        }
    }
}