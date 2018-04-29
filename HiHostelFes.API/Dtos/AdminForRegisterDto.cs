using System.ComponentModel.DataAnnotations;

namespace HiHostelFes.API.Dtos
{
    public class AdminForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(10,MinimumLength=4,ErrorMessage="You should enter a password between 4 and 10 characters")]
        public string Password { get; set; }
    }
}