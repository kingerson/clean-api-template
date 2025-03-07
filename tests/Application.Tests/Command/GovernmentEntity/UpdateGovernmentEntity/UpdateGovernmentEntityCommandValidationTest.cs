namespace Application.Tests.Command;
using FluentValidation.TestHelper;
using MsClean.Application;

public class UpdateGovernmentEntityCommandValidationTest
{
    private static readonly UpdateGovernmentEntityCommandValidation _validator = new();

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_Name()
    {
        // Arrange
        var command = new UpdateGovernmentEntityCommand
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
        var command = new UpdateGovernmentEntityCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Name);
    }
}
