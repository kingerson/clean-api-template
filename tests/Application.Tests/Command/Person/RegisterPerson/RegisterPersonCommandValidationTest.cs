namespace Application.Tests.Command;
using FluentValidation.TestHelper;
using MsClean.Application;

public class RegisterPersonCommandValidationTest
{
    private static readonly RegisterPersonCommandValidation _validator = new();

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_Name()
    {
        // Arrange
        var command = new RegisterPersonCommand
        {
            Name = "Gerson",
            LastName = "Navarro",
            Email = "g.navarrope@gmail.com"
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
        var command = new RegisterPersonCommand
        {
            LastName = "Navarro",
            Email = "g.navarrope@gmail.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Name);
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_LastName()
    {
        // Arrange
        var command = new RegisterPersonCommand
        {
            Name = "Gerson",
            LastName = "Navarro",
            Email = "g.navarrope@gmail.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.LastName);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_LastName()
    {
        // Arrange
        var command = new RegisterPersonCommand
        {
            Name = "Gerson",
            Email = "g.navarrope@gmail.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.LastName);
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationErrorFor_Email()
    {
        // Arrange
        var command = new RegisterPersonCommand
        {
            Name = "Gerson",
            LastName = "Navarro",
            Email = "g.navarrope@gmail.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Email);
    }

    [Fact]
    public void Validator_ShouldHaveValidationErrorFor_Email()
    {
        // Arrange
        var command = new RegisterPersonCommand
        {
            Name = "Gerson",
            LastName = "Navarro"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Email);
    }
}
