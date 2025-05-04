using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace Sandbox_Calc.Model
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
