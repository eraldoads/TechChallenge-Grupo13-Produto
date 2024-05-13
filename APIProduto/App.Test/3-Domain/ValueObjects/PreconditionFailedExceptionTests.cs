using Xunit;

namespace Domain.ValueObjects.Tests
{
    public class PreconditionFailedExceptionTests
    {
        [Trait("Categoria", "PreconditionFailedException")]
        [Fact(DisplayName = "PreconditionFailedException Lan�a a Exce��o Correta")]
        public void PreconditionFailedException_ThrowsCorrectException()
        {
            // Arrange
            string message = "Test message"; // substitua pela mensagem de teste
            string paramName = "Test parameter"; // substitua pelo nome do par�metro de teste

            // Act & Assert
            var exception = Assert.ThrowsAsync<PreconditionFailedException>(async () => await Task.Run(() => throw new PreconditionFailedException(message, paramName)));
            Assert.Equal(message + " (Parameter '" + paramName + "')", exception.Result.Message);
            Assert.Equal(paramName, exception.Result.ParamName);
        }
    }
}
