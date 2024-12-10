using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.Slide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data
{
    public static class ViewModelExtensions
    {
        public static SlideVm ReturnViewModel(this Slide item)
        {
            if (item == null)
            {
                return new SlideVm();
            }
            var slideVm = new SlideVm()
            {
                Id = item.Id,
                Name = item.Name,
                SubName = item.SubName,
                Description = item.Description,
                Image = item.Image == null ? new ImageVm() : new ImageVm()
                {
                    Id = item.Image.Id,
                    FileName = item.Image.FileName,
                    FileSize = item.Image.FileSize,
                    Path = item.Image.Path,
                    Type = item.Image.Type
                },
                Url = item.Url ?? "",
                UserId = item.UserId,
                IsActive = item.IsActive,
                SortOrder = item.SortOrder,
                UpdatedDate = item.UpdatedDate,
                CreatedDate = item.CreatedDate
            };
            return slideVm;
        }
    }
}
