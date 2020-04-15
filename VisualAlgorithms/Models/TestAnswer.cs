namespace VisualAlgorithms.Models
{
    public class TestAnswer
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public int TestQuestionId { get; set; }

        public TestQuestion TestQuestion { get; set; }
    }
}
