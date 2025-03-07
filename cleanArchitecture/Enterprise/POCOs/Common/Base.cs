namespace Domain.POCOs.Common;

public abstract class Base
{
    public Int64 Id { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreationDate { get; set; }
    public Int64 CreationUserId { get; set; }
    public DateTime? ModificationDate { get; set; }
    public Int64? ModificationUserId { get; set; }
}
