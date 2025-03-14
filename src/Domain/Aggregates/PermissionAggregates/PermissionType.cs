namespace MsClean.Domain;

public class PermissionType : Entity
{
    public string Description { get; private set; }

    public ICollection<Permission> Permissions { get; private set; } = [];
}