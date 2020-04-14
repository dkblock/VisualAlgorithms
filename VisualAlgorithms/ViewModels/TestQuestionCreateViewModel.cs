using System.Collections.Generic;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class TestQuestionCreateViewModel
    {
        public TestQuestion TestQuestion { get; set; }
        public List<TestAnswer> TestAnswers { get; set; }
    }
}
