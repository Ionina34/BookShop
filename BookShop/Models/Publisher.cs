using System.ComponentModel;

namespace BookShop.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [DisplayName("Издательство")]
        public string Name { get; set; }
    }
}
