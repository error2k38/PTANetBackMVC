using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mappers.DTOs
{
    /// <summary>
    /// Base DTO for MbaOptions entity.
    /// </summary>
    public record MbaOptionsBaseDto
    {
        /// <summary>
        /// Country required for the MbaOptions entity.
        /// Must be between 4 and 50 characters.
        /// </summary>
        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "The value must be between 4 and 50 characters long.")]
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// CountryCode required for the MbaOptions entity.
        /// Must be between 5 and 16 characters.
        /// </summary>
        [Required(ErrorMessage = "CountryCode is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The value must be between 1 and 50 characters long.")]
        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
    }

    /// <summary>
    /// DTO necessary for the creation of an <see cref="MbaOptions"/> entity.
    /// </summary>
    public record MbaOptionsCreateDto : MbaOptionsBaseDto
    {
        // No additional properties
    }

    public record MbaOptionsDto : MbaOptionsBaseDto
    {
        /// <summary>
        /// Required MbaOptions ID.
        /// </summary>
        [Required(ErrorMessage = "Mba options Id is required.")]
        [JsonPropertyName("mbaOptionsId")]
        public Guid MbaOptionsId { get; set; }
    }

    /// <summary>
    /// Necessary data for communication with the external service.
    /// </summary>
    public record MbaOptionsSerializableDto : MbaOptionsBaseDto
    {
        /// <summary>
        /// List of MbaDto. It is never null but can be empty.
        /// </summary>
        [JsonPropertyName("mbas")]
        public IList<MbaBaseDto> Mbas { get; set; }
    }
}
