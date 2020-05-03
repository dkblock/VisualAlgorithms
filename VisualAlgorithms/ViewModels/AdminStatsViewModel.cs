using System.Collections.Generic;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class AdminStatsViewModel
    {
        public List<UserTest> UserTests { get; set; }
        public List<Algorithm> Algorithms { get; set; }
        public List<Test> Tests { get; set; }
        public int? AlgorithmId { get; set; }
        public int? TestId { get; set; }
    }
}
