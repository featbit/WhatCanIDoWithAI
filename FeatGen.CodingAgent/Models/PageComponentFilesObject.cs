using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatGen.CodingAgent.Models
{
    public class PageComponentFilesObject
    {
        public PCFsMainPage main_page_description { get; set; }
        public List<PCFsComponent> components { get; set; }
    }

    public class PCFsMainPage
    {
        public string id { get; set; }
        public string role { get; set; }
        public List<string> features_n_functionalities_description { get; set; }
        public string design { get; set; }
        public List<PCFsBehavior> behaviors_direction { get; set; }
    }
    public class PCFsComponent
    {
        public string component_name { get; set; }
        public string component_id { get; set; }
        public string component_file_name { get; set; }
        public string direction { get; set; }
        public string reason { get; set; }
        public string role { get; set; }
        public string features_n_functionalities_description { get; set; }
        public List<PCFsBehavior> behaviors_direction { get; set; }
    }

    public class PCFsBehavior
    {
        public string component_id { get; set; }
        public string direction { get; set; }
        public string action { get; set; }
        public string reason { get; set; }
    }
}
