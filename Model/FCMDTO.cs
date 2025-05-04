using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace Sandbox_Calc.Model
{
    public class FCMDTO
    { 
        public string Device { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }


    }

   
}
