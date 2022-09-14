namespace TwitterClone.Application.Interfaces
{
    public interface IFileReader
    {
        IList<string[]> ReadFileContentsAsArrays(string[] fileNames);
    }
}
