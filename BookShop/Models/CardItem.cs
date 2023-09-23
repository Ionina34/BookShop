namespace BookShop.Models
{
    public class CardItem
    {
        public int Id { get; set; }
        public int BookId { get; set;}
        public Book Book { get; set; }
        public string UserName { get; set; }
        public int Count { get; set; }
    }
}
