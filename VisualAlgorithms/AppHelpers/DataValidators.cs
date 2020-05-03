using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using VisualAlgorithms.Models;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms.AppMiddleware
{
    public class TestQuestionViewModelValidator : AbstractValidator<TestQuestionViewModel>
    {
        public TestQuestionViewModelValidator()
        {
            RuleFor(tq => tq.TestQuestion.Question)
                .NotEmpty()
                .WithMessage("Введите вопрос!");

            RuleForEach(tq => tq.TestAnswers).SetValidator(tq => new TestAnswerValidator(tq.TestAnswers));
        }
    }

    public class TestAnswerValidator : AbstractValidator<TestAnswer>
    {
        private readonly IEnumerable<TestAnswer> _testAnswers;

        public TestAnswerValidator(IEnumerable<TestAnswer> testAnswers)
        {
            _testAnswers = testAnswers;

            RuleFor(ta => ta.Answer)
                .NotEmpty()
                .WithMessage("Введите ответ!");

            RuleFor(ta => ta.Answer)
                .Must(BeUniqueAnswer)
                .WithMessage("Ответы не должны совпадать!");
        }

        private bool BeUniqueAnswer(string answer)
        {
            return _testAnswers.Count(ta => ta.Answer == answer) == 1;
        }
    }
}
