﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VisualAlgorithms.Models
{
    public class Test
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        public int AlgorithmId { get; set; }

        public Algorithm Algorithm { get; set; }
        public IEnumerable<TestQuestion> Questions { get; set; }
    }
}
