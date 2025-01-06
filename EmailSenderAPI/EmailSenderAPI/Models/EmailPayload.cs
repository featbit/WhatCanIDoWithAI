namespace EmailSenderAPI.Models
{
    public class EmailTo
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }

    public class EmailPayload
    {
        public required List<EmailTo> To { get; set; } = new List<EmailTo>();
        public int TemplateId { get; set; }
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
    }
}
