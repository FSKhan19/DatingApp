namespace DatingApp.Backend.Models.Common
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; } = 1; // Default value
        public int PageSize { get; set; } = 10; // Default value
    }
}
