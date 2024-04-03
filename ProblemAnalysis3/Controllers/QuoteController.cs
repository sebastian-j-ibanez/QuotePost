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
        
        // Get all tags.
        [HttpGet("/api/quotes/tags")]
        public IActionResult GetAllTags()
        {
            // Get all tags from DbContext.
            var tags = _quoteDbContext.Tags
                .OrderBy(t => t.TagId)
                .ToList();
            
            return Ok(tags);
        }

        // Find a quote by Id.
        [HttpGet("/api/quotes/{quoteId}")]
        public IActionResult GetQuoteById(int quoteId)
        {
            // Find a quote from the database that matches the route's quoteId.
            var quote = _quoteDbContext.Quotes
                .FirstOrDefault(q => q.QuoteId == quoteId);

            // Manually set all quote tags associated with the found quote,
            // EXCEPT for the quote to avoid circular reference.
            if (quote != null)
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


                return Ok(quote);
            }
            else
            {
                return NotFound();
            }
        }

        // Get a random quote.
        [HttpGet("/api/quotes/random")] 
        public IActionResult GetRandomQuote()
        {
            // Select all quotes from DbContext.
            var quotes = _quoteDbContext.Quotes
                .OrderBy(q => q.QuoteId)
                .ToList();
            
            // Get quote at random index in list.
            var random = new Random();
            int randomIndex = random.Next(quotes.Count());
            Quote quote = quotes[randomIndex];
            
            return Ok(quote);
        }

        // Add a quote.
        [HttpPost("/api/quotes")]
        public IActionResult AddQuote([FromBody()] QuoteContentResource quoteContentResource)
        {
            // Add quote to Db using info found in body.
            var result= _quoteDbContext.Quotes.Add(new Quote()
            {
                Content = quoteContentResource.Content,
                Author = quoteContentResource.Author,
                QuoteTags = new List<QuoteTag>(),
            });
            
            _quoteDbContext.SaveChanges();
            
            return CreatedAtAction("AddQuote", new { id = result.Entity.QuoteId }, result.Entity);
        }

        // Update a quote.
        [HttpPatch("/api/quotes/{quoteId}")]
        public IActionResult EditQuote(int quoteId, [FromBody()] QuoteContentResource quoteContentResource)
        {
            // Find quote with matching Id.
            Quote? quote = _quoteDbContext.Quotes.Find(quoteId);
            
            if (quote == null)
            {
                return NotFound();
            }
            
            // Update the quote fields.
            quote.Content = quoteContentResource.Content;
            quote.Author = quoteContentResource.Author;
            
            // Update quote and save changes.
            _quoteDbContext.Update(quote);
            _quoteDbContext.SaveChanges();

            return Ok();
        }

        // Add a tag to a quote.
        [HttpPost("/api/quotes/{quoteId}/tag/{tagId}")]
        public IActionResult AddTagToQuote(int quoteId, int tagId)
        {
            // Find quote with matching Id.
            var quote = _quoteDbContext.Quotes
                .Include(q => q.QuoteTags)
                .FirstOrDefault(q => q.QuoteId == quoteId);

            // Find tag by id.
            var tag = _quoteDbContext.Tags.Find(tagId);

            // Return error if either quote or tag are null.
            if (quote == null || tag == null)
            {
                return NotFound();
            }
            
            // Construct quoteTag.
            QuoteTag quoteTag = new QuoteTag()
            {
                QuoteId = quoteId,
                TagId = tagId,
            };
            
            // Initialize list if has not been already.
            // Add quoteTag to quote.
            quote.QuoteTags.Add(quoteTag);
            
            // Update quote in database.
            _quoteDbContext.Update(quote);
            _quoteDbContext.SaveChanges();
            
            return Ok();
        }
        
        // Increase the a quotes like count.
        [HttpPost("/api/quotes/{quoteId}/like")]
        public IActionResult IncreaseQuoteLikeCount (int quoteId)
        {
            // Get quote from DbContext.
            var quote = _quoteDbContext.Quotes.Find(quoteId);
            
            if (quote == null)
            {
                return NotFound();
            }

            // Increment like count, update quote, save DbContext.
            quote.LikeCount++;
            _quoteDbContext.Update(quote);
            _quoteDbContext.SaveChanges();

            return Ok();
        }

        // Decrease a quotes list count.
        [HttpPost("/api/quotes/{quoteId}/dislike")]
        public IActionResult DecreaseQuoteLikeCount(int quoteId)
        {
            // Get quote from DbContext.
            var quote = _quoteDbContext.Quotes.Find(quoteId);
            
            if (quote == null)
            {
                return NotFound();
            }

            // Increment like count, update quote, save DbContext.
            quote.LikeCount--;
            _quoteDbContext.Update(quote);
            _quoteDbContext.SaveChanges();

            return Ok();
        }

        // Set like count to 0.
        [HttpPut("/api/quotes/{quoteId}/like")]
        public IActionResult ResetLikeCount(int quoteId)
        {
            // Get quote from DbContext.
            var quote = _quoteDbContext.Quotes.Find(quoteId);

            if (quote == null)
            {
                return NotFound();
            }

            // Reset like count, update Db, and save changes.
            quote.LikeCount = 0;
            _quoteDbContext.Update(quote);
            _quoteDbContext.SaveChanges();

            return Ok();
        }

        // Delete a quote by Id.
        [HttpDelete("/api/quotes/{quoteId}")]
        public IActionResult DeleteQuoteById(int quoteId)
        {
            // Remove Http
            Quote? quote = _quoteDbContext.Quotes.Find(quoteId);

            if (quote == null)
            {
                return NotFound();
            }

            // Remove quote from DbContext.
            _quoteDbContext.Remove(quote);
            _quoteDbContext.SaveChanges();
            
            return Ok();
        }
    }
}
