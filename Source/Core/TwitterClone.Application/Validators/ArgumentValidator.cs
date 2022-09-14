using TwitterClone.Application.Constants;

namespace TwitterClone.Application.Validators
{
    public class ArgumentValidator
    {
        public void ValidateArguments(string[] arguments)
        {
            if (arguments != null)
            {
                var missingArguements = ApplicationConstants.MandatoryArguments.Except(arguments);
                if (missingArguements != null && missingArguements.Any())
                {
                    throw new ArgumentException($"The files were not passed as arguments, missing arguments: {string.Join(", ", missingArguements.ToList())}");
                }
            }
        }
    }
}
