using Mappers.DTOs;
using Domain.Entities;

namespace Mapper.Mappers
{
    /// <summary>
    /// Static class for mapping between <see cref="MbaOptions"/> and <see cref="MbaOptionsDto"/> objects.
    /// </summary>
    public static class MbaOptionsMapper
    {
        /// <summary>
        /// Maps a <see cref="MbaOptionsDto"/> object to a <see cref="MbaOptions"/> object.
        /// </summary>
        /// <param name="mbaOptionsDto">The MbaOptionsDto object to map from.</param>
        /// <returns>A new <see cref="MbaOptions"/> object with properties set from the <see cref="MbaOptionsDto"/>.</returns>
        public static MbaOptions Map(this MbaOptionsDto mbaOptionsDto)
        {
            return new MbaOptions(country: mbaOptionsDto.Country, countryCode: mbaOptionsDto.CountryCode)
            {
                MbaOptionsId = mbaOptionsDto.MbaOptionsId
            };
        }

        /// <summary>
        /// Maps a <see cref="MbaOptions"/> object to a <see cref="MbaOptionsDto"/> object.
        /// </summary>
        /// <param name="mbaOptions">The <see cref="MbaOptions"/> object to map from.</param>
        /// <returns>A new <see cref="MbaOptionsDto"/> object with properties set from the <see cref="MbaOptions"/>.</returns>
        public static MbaOptionsDto Map(this MbaOptions mbaOptions)
        {
            return new MbaOptionsDto
            {
                MbaOptionsId = mbaOptions.MbaOptionsId,
                Country = mbaOptions.Country,
                CountryCode = mbaOptions.CountryCode
            };
        }

        /// <summary>
        /// Maps a <see cref="MbaOptionsSerializableDto"/> object to a <see cref="MbaOptions"/> object.
        /// </summary>
        /// <param name="mbaOptionsSerializableDto"> The <see cref="MbaOptionsSerializableDto"/> object to map from</param>
        /// <returns>A new <see cref="MbaOptions"/> object with properties set from the <see cref="MbaOptionsSerializableDto"/>.</returns>
        public static MbaOptions Map(this MbaOptionsSerializableDto mbaOptionsSerializableDto)
        {
            return new MbaOptions(country: mbaOptionsSerializableDto.Country, countryCode: mbaOptionsSerializableDto.CountryCode);
        }

        /// <summary>
        /// Updates an existing <see cref="MbaOptions"/> object with properties from a <see cref="MbaOptionsDto"/> object.
        /// </summary>
        /// <param name="target">The <see cref="MbaOptions"/> object to update.</param>
        /// <param name="source">The <see cref="MbaOptionsDto"/> object to update from.</param>
        public static void Update(this MbaOptions target, MbaOptionsDto source)
        {
            target.Country = source.Country;
            target.CountryCode = source.CountryCode;
        }
    }
}
