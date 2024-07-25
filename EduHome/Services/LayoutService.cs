
using EduHome.Data;
using EduHome.Services.Interfaces;

namespace EduHome.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly EduDbContext _dbContext;

        public LayoutService(EduDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IDictionary<string, string> GetSettings() => _dbContext.WebSettings
            .ToDictionary(k => k.Key, k => k.Value);

        //public IActionResult GetTeachers() => _dbContext.Teachers.ToList();

    }
}
