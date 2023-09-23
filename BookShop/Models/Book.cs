using System.ComponentModel;

namespace BookShop.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [DisplayName("Цена товара")]
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public string BookImage { get; set; }
        public string Description { get; set; }
        public int SeriesId { get; set; }
        public Series Series { get; set; }
    }
}
