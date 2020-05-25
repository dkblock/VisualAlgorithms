using System.Collections.Generic;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.ViewModels
{
    public class UserTestStatsViewModel
    {
        public UserTest UserTest { get; set; }
        public ApplicationUser User { get; set; }
        public List<UserAnswer> UserAnswers { get; set; }
        public List<UserAnswer> AllUserAnswers { get; set; }
        public int? AnswersType { get; set; }
        public List<string> AnswerTypes { get; set; }
    }
}
