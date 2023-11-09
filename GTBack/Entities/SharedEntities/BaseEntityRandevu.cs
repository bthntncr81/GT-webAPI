namespace GTBack.Core.Entities.SharedEntities;

public abstract class BaseEntityRandevu
{
    public BaseEntityRandevu(){
    
        CreatedDate = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
        IsDeleted = false;
    }

    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}