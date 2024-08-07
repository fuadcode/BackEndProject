namespace EduHome.ViewModels
{
    public class BasketVM
    {
        public int Id { get; set; }
        public int BasketCount { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ImageUrl { get; set; }
        public bool IsWishlist { get; set; }
    }
}
