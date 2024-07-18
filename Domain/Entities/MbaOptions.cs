using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Represents the <see cref="MbaOptions"/> entity.
    /// </summary>
    public class MbaOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="MbaOptions"/>.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MbaOptionsId { get; set; }

        /// <summary>
        /// Gets or sets the country for the <see cref="MbaOptions"/>.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the country code for the <see cref="MbaOptions"/>.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Mba"/> entities associated with the <see cref="MbaOptions"/>.
        /// </summary>
        public IList<Mba> Mbas { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MbaOptions"/> class.
        /// This constructor is required by Entity Framework.
        /// </summary>
        public MbaOptions()
        {
            Mbas = new List<Mba>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MbaOptions"/> class with the specified country and country code.
        /// </summary>
        /// <param name="country">The country for the <see cref="MbaOptions"/>.</param>
        /// <param name="countryCode">The country code for the <see cref="MbaOptions"/>.</param>
        public MbaOptions(string country, string countryCode)
        {
            Country = country;
            CountryCode = countryCode;
        }

        #endregion Constructors
    }

}
