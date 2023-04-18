namespace DDD.Shop.Domain.Core.Query;

public class PagedResult<T>
{
    public PagedResult()
    {
        
    }
    public PagedResult(List<T> results, int pageNumber, int pageSize, long total)
    {
        Results = results;
        Page = pageNumber;
        PageSize = pageSize;
        TotalCount = total;
    }

    public int Page { get; set; }   
    public int PageSize { get; set; }
    public long TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public IReadOnlyList<T> Results { get; set; }
}