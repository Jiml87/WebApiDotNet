using System.ComponentModel.DataAnnotations;

public class CreateConfigurationItemDto
{
    [Required]
    [StringLength(55)]
    public required string Key { get; set; }

    [Required]
    public required object Value { get; set; }
}