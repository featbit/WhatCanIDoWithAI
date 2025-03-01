using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase.SpecGenerator.Models
{
    public class Specification
    {
        public string Title { get; set; }
        public string Definition { get; set; }
        public List<Feature> Features { get; set; }
    }

    public class Feature
    {
        public string Description { get; set; }
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
