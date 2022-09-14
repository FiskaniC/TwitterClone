namespace TwitterClone.Application.Services
{
    using System;
    using TwitterClone.Application.Exceptions;
    using TwitterClone.Application.Interfaces;
    using TwitterClone.Application.Models;
    using TwitterClone.Application.Validators;

    public class FeedService : IFeedService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Post> _postsRepository;
        private readonly ArgumentValidator _argumentValidator;

        public FeedService(IRepository<User> userRepository, IRepository<Post> postsRepository)
        {
            _argumentValidator = new ArgumentValidator();
            _userRepository = userRepository;
            _postsRepository = postsRepository;
        }

        public bool SimulateFeed(string[] arguments)
        {
            // Validate passed arguments
            _argumentValidator.ValidateArguments(arguments);

            // Get users and posts
            var users = _userRepository.Get(arguments);
            var posts = _postsRepository.Get(arguments);

            //// Validte users and posts
            Validate(users, posts);

            //// Print out posts
            StreamPosts(users, posts);

            return true;
        }

        private void Validate(IList<User> users, IList<Post> posts)
        {
            if ((users == null || !users.Any()) || (posts == null || !posts.Any()))
            {
                throw new ArgumentException("User or tweet contents does not exist");
            }

            var invalidUsers = posts
            .Where(x => users
                .All(y => y.Username != x.Username));

            if (invalidUsers.Any())
            {
                throw new NotFoundException($"Users do not exist from tweet.txt, users [{string.Join(",", invalidUsers.Select(x => x.Username))}]");
            }
        }

        private void StreamPosts(IList<User> users, IList<Post> posts)
        {
            foreach (var user in users)
            {
                if (user != null)
                {
                    Console.WriteLine(user.Username);
                    foreach (var post in posts)
                    {
                        if (post.Username == user.Username || user.Followers.Any(x => x.Username.Equals(post.Username)))
                            Console.WriteLine($"\t@{post.Username}: {post.Message}");
                    }
                }
            }
        }
    }
}
