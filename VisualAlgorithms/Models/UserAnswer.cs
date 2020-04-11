using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }

        [Display(Name = "Ответ")]
        public int Answer { get; set; }

        public int TestQuestionId { get; set; }
        public string UserId { get; set; }

        public TestQuestion TestQuestion { get; set; }
        public ApplicationUser User { get; set; }
    }
}
