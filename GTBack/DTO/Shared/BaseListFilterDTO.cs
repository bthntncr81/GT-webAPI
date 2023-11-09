namespace GTBack.Core.DTO;

public class BaseListFilterDTO<T> where T:class
{
    public PaginationFilter PaginationFilter { get; set; }
    public SortingFilter SortingFilter { get; set; }
    public T? RequestFilter { get; set; }

}