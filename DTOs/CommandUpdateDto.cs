using System.ComponentModel.DataAnnotations;

namespace WebApiTest.DTOs
{
    public class CommandUpdateDto
    {
        //public int Id { get; set; } //not need

        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }

        [Required]
        public string Line { get; set; }

        [Required]
        public string Platform { get; set; }

    }
}