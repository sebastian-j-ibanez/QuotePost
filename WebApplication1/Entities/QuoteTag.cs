namespace ProblemAnalysis3.Entities
{
    public class QuoteTag
    {
        public int TagId { get; set; }

        public int QuoteId { get; set; }

        public Tag? Tag { get; set; }

        public Quote? Quote { get; set; }
    }
}
