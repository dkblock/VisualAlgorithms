using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.AppMiddleware
{
    public class TestsManager
    {
        private readonly ApplicationContext _db;

        public TestsManager(ApplicationContext db)
        {
            _db = db;
        }

        public async Task ProcessUserAnswer(UserAnswer userAnswer)
        {
            var testQuestion = await _db.TestQuestions.FindAsync(userAnswer.TestQuestionId);
            var correctAnswer = await _db.TestAnswers.FindAsync(testQuestion.CorrectAnswerId);

            userAnswer.IsCorrect = userAnswer.Answer.Equals(
                testQuestion.TestQuestionType == TestQuestionType.FreeAnswer
                ? correctAnswer.Answer
                : correctAnswer.Id.ToString());

            await _db.UserAnswers.AddAsync(userAnswer);
            await _db.SaveChangesAsync();
        }

        public async Task<UserTest> GetUserTestResult(UserAnswer userAnswer)
        {
            var userId = userAnswer.UserId;
            var question = await _db.TestQuestions
                .Include(q => q.Test)
                .ThenInclude(t => t.TestQuestions)
                .SingleAsync(q => q.Id == userAnswer.TestQuestionId);
            var test = question.Test;
            var testQuestions = test.TestQuestions.ToList();
            var userAnswers = await _db.UserAnswers
                .Where(ua => ua.UserId == userId && testQuestions.Select(tq => tq.Id).Contains(ua.TestQuestionId))
                .ToListAsync();

            var correctAnswersCount = userAnswers.Count(ua => ua.IsCorrect);
            var result = (double) correctAnswersCount / testQuestions.Count * 100;
            var userTest = new UserTest
            {
                CorrectAnswers = correctAnswersCount,
                TotalQuestions = testQuestions.Count,
                PassingTime = DateTime.Now,
                Result = (int) result,
                TestId = test.Id,
                UserId = userId
            };

            var userResult = await _db.UserTests.AddAsync(userTest);
            await _db.SaveChangesAsync();

            return userResult.Entity;
        }

        public async Task ClearUserTestResult(int testId, string userId)
        {
            var userTest = await _db.UserTests.FindAsync(testId, userId);

            if (userTest != null)
            {
                var userAnswers = await _db.UserAnswers
                    .Include(ua => ua.TestQuestion)
                    .Where(ua => ua.UserId == userId && ua.TestQuestion.TestId == testId)
                    .ToListAsync();

                _db.UserAnswers.RemoveRange(userAnswers);
                _db.UserTests.Remove(userTest);
                await _db.SaveChangesAsync();
            }
        }

        public void MixTestAnswers(List<TestAnswer> testAnswers)
        {
            var rnd = new Random();

            for (var i = testAnswers.Count - 1; i >= 1; i--)
            {
                var j = rnd.Next(i + 1);

                var tmp = testAnswers[j];
                testAnswers[j] = testAnswers[i];
                testAnswers[i] = tmp;
            }
        }
    }
}