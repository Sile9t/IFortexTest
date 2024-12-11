using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class BookService : IBookService
    {
        private ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Book> GetBook()
        {
            var book = await _context.Books
                .AsNoTracking()
                .OrderByDescending(b => b.Price * b.QuantityPublished).FirstOrDefaultAsync();

            return book;
        }

        public async Task<List<Book>> GetBooks()
        {
            var books = await _context.Books
                .AsNoTracking()
                .Where(b => b.PublishDate > DateTime.Parse("25/05/2012") && b.Title.Contains("Red"))
                .ToListAsync();

            return books;
        }
    }
}
