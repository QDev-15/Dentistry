using Dentistry.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Articles
{
    public class ArticleVm
    {
        public int Id { get; set; }
        public string Alias { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<ImageVm> Images { get; set; } = new List<ImageVm>();
        public UserVm CreatedBy { get; set; }

    }
}
