using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.GeneratorDB.Entities
{
    public class HomeArticle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int BackgroundImageId { get; set; }
        public Image BackgroundImage { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public ThemeStyle ThemeStyle { get; set; }

    }
}
