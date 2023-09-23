using System.ComponentModel;

namespace BookShop.Models
{
    public class Author
    {
        public int Id { get; set; }

        [DisplayName("Имя автора")]
        public string Name { get; set; }
    }
}
