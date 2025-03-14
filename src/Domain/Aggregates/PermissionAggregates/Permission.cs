namespace MsClean.Domain;
public class Permission : Entity
{
    public string EmployeeForename { get; private set; }
    public string EmployeeLastName { get; private set; }
    public int PermissionTypeId { get; private set; }
    public DateTime PermissionDate { get; private set; }
    public PermissionType PermissionType { get; private set; }
}