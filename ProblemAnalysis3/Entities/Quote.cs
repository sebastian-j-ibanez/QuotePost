using System.ComponentModel.DataAnnotations;

namespace ProblemAnalysis3.Entities
{
    public class Quote
    {
        public int QuoteId { get; set; }

        [Required(ErrorMessage = "Quote content is required")]
        public required string Content { get; set; }

        public string? Author { get; set; }

        public int LikeCount { get; set; } = 0;

        public List<QuoteTag> QuoteTags { get; set; } = new List<QuoteTag>();
    }
}
