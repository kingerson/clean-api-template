namespace Application.Tests.Command;

using FluentValidation.TestHelper;
using MsClean.Application;

public class DeleteGovernmentEntityCommandValidationTest
{
    private static readonly DeleteGovernmentEntityCommandValidation _validator = new();

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_Id()
    {
        // Arrange
        var command = new DeleteGovernmentEntityCommand
        {
            Id = Guid.NewGuid(),
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Id);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Id()
    {
        // Arrange
        var command = new DeleteGovernmentEntityCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Id);
    }
}
