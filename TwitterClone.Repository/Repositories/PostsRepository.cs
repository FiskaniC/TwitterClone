namespace TwitterClone.Repository.Repositories
{
    using System.Collections.Generic;
    using TwitterClone.Application.Interfaces;
    using TwitterClone.Application.Models;

    public class PostsRepository : IRepository<Post>
    {
        private readonly IFileReader _fileReader;
        private readonly string postsDelimeter = "> ";

        public PostsRepository(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public IList<Post> Get(string[] arguments)
        {
            var fileContentsAsArrays = _fileReader.ReadFileContentsAsArrays(arguments);

            if (fileContentsAsArrays == null || !fileContentsAsArrays.Any())
                return new List<Post>();

            var rawPosts = fileContentsAsArrays
            .Where(x => x
                .Any(x => x
                    .Contains(postsDelimeter)))
            .SingleOrDefault();

            if (rawPosts == null || !rawPosts.Any())
                return new List<Post>();

            var posts = rawPosts
                .Select(x => x
                    .Split(postsDelimeter))
                .Select(x => new Post
                {
                    Username = x[0],
                    Message = x[1]
                })
                .ToList();

            return posts;
        }
    }
}
