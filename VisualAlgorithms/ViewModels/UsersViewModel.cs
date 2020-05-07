using System.Collections.Generic;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class UsersViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public int? GroupId { get; set; }
        public List<Group> Groups { get; set; }
    }
}
