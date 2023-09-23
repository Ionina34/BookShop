using BookShop.Models;
using BookShop.Interface;
using Microsoft.EntityFrameworkCore;
using BookShop.Data;
using NuGet.Packaging.Signing;

namespace BookShop.Service
{
    public class EFShopRepository : IShopRepository
    {
        private IdentityContext context;
        public EFShopRepository(IdentityContext db) => context = db;

        public Task<List<Book>> GetBooks()
        {
            return Task.FromResult(context.Books.Include(x => x.Author).Include(x => x.Publisher).Include(x => x.Category).Include(x => x.Series).ToList());
        }
        public  Task<List<Book>> GetBooksInSeries(int id)
        {
            var books = context.Books.Include(x => x.Author).Include(x => x.Publisher).Include(x => x.Category).Include(x => x.Series).ToList();
            return Task.FromResult(books.Where(o=>o.SeriesId==id).ToList());
        }
        public Task<List<Author>> GetAuthors()
        {
            return Task.FromResult(context.Authors.ToList());
        }
        public Task<List<Publisher>> GetPublishers()
        {
            return Task.FromResult(context.Publishers.ToList());
        }
        public Task<List<Category>> GetCategories()
        {
            return Task.FromResult(context.Categories.ToList());
        }
        public Task<List<Series>> GetSeries()
        {
            return Task.FromResult(context.Series.ToList());
        }

        public async Task<Book> GetBook(int id)
        {
            var books = await GetBooks();
            return (books.First(o => o.Id == id));
        }

        public async Task<string> AddBook(Book book)
        {
            try
            {
                context.Books.Add(book);
                var res = await context.SaveChangesAsync();
                if (res != 0)
                    return "Книга создана";
                else
                    return "Произошла Ошибка";
            }
            catch
            {
                return "Произошла ошибка, возможно не все поля заполнены";
            }
        }
        public async Task<string> AddDetails(string where, string what)
        {
            if (where == "Aurhor")
            {
                await AddAuthor(what);
                return "author";
            }
            else if (where == "Category")
            {
                await AddCategory(what);
                return "category";
            }
            else if (where == "Publisher")
            {
                await AddPublisher(what);
                return "publisher";
            }
            else if (where == "Series")
            {
                await AddSeries(what);
                return "series";
            }
            else return "non";
        }

        private async Task AddAuthor(string what)
        {
            var author = new Author
            {
                Name = what
            };
            await context.Authors.AddAsync(author);
            context.SaveChanges();
        }
        private async Task AddCategory(string what)
        {
            var author = new Category
            {
                Name = what
            };
            await context.Categories.AddAsync(author);
            context.SaveChanges();
        }
        private async Task AddPublisher(string what)
        {
            var author = new Publisher
            {
                Name = what
            };
            await context.Publishers.AddAsync(author);
            context.SaveChanges();
        }
        private async Task AddSeries(string what)
        {
            var author = new Series
            {
                Name = what
            };
            await context.Series.AddAsync(author);
            context.SaveChanges();
        }

        public async Task<bool> EditBook(Book newBook, int id)
        {

            var book = await GetBook(id);
            var d = book.Publisher.Name;
            if (book != null)
            {
                book.Title = newBook.Title;
                var author = context.Authors.FirstOrDefault(o => o.Name == newBook.Author.Name);
                if (author != null)
                    book.Author = author;
                var publisher = context.Publishers.FirstOrDefault(o => o.Name == newBook.Publisher.Name);
                if (publisher != null)
                    book.Publisher = publisher;
                var categories = context.Categories.FirstOrDefault(o => o.Name == newBook.Category.Name);
                if (categories != null)
                    book.Category = categories;
                var series = context.Series.FirstOrDefault(o => o.Name == newBook.Series.Name);
                if (series != null)
                    book.Series = series;
                book.Price = newBook.Price;
                book.BookImage = newBook.BookImage;
                book.Description = newBook.Description;
            }
            var res = await context.SaveChangesAsync();
            if (res != 0)
                return true;
            else
                return false;
        }
        public async Task<List<Book>> Delete(int id)
        {
            var book = context.Books.Where(o => o.Id == id).FirstOrDefault();
            if (book != null)
                context.Books.Remove(book);
            context.SaveChanges();
            return await GetBooks();
        }

