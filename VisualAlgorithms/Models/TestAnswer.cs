using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class TestAnswer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо дать ответ!")]
        public string Answer { get; set; }

        public int TestQuestionId { get; set; }

        public TestQuestion TestQuestion { get; set; }
    }
}
