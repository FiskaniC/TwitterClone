namespace TwitterClone.Application.Models
{
    public class User
    {
        public User()
        {

        }

        public User(string username)
        {
            this.Username = username;
        }

        public string? Username { get; set; }
        public List<User> Followers { get; set; } = new List<User>();
    }
}
