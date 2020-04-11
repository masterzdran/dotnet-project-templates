using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Extended.WebApi.Models
{
    public sealed class City
    {
        [Required(ErrorMessage = "City Id field is mandatory.")]
        [Range(1,50,ErrorMessage = "The value must be between 1 and 50.")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "City Name field is mandatory.")]
        [MaxLength(16,ErrorMessage = "City Name maximum length is 16.")]
        [MinLength(length:4,ErrorMessage = "City Name minimum length is 4.")]
        public string Name { get; set; }    
    }
}