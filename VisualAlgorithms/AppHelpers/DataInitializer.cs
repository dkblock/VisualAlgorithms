using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.AppHelpers
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
                        Tag = "binary-tree",
                        ImageUrl = "binary-tree.png"
                    },
                    new Algorithm
                    {
                        Name = "AVL-дерево",
                        Tag = "avl-tree",
                        ImageUrl = "avl-tree.png"
                    },
                    new Algorithm
                    {
                        Name = "Красно-чёрное дерево",
                        Tag = "red-black-tree",
                        ImageUrl = "red-black-tree.png"
                    }
                };

                await db.Algorithms.AddRangeAsync(algorithms);
                await db.SaveChangesAsync();
            }
        }
    }
}
