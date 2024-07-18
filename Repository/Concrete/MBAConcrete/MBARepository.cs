using Domain.Entities;
using Repository.Abstract.MBAbstract;

namespace Repository.Concrete
{
    /// <summary>
    /// Represents the ApplicationDBRepository class which implements the IMbaOptionsRepository interface.
    /// </summary>
    public partial class ApplicationDBRepository : IMbaRepository
    {
        /// <summary>
        /// Asynchronously creates a new <see cref="Mba"/> entity with the specified code and name.
        /// </summary>
        /// <param name="code">The code for the new <see cref="Mba"/> entity.</param>
        /// <param name="name">The name for the new <see cref="Mba"/> entity.</param>
        /// <returns>The newly created <see cref="Mba"/> entity.</returns>
        public async Task<Mba> CreateMbAsync(string code, string name, Guid mbaOptionsId)
        {
            var mba = new Mba(code, name, mbaOptionsId);
            await AddAsync(mba);
            return mba;
        }

        /// <summary>
        /// Asynchronously adds a range of <see cref="Mba"/> entities to the database.
        /// </summary>
        /// <param name="mba">The list of <see cref="Mba"/> entities to add.</param>
        /// <returns>The list of added <see cref="Mba"/> entities.</returns>
        public async Task<IList<Mba>> AddRangeMbAsync(IList<Mba> mba)
        {
            await AddRangeAsync(mba);
            return mba;
        }

        /// <summary>
        /// Asynchronously deletes a specified <see cref="Mba"/> entity from the database.
        /// </summary>
        /// <param name="mba">The <see cref="Mba"/> entity to delete.</param>
        public async Task DeleteMbAsync(Mba mba)
        {
            await DeleteAsync(mba);
        }

        /// <summary>
        /// Asynchronously deletes a range of <see cref="Mba"/> entities from the database.
        /// </summary>
        /// <param name="mba">The list of <see cref="Mba"/> entities to delete.</param>
        public async Task DeleteRangeMbAsync(IList<Mba> mba)
        {
            await DeleteRangeAsync(mba);
        }

        /// <summary>
        /// Asynchronously retrieves a <see cref="Mba"/> entity by its ID.
        /// </summary>
        /// <param name="mbaId">The ID of the <see cref="Mba"/> entity to retrieve.</param>
        /// <returns>The <see cref="Mba"/> entity with the specified ID.</returns>
        public async Task<Mba> GetMbaByIdAsync(Guid mbaId)
        {
            return await GetByIDAsync<Mba>(mbaId);
        }

        /// <summary>
        /// Asynchronously retrieves a list of <see cref="Mba"/> entities that are associated with the specified MbaOptions entity.
        /// </summary>
        /// <param name="mbaOptionsId">The ID of the MbaOptions entity to retrieve the associated <see cref="Mba"/> entities for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Mba"/> entities associated with the specified <see cref="MbaOptions"/> entity.</returns>
        public async Task<IList<Mba>> GetMbaByMbaOptionsAsync(Guid mbaOptionsId)
        {
            return await GetBasicAsync<Mba>(x => x.MbaOptionsId == mbaOptionsId);
        }

        /// <summary>
        /// Asynchronously retrieves a list of <see cref="Mba"/> entities.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Mba"/> entities.</returns>
        public async Task<IList<Mba>> GetAllMbaAsync()
        {
            return await GetBasicAsync<Mba>();
        }

        /// <summary>
        /// Asynchronously updates a specified <see cref="Mba"/> entity in the database.
        /// </summary>
        /// <param name="mba">The <see cref="Mba"/> entity to update.</param>
        public async Task UpdateMbAsync(Mba mba)
        {
            await UpdateAsync(mba);
        }

        /// <summary>
        /// Asynchronously updates a range of <see cref="Mba"/> entities in the database.
        /// </summary>
        /// <param name="mba">The list of <see cref="Mba"/> entities to update.</param>
        public async Task UpdateRangeMbAsync(IList<Mba> mba)
        {
            await UpdateAsync(mba);
        }
    }

}
