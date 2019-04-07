using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerApi.Contracts.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
 
        public string FirstName { get; set; }
 
        public string LastName { get; set; }

        public string BirthDate { get; set; }
    }
}
