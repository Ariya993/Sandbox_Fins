using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("Category")]
public class Category
{
    [Key]
    public int id { get; set; }

    [Required]
    [MaxLength(50)]
    public string? category { get; set; }
     
    [MaxLength(50)]
    public string? user_created { get; set; }

    public int status { get; set; }

    public DateTime? date_created { get; set; }

    [MaxLength(50)]
    public string? user_modified { get; set; }

    public DateTime? date_modified { get; set; }
}
