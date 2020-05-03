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
                        Name = "Бинарное дерево поиска",
                        ImageUrl = "binary-tree.png"
                    },
                    new Algorithm
                    {
                        Name = "AVL-дерево",
                        ImageUrl = "avl-tree.png"
                    },
                    new Algorithm
                    {
                        Name = "Красно-чёрное дерево",
                        ImageUrl = "red-black-tree.png"
                    }
                };

                await db.Algorithms.AddRangeAsync(algorithms);
                await db.SaveChangesAsync();
            }
        }
    }
}
