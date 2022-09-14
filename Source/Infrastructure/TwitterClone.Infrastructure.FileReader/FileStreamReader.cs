namespace TwitterClone.Infrastructure
{
    using TwitterClone.Application.Interfaces;

    public class FileStreamReader : IStreamReader
    {
        public StreamReader StreamReader(string path)
        {
            return new StreamReader(path);
        }
    }
}
