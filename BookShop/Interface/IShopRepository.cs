using BookShop.Models;


namespace BookShop.Interface
{
    public interface IShopRepository
    {
        Task<List<Book>> GetBooks();
        Task<List<Book>> GetBooksInSeries(int id);
        Task<List<Author>> GetAuthors();
        Task<List<Publisher>> GetPublishers();
        Task<List<Category>> GetCategories();
        Task<List<Series>> GetSeries();

        Task<Book> GetBook(int id);

        Task<string> AddBook(Book book);
        Task<string> AddDetails(string where, string what);

        Task<bool> EditBook(Book book,int id);
        Task<List<Book>> Delete(int id);

        Task<List<Favourites>> AllFarourite(string name);
        Task<bool> FindBookInFav(string name, int id);
        Task<bool> AddToFavourite(int id, string name);

        Task<List<CardItem>> AllCardItem(string name);
        Task<bool> FindBookInCard(string name,int id);
        Task<bool> AddToCart(int id, string name);

        Task Plus(string name, int id);
        Task Minus(string name, int id);
        Task<bool> DeleteBookInCard(string name, int id);
        Task<bool> DeleteBookInFav(string name, int id);

    }
}
