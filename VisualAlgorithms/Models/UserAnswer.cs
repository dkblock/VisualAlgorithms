namespace VisualAlgorithms.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public int TestQuestionId { get; set; }
        public string UserId { get; set; }

        public TestQuestion TestQuestion { get; set; }
        public ApplicationUser User { get; set; }
    }
}
