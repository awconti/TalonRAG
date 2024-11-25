namespace TalonRAG.Application.Services
{
    /// <summary>
    /// Interface for service classes seeking to implement functionality on behalf of a console application.
    /// </summary>
    public interface IConsoleAppService
    {
        /// <summary>
        /// Executes the required task for the console application.
        /// </summary>
        Task RunAsync();
    }
}
