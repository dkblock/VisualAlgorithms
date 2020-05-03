using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class UserAnswer
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Ответ")]
        [Required(ErrorMessage = "Введите ответ!")]
        public string Answer { get; set; }

        public bool IsCorrect { get; set; }

        public int TestQuestionId { get; set; }

        public string UserId { get; set; }

        public TestQuestion TestQuestion { get; set; }
        public ApplicationUser User { get; set; }
    }
}
