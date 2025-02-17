using System.Net.Mail;
using CSharpFunctionalExtensions;
using ShopManager.Domain.Interfaces.BaseInterfaces;

namespace ShopManager.Domain.Models;

public record User : IModelKey<Guid>
{
    public const int MaxLengthNickname = 320;
    public const int MaxEmailLength = 320;
    public const int MaxNameLength = 50;

    public Guid Id { get; init; }

    public string Email { get; }

    public string UserName { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string MiddleName { get; }

    public DateOnly DateOfBirth { get; }

    public DateTime RegistrationDate { get; }

    private User(Guid id, string email, string userName, string firstName, string lastName, string middleName,
        DateOnly dateOfBirth, DateTime registrationDate)
    {
        Id = id;
        Email = email;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        RegistrationDate = registrationDate;
    }

    public static Result<User> Create(
        string email,
        string userName,
        string firstName,
        string lastName,
        string middleName,
        DateOnly dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return Result.Failure<User>("UserName cannot be empty");
        }

        if (userName.Length > MaxLengthNickname)
        {
            return Result.Failure<User>($"UserName cannot be longer than {MaxLengthNickname} characters");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<User>("Email cannot be empty");
        }

        if (IsValidEmail(email) == false)
        {
            return Result.Failure<User>("Email is incorrect");
        }

        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<User>("FirstName cannot be empty");
        }

        if (firstName.Length > MaxNameLength)
        {
            return Result.Failure<User>($"FirstName cannot be longer than {MaxNameLength} characters");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<User>("LastName cannot be empty");
        }

        if (lastName.Length > MaxNameLength)
        {
            return Result.Failure<User>($"LastName cannot be longer than {MaxNameLength} characters");
        }

        if (string.IsNullOrWhiteSpace(middleName))
        {
            return Result.Failure<User>("MiddleName cannot be empty");
        }

        if (middleName.Length > MaxNameLength)
        {
            return Result.Failure<User>($"MiddleName cannot be longer than {MaxNameLength} characters");
        }

        if (dateOfBirth > DateOnly.FromDateTime(DateTime.Now))
        {
            return Result.Failure<User>("Date of birth cannot be in the future");
        }

        return new User(Guid.Empty, email, userName, firstName, lastName, middleName, dateOfBirth, DateTime.Now);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var address = new MailAddress(email);
            return address.Address == email;
        }
        catch
        {
            return false;
        }
    }
}