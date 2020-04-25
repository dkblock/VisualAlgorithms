using System.Collections.Generic;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class TestQuestionViewModel
    {
        public TestQuestion TestQuestion { get; set; }
        public List<TestAnswer> TestAnswers { get; set; }
        public string Image { get; set; }
        public string UserId { get; set; }
    }
}
