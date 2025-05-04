using System;
using System.ComponentModel.DataAnnotations;

public class RequestLog
{
    [Key]
    public int Id { get; set; }
    public string Method { get; set; }  // GET, POST, PUT, DELETE
    public string Path { get; set; }    // Endpoint API
    public string UserIP { get; set; }  // IP Pengguna
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
