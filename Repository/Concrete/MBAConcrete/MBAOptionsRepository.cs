using Domain.Entities;
using Repository.Abstract.MBAbstract;

namespace Repository.Concrete
{
    /// <summary>
    /// Represents the ApplicationDBRepository class which implements the IMbaOptionsRepository interface.
    /// </summary>
    public partial class ApplicationDBRepository : IMbaOptionsRepository
    {
        /// <summary>
        /// Asynchronously creates a new <see cref="MbaOptions"/> entity with the specified country and country code.
        /// </summary>
        /// <param name="country">The country for the new <see cref="MbaOptions"/> entity.</param>
        /// <param name="countryCode">The country code for the new <see cref="MbaOptions"/> entity.</param>
        /// <returns>The newly created <see cref="MbaOptions"/> entity.</returns>
        public async Task<MbaOptions> CreateMbaOptionsAsync(string country, string countryCode)
        {
            var mbaOptions = new MbaOptions(country, countryCode);
            await AddAsync(mbaOptions);
            return mbaOptions;
        }

        /// <summary>
        /// Asynchronously creates a new <see cref="MbaOptions"/> entity with the specified country, country code, and a list of MBAs.
        /// </summary>
        /// <param name="country">The country for the new <see cref="MbaOptions"/> entity.</param>
        /// <param name="countryCode">The country code for the new <see cref="MbaOptions"/> entity.</param>
        /// <param name="mbas">The list of MBAs for the new <see cref="MbaOptions"/> entity.</param>
        /// <returns>The newly created <see cref="MbaOptions"/> entity.</returns>
        public async Task<MbaOptions> CreateMbaOptionsAsync(string country, string countryCode, IList<Mba> mbas)
        {
            var mbaOptions = new MbaOptions(country, countryCode)
            {
                Mbas = mbas
            };
            await AddAsync(mbaOptions);
            return mbaOptions;
        }

        /// <summary>
        /// Asynchronously adds a range of <see cref="MbaOptions"/> entities to the database.
        /// </summary>
        /// <param name="mbaOptions">The list of <see cref="MbaOptions"/> entities to add.</param>
        /// <returns>The list of added <see cref="MbaOptions"/> entities.</returns>
        public async Task<IList<MbaOptions>> AddRangeMbaOptionsAsync(IList<MbaOptions> mbaOptions)
        {
            await AddRangeAsync(mbaOptions);
            return mbaOptions;
        }

        /// <summary>
        /// Asynchronously deletes a specified <see cref="MbaOptions"/> entity from the database.
        /// </summary>
        /// <param name="mbaOptions">The <see cref="MbaOptions"/> entity to delete.</param>
        public async Task DeleteMbaOptionsAsync(MbaOptions mbaOptions)
        {
            var mbas = await GetMbaByMbaOptionsAsync(mbaOptions.MbaOptionsId);
            await DeleteRangeMbAsync(mbas);
            await DeleteAsync(mbaOptions);
        }

        /// <summary>
        /// Asynchronously deletes a range of <see cref="MbaOptions"/> entities from the database.
        /// </summary>
        /// <param name="mbaOptions">The list of <see cref="MbaOptions"/> entities to delete.</param>
        public async Task DeleteRangeMbaOptionsAsync(IList<MbaOptions> mbaOptions)
        {
            await DeleteRangeAsync(mbaOptions);
        }

        /// <summary>
        /// Asynchronously retrieves a <see cref="MbaOptions"/> entity by its ID.
        /// </summary>
        /// <param name="MbaOptionsId">The ID of the <see cref="MbaOptions"/> entity to retrieve.</param>
        /// <returns>The <see cref="MbaOptions"/> entity with the specified ID.</returns>
        public async Task<MbaOptions> GetMbaOptionsByIdAsync(Guid MbaOptionsId)
        {
            return await GetByIDAsync<MbaOptions>(MbaOptionsId);
        }

        /// <summary>
        /// Asynchronously retrieves all <see cref="MbaOptions"/> entities from the database.
        /// </summary>
        /// <returns>A list of all <see cref="MbaOptions"/> entities.</returns>
        public async Task<IList<MbaOptions>> GetMbasOptionsAsync()
        {
            return await GetAsync<MbaOptions>();
        }

        /// <summary>
        /// Asynchronously updates a specified <see cref="MbaOptions"/> entity in the database.
        /// </summary>
        /// <param name="mbaOptions">The <see cref="MbaOptions"/> entity to update.</param>
        public async Task UpdateMbaOptionsAsync(MbaOptions mbaOptions)
        {
            await UpdateAsync(mbaOptions);
        }

        /// <summary>
        /// Asynchronously updates a range of <see cref="MbaOptions"/> entities in the database.
        /// </summary>
        /// <param name="mbaOptions">The list of <see cref="MbaOptions"/> entities to update.</param>
        public async Task UpdateRangeMbaOptionsAsync(IList<MbaOptions> mbaOptions)
        {
            await UpdateAsync(mbaOptions);
        }
    }

}
