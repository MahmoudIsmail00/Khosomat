using Khosomat.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khosomat.Entities.ViewModels
{
    public class ShoppingCart
    {
        public Product Product { get; set; }

        [Range(0, 100,ErrorMessage ="You must enter a value between 1 and 100")]
        public int Count { get; set; }
    }
}
