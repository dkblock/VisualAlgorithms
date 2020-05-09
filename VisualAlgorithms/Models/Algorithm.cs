using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class Algorithm
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Алгоритм")]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int AlgorithmTimeComplexityId { get; set; }

        public AlgorithmTimeComplexity AlgorithmTimeComplexity { get; set; }
        public IEnumerable<Test> Tests { get; set; }
    }
}
