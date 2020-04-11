using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.AppMiddleware
{
    public static class DataInitializer
    {
        public static async Task Initialize(ApplicationContext db)
        {
            if (!db.Algorithms.Any())
            {
                var algorithms = new List<Algorithm>
                {
                    new Algorithm
                    {
                        Name = "Бинарное дерево поиска"
                    },
                    new Algorithm
                    {
                        Name = "AVL-дерево"
                    },
                    new Algorithm
                    {
                        Name = "Красно-чёрное дерево"
                    }
                };

                await db.Algorithms.AddRangeAsync(algorithms);
                await db.SaveChangesAsync();
            }
        }
    }
}
