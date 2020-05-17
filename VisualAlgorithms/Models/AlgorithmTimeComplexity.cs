using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class AlgorithmTimeComplexity
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Сортировка")]
        public string SortingBestTime { get; set; }

        [Display(Name = "Сортировка")]
        public string SortingAverageTime { get; set; }

        [Display(Name = "Сортировка")]
        public string SortingWorstTime { get; set; }

        [Display(Name = "Поиск")]
        public string SearchingAverageTime { get; set; }

        [Display(Name = "Поиск")]
        public string SearchingWorstTime { get; set; }

        [Display(Name = "Вставка")]
        public string InsertionAverageTime { get; set; }

        [Display(Name = "Вставка")]
        public string InsertionWorstTime { get; set; }

        [Display(Name = "Удаление")]
        public string DeletionAverageTime { get; set; }

        [Display(Name = "Удаление")]
        public string DeletionWorstTime { get; set; }

        public int AlgorithmId { get; set; }

        public Algorithm Algorithm { get; set; }
    }
}
