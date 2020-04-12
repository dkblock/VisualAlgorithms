using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class Algorithm
    {
        public int Id { get; set; }

        [Display(Name = "Структура данных")]
        public string Name { get; set; }

        public IEnumerable<Test> Tests { get; set; }
    }
}
