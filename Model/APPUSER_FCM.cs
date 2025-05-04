using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("APPUSER_FCM")]
public class APPUSER_FCM
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string? Username { get; set; }
     
    [MaxLength(200)]
    public string? token_fcm { get; set; }

   
}
