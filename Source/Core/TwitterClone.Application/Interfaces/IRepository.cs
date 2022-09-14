namespace TwitterClone.Application.Interfaces
{
    public interface IRepository<T>
    {
        IList<T> Get(string[] arguments);
    }
}
