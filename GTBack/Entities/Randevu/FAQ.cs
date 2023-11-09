

using System.ComponentModel.DataAnnotations.Schema;
using GTBack.Core.Entities.SharedEntities;

namespace GTBack.Core.Entities;

public class FAQ:BaseEntity
{
    public String Question { get; set; }
    public String Answer { get; set; }
    public int Like { get; set; }
    public long CompanyId { get; set; }
    
    public long SenderUserId { get; set; }
    public long? AnsweredUserId { get; set; }
    public  User SenderUser { get; set; }
    public  User? AnsweredUser { get; set; }
    public  Company Company { get; set; }
    
}