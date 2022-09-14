namespace TwitterClone.Repository.Repositories
{
    using System.Collections.Generic;
    using TwitterClone.Application.Interfaces;
    using TwitterClone.Application.Models;

    public class UsersRepository : IRepository<User>
    {
        private readonly IFileReader _fileReader;
        private readonly string usersDelimeter = " follows ";
        private readonly string multipleFollowerDelimter = ", ";

        public UsersRepository(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public IList<User> Get(string[] arguments)
        {
            var fileContentsAsArrays = _fileReader.ReadFileContentsAsArrays(arguments);

            if (fileContentsAsArrays == null || !fileContentsAsArrays.Any())
                return new List<User>();
            
            var rawUsers = fileContentsAsArrays
            .Where(x => x
                .Any(x => x
                    .Contains(usersDelimeter)))
            .SingleOrDefault();

            if (rawUsers == null || !rawUsers.Any())
                return new List<User>();

            var users = rawUsers
                .Select(x => x
                    .Split(usersDelimeter))
                .Select(x => new User
                {
                    Username = x[0],
                    Followers = x[1]
                    .Contains(multipleFollowerDelimter) ? x[1]
                        .Split(multipleFollowerDelimter)
                        .Select(x => new User(x))
                        .ToList() : new List<User>
                        {
                                        new User(x[1])
                        }
                })
                .GroupBy(x => x.Username)
                .Select(x => new User
                {
                    Username = x.Key,
                    Followers = x
                    .ToList()
                    .SelectMany(x => x.Followers)
                    .GroupBy(x => x.Username)
                    .Select(x => x
                        .First())
                    .ToList()
                })
                .ToList();

            var followersNotUsers = users
            .SelectMany(x => x.Followers)
            .GroupBy(x => x.Username)
            .Select(x => x
                .First())
            .Where(x => users
                .All(y => y.Username != x.Username));

            if (followersNotUsers.Any())
            {
                users.AddRange(followersNotUsers);
            }

            return users.OrderBy(x => x.Username).ToList();
        }
    }
}
