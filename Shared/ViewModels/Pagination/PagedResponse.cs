
namespace Shared.ViewModels.Pagination
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public T Data { get; set; }

        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
        }
    }
}
