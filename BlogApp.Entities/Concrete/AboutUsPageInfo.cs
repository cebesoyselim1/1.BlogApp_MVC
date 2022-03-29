using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entities.Concrete
{
    public class AboutUsPageInfo
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTags { get; set; }
        public string SeoAuthor { get; set; }
    }
}