namespace TalonRAG.Application.Interfaces
{
    /// <summary>
    /// Interface for service classes seeking to implement console application functionality.
    /// </summary>
    public interface IConsoleAppService
    {
        /// <summary>
        /// Executes the required task for the console application.
        /// </summary>
        Task RunAsync();
    }
}
