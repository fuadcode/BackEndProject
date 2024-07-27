using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewModels
{
    public class PaginationVM<T> : List<T>
    {
        public PaginationVM(List<T> items, int currentPage, int totalPage)
        {
            CurrentPage = currentPage;
            TotalPage = totalPage;
            this.AddRange(items);
            int start = currentPage - 2;
            int end = currentPage + 2;
            if (start <= 0)
            {
                end -= (start - 1);
                start = 1;
            }
            if (end >= TotalPage)
            {
                end = totalPage;
                start = end - 4;
            }
            Start = start;
            End = end;
        }
        public int CurrentPage { get; }
        public int TotalPage { get; }
        public bool HasNext => CurrentPage < TotalPage;
        public bool HasPrev => CurrentPage > 1;
        public int Start { get; set; }
        public int End { get; set; }
        public static async Task<PaginationVM<T>> CreateVM(IQueryable<T> query, int stage, int take)
        {
            if (stage <= 0)
            {
                stage = 1;
            }
            var data = await query
                .Skip((stage - 1) * take)
                .Take(take)
                .ToListAsync();
            var totalPage = (int)Math.Ceiling((decimal)query.Count() / take);
            if (stage > totalPage)
            {
                stage = totalPage;
            }
            return new PaginationVM<T>(data, stage, totalPage);
        }
    }
}
//pagination
