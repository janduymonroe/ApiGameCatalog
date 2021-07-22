using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiGameCatalog.InputModel
{
    public class GameInputModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The game name must contain between 3 and 100 characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The producer name must contain between 3 and 100 characters")]
        public string Producer { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "The price must be a minimun 1 dollar and a maximun of 1000 dollars")]
        public double Price { get; set; }
    }
}
