using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Property_Management_System.Models
{
    public class MessageViewModel
    {
        public string Content { get; set; }
        public string ReceiverId { get; set; }

        // List of users to populate the dropdown
        public List<SelectListItem> UserSelectList { get; set; }
    }
}
