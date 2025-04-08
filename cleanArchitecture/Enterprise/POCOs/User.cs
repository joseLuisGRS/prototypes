namespace Domain.POCOs;

public class User : Base
{
    public Int64 PersonId { get; set; }
    public Person Person { get; set; }
    public Int64 RoleId { get; set; }
    public Role Role { get; set; }
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

}
