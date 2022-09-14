namespace DesignPatterns.ConsoleApp.Concepts
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var allan = new User("Alan");
            var ward = new User("Ward");
            var martin = new User("Martin");


            ward.Follow(allan);
            allan.Follow(martin);
            ward.Follow(martin);

            allan.SendPost("If you have a procedure with 10 parameters, you probably missed some.");
            ward.SendPost("There are only two hard things in Computer Science: cache invalidation, naming things and off-by-1 errors.");
            allan.SendPost("Random numbers should not be generated with a method chosen at random.");

            var users = new List<User>()
            {
                allan, ward, martin
            }.OrderBy(x => x.Username);

            foreach (var user in users)
            {
                if (user != null)
                {
                    Console.WriteLine(user.Username);
                    foreach (var post in user.Posts)
                    {
                        Console.WriteLine($"\t@{post.Username}: {post.Message}");
                    }
                }
            }
        }
    }

    public class Post : EventArgs
    {
        private Post() { }

        public string Username { get; set; }
        public string Message { get; set; }

        public Post(string username, string message)
        {
            Username = username;
            Message = message;
        }
    }

    public class User
    {
        private User() { }

        public string Username { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();

        public User(string username)
        {
            this.Username = username;
            this.Follow(this);
        }

        private EventHandler<Post> postEvent;

        public void SendPost(string message)
        {
            var post = new Post(Username, message);
            if (postEvent != null)
            {
                postEvent(this, post);
            }
        }

        public void Follow(User user)
        {
            user.postEvent += ShowPost;
        }

        public void Unfollow(User user)
        {
            user.postEvent -= ShowPost;
        }

        public void ShowPost(object sender, Post post)
        {
            Posts.Add(post);
        }
    }
}
