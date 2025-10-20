namespace EmployeeManagement.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for generating random strings.
    /// </summary>
    public interface IRandomStringGenerator
    {
        /// <summary>
        /// Generates a random string asynchronously.
        /// </summary>
        Task<string> GenerateRandomStringAsync(int length);
    }
}
