using System.ComponentModel.DataAnnotations.Schema;

namespace KnowledgeBase.DataModels.ReportGenerator
{
    public class Specification
    {
        public string Title { get; set; }
        public string Definition { get; set; }
        
        //[NotMapped]
        public List<Feature> Features { get; set; }
    }

    public class Feature
    {
        public string Description { get; set; }
        public string Name { get; set; }
        
        //[NotMapped]
        public List<Module> Modules { get; set; }
        
        public string MenuItem { get; set; }
    }

    public class Module
    {
        public string ShortDescription { get; set; }
        public string DetailDescription { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
