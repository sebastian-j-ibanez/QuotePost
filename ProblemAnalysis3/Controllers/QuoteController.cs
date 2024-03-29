using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProblemAnalysis3.DataAccess;
using ProblemAnalysis3.Entities;
using ProblemAnalysis3.Models;

namespace ProblemAnalysis3.Controllers
{
    public class QuoteController : Controller
    {
        private QuoteDbContext _quoteDbContext;

        public QuoteController(QuoteDbContext quoteDbContext)
        {
            _quoteDbContext = quoteDbContext;
        }

        // Get all quotes.
        [HttpGet("/api/quotes")]
        public IActionResult GetAllQuotes()
        {
            // Select all quotes from DbContext.
            var quotes = _quoteDbContext.Quotes
                .OrderBy(q => q.QuoteId)
                .ToList();

            // Iterate through each quote,
            // add all associated quote tags,
            // EXCEPT the quote reference in QuoteTag to avoid circular reference.
            foreach (Quote quote in quotes)
            {
                quote.QuoteTags = _quoteDbContext.QuoteTags
                    .Where(qt => qt.QuoteId == quote.QuoteId)
                    .Select(qt => new QuoteTag()
                    {
                        QuoteId = quote.QuoteId, 
                        TagId = qt.TagId,
                        Tag = qt.Tag
                    })
                    .ToList();
            }

            return Ok(quotes);
        }

        // Get all quotes by tag id.
        [HttpGet("/api/quotes/tag/{tagId}")]
        public IActionResult GetAllQuotesByTag(int tagId)
        {
            // Get all quotes.
            var quotes = _quoteDbContext.Quotes
                .Where(q => q.QuoteTags.Any(t => t.TagId == tagId))
                .OrderBy(q => q.QuoteId)
                .ToList();

            // Iterate through each quote,
            // add all associated quote tags,
            // EXCEPT the quote reference in QuoteTag to avoid circular reference.
            foreach (Quote quote in quotes)
            {
                quote.QuoteTags = _quoteDbContext.QuoteTags
                    .Where(qt => qt.QuoteId == quote.QuoteId)
                    .Select(qt => new QuoteTag()
                    {
                        QuoteId = quote.QuoteId, 
                        TagId = qt.TagId,
                        Tag = qt.Tag
                    })
                    .ToList();
            }

            return Ok(quotes);
        }

        // Find a quote by Id.
        [HttpGet("/api/quotes/{quoteId}")]
        public IActionResult GetQuoteById(int quoteId)
        {
            // Find a quote from the database that matches the route's quoteId.
            var quote = _quoteDbContext.Quotes
                .Where(q => q.QuoteId == quoteId)
                .FirstOrDefault();

            // Manually set all quote tags associated with the found quote,
            // EXCEPT for the quote to avoid circular reference.
            quote.QuoteTags = _quoteDbContext.QuoteTags
                .Where(qt => qt.QuoteId == quote.QuoteId)
                .Select(qt => new QuoteTag()
                {
                    QuoteId = quote.QuoteId, 
                    TagId = qt.TagId,
                    Tag = qt.Tag
                })
                .ToList();
                

            return Ok(quote);
        }

        // Add a quote.
        [HttpPost("/api/quotes")]
        public IActionResult AddQuote([FromBody()]AddQuoteRequest addQuoteRequest)
        {
            Quote newQuote = new Quote()
            {
                Content = addQuoteRequest.Content,
                Author = addQuoteRequest.Author,
                QuoteTags = new List<QuoteTag>(),
            };
            
            var result= _quoteDbContext.Quotes.Add(newQuote);
            _quoteDbContext.SaveChanges();
            
            return CreatedAtAction("GetQuoteById", new { id = result.Entity.QuoteId }, result.Entity);
        }

        // Update a quote.
        [HttpPost("/api/quotes/{quoteId}")]
        public IActionResult EditQuote([FromBody()] UpdateQuoteRequest updateQuoteRequest)
        {

            return Ok();
        }

        // Add a tag to a quote.

        // Delete a quote by Id.
    }
}
