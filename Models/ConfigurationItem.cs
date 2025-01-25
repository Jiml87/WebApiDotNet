using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class ConfigurationItem
    {
        // [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // public int Id { get; set; } 

        [Required]
        [StringLength(55)]
        public required string Key { get; set; }

        [Required]
        [StringLength(255)]
        public required string Value { get; set; }
    }
}