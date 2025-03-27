using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.ViewModels.Common
{
    public class DataTableRequest
    {
        public bool IsActive { get; set; } = true;
        public int Draw { get; set; } // Số lần request của DataTables
        public int PageIndex => Start / Length; // Trang hiện tại
        public int PageSize => Length; // Số lượng item mỗi trang
        public int Start { get; set; } // Bản ghi bắt đầu
        public int Length { get; set; } // Số bản ghi mỗi trang
        public string SearchValue { get; set; } = ""; // Từ khóa tìm kiếm
        public string SortColumn { get; set; } = ""; // Cột cần sort
        public string SortDirection { get; set; } = "asc"; // Hướng sort (asc/desc)
    }
}