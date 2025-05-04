using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
 
public class TransaksiDTO
{ 

    public int id_category { get; set; }

    [Required] 
    public DateTime? tanggal { get; set; }

    [Required] 
    public decimal nominal { get; set; }

    [MaxLength(200)]
    [Required]
    public string? description { get; set; }

    
    public IFormFile? ImageFile { get; set; }

    public int status { get; set; }

    [MaxLength(20)]
    public string? user_created { get; set; }

    public DateTime? date_created { get; set; }
     
}

public class UpdateTransaksiDTO
{
    [Key]
    public int Id { get; set; }

    public int id_category { get; set; }

    [Required]
    public DateTime? tanggal { get; set; }

    [Required]
    public decimal nominal { get; set; }

    [MaxLength(200)]
    [Required]
    public string? description { get; set; }


    public IFormFile? ImageFile { get; set; }

    public int status { get; set; }

   

    [MaxLength(20)]
    public string? user_modified { get; set; }

    public DateTime? date_modified { get; set; }
}