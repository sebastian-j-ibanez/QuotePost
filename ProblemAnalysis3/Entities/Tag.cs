namespace ProblemAnalysis3.Entities
{
    public class Tag
    {
        public int TagId { get; set; }

        public string? TagName { get; set; }

        public List<QuoteTag> QuoteTags { get; set; } = new List<QuoteTag>();
    }
}
