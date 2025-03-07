namespace Application.Tests.Command;

using FluentValidation.TestHelper;
using MsClean.Application;

public class RegisterGovernmentEntityCommandValidationTest
{
    private static readonly RegisterGovernmentEntityCommandValidation _validator = new();

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_Name()
    {
        // Arrange
        var command = new RegisterGovernmentEntityCommand
        {
            Name = "Ministerio de Relaciones Publicas",
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Name);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Name()
    {
        // Arrange
        var command = new RegisterGovernmentEntityCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Name);
    }
}
