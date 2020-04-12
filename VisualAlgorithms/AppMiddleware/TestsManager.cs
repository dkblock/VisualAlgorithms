using System;
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

        public async Task AddUserAnswer(UserAnswer userAnswer)
        {
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

            var count = 0;

            foreach (var answer in userAnswers)
            {
                var currentAnswer = answer.Answer;
                var questionAnswer = testQuestions.Single(q => q.Id == answer.TestQuestionId).Answer;

                if (currentAnswer == questionAnswer)
                    count++;
            }

            var result = (double) count / testQuestions.Count * 100;
            var userTest = new UserTest
            {
                CorrectAnswers = count,
                TotalQuestions = testQuestions.Count,
                PassingTime = DateTime.Now,
                Result = (int)result,
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
    }
}