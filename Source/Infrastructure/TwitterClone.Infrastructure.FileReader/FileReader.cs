namespace TwitterClone.Infrastructure
{
    using TwitterClone.Application.Constants;
    using TwitterClone.Application.Interfaces;

    public class FileReader : IFileReader
    {
        IStreamReader _streamReader;

        public FileReader(IStreamReader streamReader)
        {
            _streamReader = streamReader;
        }

        public IList<string[]> ReadFileContentsAsArrays(string[] fileNames)
        {
            var filesAsStrings = new List<string[]>();
            foreach (var file in fileNames)
            {
                var path = $"{ApplicationConstants.FilesLocation}/{file}";
                filesAsStrings.Add(ReadFileAsStringArray(path));
            }
            return filesAsStrings;
        }

        private string[] ReadFileAsStringArray(string filePath)
        {
            var read = new string[] { };
            try
            {
                using (var reader = _streamReader.StreamReader(filePath))
                {

                    read = reader.ReadToEnd().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToArray();

                    if (read == null || !read.Any() || read.Any(x => string.IsNullOrEmpty(x)))
                    {
                        throw new ArgumentException("File has empty contents");
                    }
                }
            }
            catch (IOException exception)
            {
                throw new IOException($"The file could not be read, error message: {exception.Message}");
            }

            return read;
        }
    }
}
