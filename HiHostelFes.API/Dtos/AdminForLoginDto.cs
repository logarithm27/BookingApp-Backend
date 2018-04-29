using System.ComponentModel.DataAnnotations;
namespace HiHostelFes.API.Dtos
{
    public class AdminForLoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}