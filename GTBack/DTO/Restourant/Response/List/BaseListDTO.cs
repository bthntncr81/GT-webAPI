namespace GTBack.Core.DTO.Restourant.Response;

public class BaseListDTO<T1,T2> where T1:class where T2:class 
{
   public  ICollection<T1> List { get; set; }
   public  T2 Filter { get; set; }
}