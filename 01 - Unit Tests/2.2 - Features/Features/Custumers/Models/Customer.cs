using System.ComponentModel.DataAnnotations;
using Features.Core;
using Features.Costumers.Validators;

namespace Features.Costumers.Models;

public class Customer : Entity
{
    public string Name { get; private set; }
    public string LastName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public DateTime RegistrationDate { get; private set; }
    public string Email { get; private set; }
    public bool Active { get; private set; }

    protected Customer() { }

    public Customer(
        Guid id,
        string name,
        string lastName,
        DateTime birthDate,
        DateTime registrationDate,
        string email,
        bool active)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        BirthDate = birthDate;
        RegistrationDate = registrationDate;
        Email = email;
        Active = active;
    }

    public string FullName() => $"{Name} {LastName}";

    public bool IsSpecial() => RegistrationDate.CompareTo(DateTime.Now.AddYears(-3)) < 0 && Active;

    public void Deactivate() => Active = false;

    public override bool IsValid()
    {
        ValidationResult = new CustomerValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}