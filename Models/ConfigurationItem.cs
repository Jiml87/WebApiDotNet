using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class ConfigurationItem
    {
        [Required]
        [StringLength(55)]
        public string Key { get; set; }

        [Required]
        [StringLength(255)]
        public string Value { get; set; }
    }
}