using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class TestQuestionStatsViewModel
    {
        public TestQuestion TestQuestion { get; set; }
        public int AverageResult { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
        public int TotalAnswers { get; set; }
    }
}
