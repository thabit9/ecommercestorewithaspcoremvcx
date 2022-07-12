using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ecommercestorewithaspcoremvc.Models
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            InverseParents = new HashSet<Category>();
        }
        public int Id { get; set; } 
        public string Name { get; set; }  
        public bool Status { get; set; }
        public int? ParentId { get; set; }

        public virtual Category parent { get; set;}
        public virtual ICollection<Category> InverseParents { get; set;}

    }
}