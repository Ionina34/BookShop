using System.ComponentModel;

namespace BookShop.Models
{
    public class Category
    {
        public int Id { get; set; }

        [DisplayName("Жанр")]
        public string Name { get; set; }
    }
}
