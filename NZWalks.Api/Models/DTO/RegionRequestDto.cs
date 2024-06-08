using System.ComponentModel.DataAnnotations;

namespace NZWalks.Api.Models.DTO
{
    public class RegionRequestDto
    {
        [Required]
        [RegularExpression("^[A-Za-z]{3}$", ErrorMessage = "Minimum and Maximum of 3 Characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name should be a maximum of 100 Characters ")]       
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
