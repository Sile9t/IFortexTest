using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private ApplicationDbContext _context;
        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Author> GetAuthor()
        {
            var authorsWithOneBookWithLongestTitle = await _context.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .Take(5)
                .OrderByDescending(a => a.Books
                    .OrderByDescending(b => b.Title.Length)
                    .FirstOrDefault().Title.Length)
                .ThenBy(a => a.Id)
                .ToListAsync();
            //OutputFormatter has problem with serialization records with nested records,
            //so I decided to get another list of authors without nested records
            var author = await _context.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id.Equals(authorsWithOneBookWithLongestTitle.FirstOrDefault().Id));

            return author;
        }

        public async Task<List<Author>> GetAuthors()
        {
            var authorsIdsWithEvenBooksQuanityWrittenAfter2015 = await _context.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .Where(a => a.Books.Where(b => b.PublishDate.Year > 2015)
                    .Sum(b => b.QuantityPublished) % 2 == 0)
                .Select(a => a.Id)
                .ToListAsync();
            //OutputFormatter has problem with serialization records with nested records,
            //so I decided to get another list of authors without nested records
            var authors = await _context.Authors
                .Where(a => authorsIdsWithEvenBooksQuanityWrittenAfter2015.Contains(a.Id))
                .ToListAsync();

            return authors;
        }
    }
}
