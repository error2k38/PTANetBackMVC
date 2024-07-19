using Domain.Entities;
using Mappers.DTOs;

namespace Mapper.Mappers
{
    /// <summary>
    /// Static class for mapping between <see cref="Mba"/> and <see cref="MbaOptions"/> objects.
    /// </summary>
    public static class MbaMapper
    {
        /// <summary>
        /// Maps a <see cref="MbaDto"/> object to a <see cref="Mba"/> object.
        /// </summary>
        /// <param name="mbaDto">The MbaDto object to map from.</param>
        /// <returns>A new <see cref="Mba"/> object with properties set from the <see cref="MbaDto"/>.</returns>
        public static Mba Map(this MbaDto mbaDto)
        {
            return new Mba(code: mbaDto.Code, name: mbaDto.Name, mbaOptionsId: mbaDto.MbaOptionsId)
            {
                MbaId = mbaDto.MbaId
            };
        }

        /// <summary>
        /// Maps a <see cref="MbaBaseDto"/> object to a <see cref="Mba"/> object.
        /// </summary>
        /// <param name="mbaDto">The MbaDto object to map from.</param>
        /// <returns>A new <see cref="Mba"/> object with properties set from the <see cref="MbaBaseDto"/>.</returns>
        public static Mba Map(this MbaBaseDto mbaDto, Guid mbaOptionsId)
        {
            return new Mba(code: mbaDto.Code, name: mbaDto.Name, mbaOptionsId: mbaOptionsId);
        }

        /// <summary>
        /// Maps a <see cref="Mba"/> object to a <see cref="MbaDto"/> object.
        /// </summary>
        /// <param name="mba">The <see cref="Mba"/> object to map from.</param>
        /// <returns>A new <see cref="MbaDto"/> object with properties set from the <see cref="Mba"/>.</returns>
        public static MbaDto Map(this Mba mba)
        {
            return new MbaDto
            {
                MbaId = mba.MbaId,
                MbaOptionsId = mba.MbaOptionsId,
                Code = mba.Code,
                Name = mba.Name
            };
        }

        /// <summary>
        /// Updates an existing <see cref="Mba"/> object with properties from a <see cref="MbaDto"/> object.
        /// </summary>
        /// <param name="target">The <see cref="Mba"/> object to update.</param>
        /// <param name="source">The <see cref="MbaDto"/> object to update from.</param>
        public static void Update(this Mba target, MbaDto source)
        {
            target.Code = source.Code;
            target.Name = source.Name;
        }
    }
}
