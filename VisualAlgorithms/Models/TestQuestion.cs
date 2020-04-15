using System.Collections.Generic;

namespace VisualAlgorithms.Models
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public TestQuestionType TestQuestionType { get; set; }
        public int CorrectAnswerId { get; set; }
        public bool IsLastQuestion { get; set; }
        public int TestId { get; set; }

        public Test Test { get; set; }
        public List<TestAnswer> TestAnswers { get; set; }
        public List<UserAnswer> UserAnswers { get; set; }
    }

    public enum TestQuestionType
    {
        SelectAnswer = 0,
        FreeAnswer = 1
    }
}