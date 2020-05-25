using System.Collections.Generic;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class TestStatsViewModel
    {
        public Test Test { get; set; }
        public List<TestQuestionStatsViewModel> TestQuestions { get; set; }
        public int AverageResult { get; set; }
        public int TotalPassings { get; set; }
    }
}
