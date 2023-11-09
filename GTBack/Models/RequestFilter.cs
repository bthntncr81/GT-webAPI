namespace GTBack.Core.Models;

public class RequestFilter
{
    public string Name { get; set; }
    public string TypeName { get; set; }
    public  RequestFilter(string name,string typeName)
    {
        this.Name = name;
        this.TypeName = typeName;
    }
   
}