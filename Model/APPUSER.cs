using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("Appuser")]
public class Appuser
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string? Username { get; set; }

    [Required]
    [MaxLength(50)]
    public string? Password { get; set; }

    [MaxLength(100)]
    public string? Nama { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
