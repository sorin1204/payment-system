namespace TMPPP.Domain.Entities;

public sealed class Customer
{
    public Customer(Guid id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public Guid Id { get; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    public void UpdateContact(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
