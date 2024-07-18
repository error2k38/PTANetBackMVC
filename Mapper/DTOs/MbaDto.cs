using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mappers.DTOs
{
    /// <summary>
    /// Base DTO for <see cref="Mba"/> entity.
    /// </summary>
    public record MbaBaseDto
    {
        /// <summary>
        /// Code required for the <see cref="Mba"/> entity.
        /// Must be between 5 and 16 characters.
        /// </summary>
        [Required(ErrorMessage = "Code is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The value must be between 1 and 50 characters long.")]
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Name required for the <see cref="Mba"/> entity.
        /// Must be between 1 and 256 characters.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "The value must be between 1 and 256 characters long.")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// DTO necessary for the creation of an <see cref="Mba"/> entity.
    /// </summary>
    public record MbaCreationDto : MbaBaseDto
    {
        /// <summary>
        /// Mba Options ID required for the <see cref="MbaOptions"/> entity.
        /// </summary>
        [Required(ErrorMessage = "Mba Options ID is required")]
        [JsonPropertyName("mbaOptionsId")]
        public Guid MbaOptionsId { get; set; }
    }

    /// <summary>
    /// DTO associated with the <see cref="Mba"/> entity.
    /// </summary>
    public record MbaDto : MbaCreationDto
    {
        /// <summary>
        /// Mba ID required.
        /// </summary>
        [Required(ErrorMessage = "Mba ID is required.")]
        [JsonPropertyName("mbaId")]
        public Guid MbaId { get; set; }
    }

}
