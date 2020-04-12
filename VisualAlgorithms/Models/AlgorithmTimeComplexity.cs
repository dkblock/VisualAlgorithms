using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class AlgorithmTimeComplexity
    {
        public int Id { get; set; }

        [Display(Name = "Сортировка (в среднем)")]
        public string SortingAverageTime { get; set; }

        [Display(Name = "Сортировка (в худшем)")]
        public string SortingWorstTime { get; set; }

        [Display(Name = "Индексация (в среднем)")]
        public string IndexingAverageTime { get; set; }

        [Display(Name = "Индексация (в худшем)")]
        public string IndexingWorstTime { get; set; }

        [Display(Name = "Поиск (в среднем)")]
        public string SearchingAverageTime { get; set; }

        [Display(Name = "Поиск (в худшем)")]
        public string SearchingWorstTime { get; set; }

        [Display(Name = "Вставка (в среднем)")]
        public string InsertionAverageTime { get; set; }

        [Display(Name = "Вставка (в худшем)")]
        public string InsertionWorstTime { get; set; }

        [Display(Name = "Удаление (в среднем)")]
        public string DeletionAverageTime { get; set; }

        [Display(Name = "Удаление (в худшем)")]
        public string DeletionWorstTime { get; set; }

        public int AlgorithmId { get; set; }

        public Algorithm Algorithm { get; set; }
    }
}
