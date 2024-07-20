using EduHome.Models;
using System.Reflection.Metadata;

namespace EduHome.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public SliderText SliderText { get; set; }
    }
}
