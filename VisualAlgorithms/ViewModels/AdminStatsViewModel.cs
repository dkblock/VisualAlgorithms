using System.Collections.Generic;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class AdminStatsViewModel
    {
        public List<UserTest> UserTests { get; set; }
        public List<Algorithm> Algorithms { get; set; }
        public List<Test> Tests { get; set; }
        public List<Group> Groups { get; set; }
        public int? AlgorithmId { get; set; }
        public int? TestId { get; set; }
        public int? GroupId { get; set; }
        public int? OrderBy { get; set; }
        public int DeletedTestId { get; set; }
        public string DeletedUserId { get; set; }
    }
}
