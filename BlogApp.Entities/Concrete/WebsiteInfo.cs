using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entities.Concrete
{
    public class WebsiteInfo
    {
        public string Title { get; set; }
        public string MenuTitle { get; set; }
        public string SeoAuthor { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTags { get; set; }
    }
}