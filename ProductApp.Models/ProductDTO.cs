using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.Models
{
    public class ProductDTO
    {

        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }

        [Range(0, int.MaxValue)]
        public int AmountInStock { get; set; }
        public string Distributor { get; set; }
        //[Required]
        public string Creator { get; set; }
    }
}
