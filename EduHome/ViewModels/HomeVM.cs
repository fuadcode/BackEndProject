using EduHome.Models;


namespace EduHome.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public SliderText SliderText { get; set; }

        public IEnumerable<Event> Event { get; set; }

        public IEnumerable<Setting> Settings { get; set; }

        public IEnumerable<Main> Mains { get; set; }
        public IEnumerable<Card> Cards { get; set; }

        public CardText CardTexts { get; set; }
    }
}