        public Task<List<Favourites>> AllFarourite(string name)
        {
            var favourites = context.Favourites.Include(o => o.Book).ThenInclude(o => o.Author)
                .Include(o => o.Book).ThenInclude(o => o.Category)
                .Include(o => o.Book).ThenInclude(o => o.Publisher)
                .Include(o => o.Book).ThenInclude(o => o.Series);
            return Task.FromResult(favourites.Where(o => o.UserName == name).ToList());
        }
        public async Task<bool> FindBookInFav(string name, int id)
        {
            var favourites = await AllFarourite(name);
            return favourites.Any(o => o.BookId == id);
        }
        public async Task<bool> AddToFavourite(int id, string name)
        {
            //Все книги пользователя
            var books = context.Favourites.Where(o => o.UserName == name).ToList();
            var book = books.FirstOrDefault(o => o.BookId == id);
            if (book == null)
            {
                var b = new Favourites
                {
                    BookId = id,
                    UserName = name
                };
                context.Favourites.Add(b);
            }
            else
            {
                context.Favourites.Remove(book);
            }
            var res = await context.SaveChangesAsync();
            if (res != 0)
                return true;
            else
                return false;
        }

        public Task<List<CardItem>> AllCardItem(string name)
        {
            var items = context.CardItems.Include(o => o.Book).ThenInclude(o => o.Author)
                .Include(o => o.Book).ThenInclude(o => o.Category)
                .Include(o => o.Book).ThenInclude(o => o.Publisher)
                .Include(o => o.Book).ThenInclude(o => o.Series);
            return Task.FromResult(items.Where(o => o.UserName == name).ToList());
        }
        public async Task<bool> FindBookInCard(string name, int id)
        {
            var cards = await AllCardItem(name);
            return cards.Any(o => o.BookId == id);
        }
        public async Task<bool> AddToCart(int id, string name)
        {
            var items = context.CardItems.Where(o => o.UserName == name).ToList();
            var item = items.FirstOrDefault(o => o.BookId == id);
            if (item == null)
            {
                var i = new CardItem
                {
                    BookId = id,
                    UserName = name,
                    Count = 1
                };
                context.CardItems.Add(i);
            }
            var res = await context.SaveChangesAsync();
            if (res != 0)
                return true;
            else
                return false;
        }

        public Task Plus(string name, int id)
        {
            var items = context.CardItems.Where(o => o.UserName == name);
            var item = items.FirstOrDefault(o => o.BookId == id);
            if (item != null)
                item.Count++;
            context.SaveChanges();
            return Task.CompletedTask;
        }
        public Task Minus(string name, int id)
        {
            var items = context.CardItems.Where(o => o.UserName == name);
            var item = items.FirstOrDefault(o => o.BookId == id);
            if (item != null)
            {
                if (item.Count > 1)
                    item.Count--;
                else if (item.Count == 1)
                    context.CardItems.Remove(item);
            }

            context.SaveChanges();
            return Task.CompletedTask;
        }
        public async Task<bool> DeleteBookInCard(string name, int id)
        {
            var items = context.CardItems.Where(o => o.UserName == name);
            var item = items.FirstOrDefault(o => o.BookId == id);
            if (item != null)
                context.CardItems.Remove(item);
            var res = await context.SaveChangesAsync();
            if (res != 0)
                return true;
            else
                return false;
        }
        public async Task<bool> DeleteBookInFav(string name, int id)
        {
            var items = context.Favourites.Where(o => o.UserName == name);
            var item = items.FirstOrDefault(o => o.BookId == id);
            if (item != null)
                context.Favourites.Remove(item);
            var res = await context.SaveChangesAsync();
            if (res != 0)
                return true;
            else
                return false;
        }

    }
}
