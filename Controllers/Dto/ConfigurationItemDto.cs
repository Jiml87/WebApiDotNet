using System.ComponentModel.DataAnnotations;

public class ConfigurationItemDto
{
    [Required]
    [StringLength(55)]
    public required string Key { get; set; }

    [Required]
    public required object Value { get; set; }
}