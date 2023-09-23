namespace BookShop.Models
{
    public class Favourites
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string UserName { get; set; }
    }
}
