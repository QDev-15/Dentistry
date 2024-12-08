using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.ViewModels.Utilities.Slides
{
    public class SlideVm
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Url { set; get; }
        public bool Active {  set; get; }                                       
        public int? ImageId { set; get; }
        public IFormFile Image { get; set; }
        public int SortOrder { get; set; }
    }
}