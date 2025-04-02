namespace api.Helpers
{
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsSortDecsending { get; set; } = false;
        public int PageNumer { get; set; } = 1;
        public int PageSize { get; set; } = 2;
    }
}
