﻿
using Dentistry.ViewModels.System.Users;

namespace Dentistry.ViewModels.Catalog.Contacts
{
    public class ContactVm
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
        public string Message { set; get; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public UserVm ProcessBy {  set; get; }
    }
}