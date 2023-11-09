namespace GTBack.Core.Entities;

public class Company:BaseEntity
{
    public String Name { get; set; }
    public String Address { get; set; }
    public int CompanyTypeId { get; set; }
    public String Mail { get; set; }
    public String Phone { get; set; }
    public float Latitude { get; set; }
    public float Longtitude { get; set; }
    public  virtual  ICollection<User> User { get; set; }
    public virtual ICollection<FAQ>? Faq { get; set; }
    public  virtual  ICollection<EventTypeCompanyRelation> EventTypeCompanyRelation { get; set; }
    
}