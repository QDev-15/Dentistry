using Dentistry.ViewModels.Catalog;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dentistry.ViewModels.Catalog.Slide
{
    public class SlideVm
    {
        public int Id { set; get; }

        [Required]
        [MaxLength(200)]
        public string Name { set; get; }
        public string SubName { set; get; }
        [Required]
        [MaxLength(200)]
        public string Description { set; get; }
        public string Url { set; get; }
        public Guid UserId { set; get; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public IFormFile ImageFile { get; set; }
        public ImageVm Image { get; set; }
    }
}