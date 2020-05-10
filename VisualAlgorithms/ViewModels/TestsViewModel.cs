using System.Collections.Generic;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class TestsViewModel
    {
        public List<Test> Tests { get; set; }
        public List<UserTest> UserTests { get; set; }
        public List<Algorithm> Algorithms { get; set; }
        public List<string> TestTypes { get; set; }
        public int? AlgorithmId { get; set; }
        public int? TestsType { get; set; }
        public int? OrderBy { get; set; }
    }
}
