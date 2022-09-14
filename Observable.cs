namespace DesignPatterns.ConsoleApp.Concepts
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var allan = new User("Allan");
            var ward = new User("Ward");
            var martin = new User("Martin");

            allan.Subscribe(ward);
            martin.Subscribe(allan);
            martin.Subscribe(ward);

            allan.SendTweet("If you have a procedure with 10 parameters, you probably missed some.");
            ward.SendTweet("There are only two hard things in Computer Science: cache invalidation, naming things and off-by-1 errors.");
            allan.SendTweet("Random numbers should not be generated with a method chosen at random.");

            var users = new List<User>()
                {
                    allan, ward, martin
                }.OrderBy(x => x.Username);

            foreach (var user in users)
            {
                if (user != null)
                {
                    Console.WriteLine(user.Username);
                    foreach (var tweet in user.Tweets)
                    {
                        Console.WriteLine($"\t@{tweet.Username}: {tweet.Message}");
                    }
                }
            }
        }
    }

    /// <summary>
    /// The 'Subject' concrete class
    /// </summary>
    public partial class UserSubject
    {
        public List<Tweet> Tweets = new List<Tweet>();
        public List<User> Followers = new List<User>();

        public void Subscribe(User follower)
        {
            Followers.Add(follower);
        }

        public void Unsubscribe(User follower)
        {
            Followers.Remove(follower);
        }

        public void Notify(Tweet tweet)
        {
            Tweets.Add(tweet);
        }
    }

    /// <summary>
    /// The 'Observer' interface class
    /// </summary>
    public partial interface IUser
    {
        void SendTweet(string Message);
    }

    /// <summary>
    /// The 'Observer' concrete class
    /// The 'Absatract' concrete class
    /// The 'Invoker' class
    /// </summary>
    public partial class User : UserSubject, IUser
    {
        public string Username { get; set; }

        public User(string username)
        {
            Username = username;
            this.Subscribe(this);
        }

        public void SendTweet(string message)
        {
            var tweet = new Tweet(this.Username, message);
            var tweetCommand = new PublishTweetCommand(tweet);

            foreach (var follower in Followers)
            {
                tweetCommand.Execute();
                follower.Notify(tweet);
            }
        }
    }

    /// <summary>
    /// The 'Reciever' class
    /// </summary>
    public class Tweet
    {
        public string Username { get; set; }
        public string Message { get; set; }

        public Tweet()
        {

        }
        public Tweet(string username, string message)
        {
            Username = username;
            Message = message;
        }

        public void PublishTweet(Tweet tweet)
        {
            Console.WriteLine(tweet.Message);
        }
    }

    /// <summary>
    /// The ICommand interface declares a method for 
    /// executing a command 
    /// </summary>
    public interface ICommand
    {
        void Execute();
    }

    public class PublishTweetCommand : ICommand
    {
        private Tweet Tweet;
        public PublishTweetCommand(Tweet tweet)
        {
            Tweet = tweet;
        }

        public void Execute()
        {
            Tweet.PublishTweet(Tweet);
        }
    }
}

