using System.ComponentModel.DataAnnotations;

public class Login
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Mdp { get; set; }
}
