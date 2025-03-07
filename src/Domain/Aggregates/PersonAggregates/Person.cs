namespace MsClean.Domain;
public class Person : Entity
{
    public string Name { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    public void Register(string name, string lastName, string email)
    {
        Name = name;
        LastName = lastName;
        Email = email;
    }

    public void Update(string lastName, string email)
    {
        LastName = lastName;
        Email = email;
    }

    public void Delete()
    {
        IsActive = false;
    }
}
