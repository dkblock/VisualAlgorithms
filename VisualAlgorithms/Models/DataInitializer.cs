using System.Collections.Generic;
using System.Linq;

namespace VisualAlgorithms.Models
{
    public static class DataInitializer
    {
        public static void Initialize(ApplicationContext db)
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

                db.Algorithms.AddRange(algorithms);
                db.SaveChanges();
            }
        }
    }
}
