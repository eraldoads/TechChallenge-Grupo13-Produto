namespace Domain.ValueObjects
{
    /// <summary>
    /// Construtor para a classe ResourceNotFoundException.
    /// </summary>
    /// <param name="message">A mensagem de erro a ser associada à exceção.</param>
    public class ResourceNotFoundException(string message) : Exception(message)
    {
    }
}
