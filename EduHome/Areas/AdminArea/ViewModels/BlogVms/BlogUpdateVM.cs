namespace EduHome.Areas.AdminArea.ViewModels.BlogVMs
{
    public class BlogUpdateVM
    {
        public IFormFile Photo { get; set; }
        public string Comment { get; set; }
        public string ImgUrl { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string DescPart { get; set; }
    }

    }

