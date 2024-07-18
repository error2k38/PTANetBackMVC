using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Represents the <see cref="Mba"/> entity.
    /// </summary>
    public class Mba
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="Mba"/>.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MbaId { get; set; }

        /// <summary>
        /// Gets or sets the code for the <see cref="Mba"/>.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="Mba"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="MbaOptions"/> associated with the <see cref="Mba"/>.
        /// </summary>
        public Guid MbaOptionsId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MbaOptions"/> associated with the <see cref="Mba"/>.
        /// </summary>
        public MbaOptions MbaOptions { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Mba"/> class.
        /// This constructor is required by Entity Framework.
        /// </summary>
        public Mba() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mba"/> class with the specified code and name.
        /// </summary>
        /// <param name="code">The code for the <see cref="Mba"/>.</param>
        /// <param name="name">The name of the <see cref="Mba"/>.</param>
        public Mba(string code, string name, Guid mbaOptionsId)
        {
            Code = code;
            Name = name;
            MbaOptionsId = mbaOptionsId;
        }

        #endregion Constructors
    }

}
