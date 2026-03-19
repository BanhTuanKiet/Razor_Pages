namespace WebApplication1.DTOs
{
    public class FilterDto
    {
        public class AgGridFilter
        {
            public string FilterType { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string Filter { get; set; } = string.Empty;
        }

        public class AgGridSort
        {
            public string Key { get; set; } = string.Empty;
            public string Sort { get; set; } = string.Empty;
        }

        public class AgGridRequest
        {
            public Dictionary<string, AgGridFilter> Filters { get; set; } = new();
            public List<AgGridSort> Sorts { get; set; } = new();
            public int Page { get; set; } = 0;
            public int PageSize { get; set; } = 10;
        }

        public class PagedResult<T>
        {
            public List<T> Data { get; set; } = new();
            public int TotalCount { get; set; }
            public int TotalPages { get; set; }
        }
    }
}