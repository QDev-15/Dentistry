using Dentistry.ViewModels.Catalog;
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
        public bool IsActive {  set; get; }                                       
        public IFormFile ImageFile { get; set; }
        public int SortOrder { get; set; }
        public ImageVm Image { get; set; }
    }
}