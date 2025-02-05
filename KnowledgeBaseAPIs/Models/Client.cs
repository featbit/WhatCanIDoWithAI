namespace KnowledgeBaseAPIs.Models
{
    public class Client
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
    }

    public class ClientParam : Client { }
}
