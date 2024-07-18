using Domain.Entities;

namespace Repository.Abstract.MBAbstract
{
    /// <summary>
    /// Defines the interface for the Mba repository.
    /// </summary>
    public interface IMbaRepository : IRepository
    {
        /// <summary>
        /// Asynchronously creates a new <see cref="Mba"/> entity with the specified code, name, and MbaOptionsId.
        /// </summary>
        /// <param name="code">The code for the new <see cref="Mba"/> entity.</param>
        /// <param name="name">The name for the new <see cref="Mba"/> entity.</param>
        /// <param name="MbaOptionsId">The MbaOptionsId for the new <see cref="Mba"/> entity.</param>
        /// <returns>The newly created <see cref="Mba"/> entity.</returns>
        Task<Mba> CreateMbAsync(string code, string name, Guid MbaOptionsId);

        /// <summary>
        /// Asynchronously adds a range of <see cref="Mba"/> entities to the database.
        /// </summary>
        /// <param name="mba">The list of <see cref="Mba"/> entities to add.</param>
        /// <returns>The list of added <see cref="Mba"/> entities.</returns>
        Task<IList<Mba>> AddRangeMbAsync(IList<Mba> mba);

        /// <summary>
        /// Asynchronously deletes a specified <see cref="Mba"/> entity from the database.
        /// </summary>
        /// <param name="mba">The <see cref="Mba"/> entity to delete.</param>
        Task DeleteMbAsync(Mba mba);

        /// <summary>
        /// Asynchronously deletes a range of <see cref="Mba"/> entities from the database.
        /// </summary>
        /// <param name="mba">The list of <see cref="Mba"/> entities to delete.</param>
        Task DeleteRangeMbAsync(IList<Mba> mba);

        /// <summary>
        /// Asynchronously retrieves a <see cref="Mba"/> entity by its ID.
        /// </summary>
        /// <param name="MbaId">The ID of the <see cref="Mba"/> entity to retrieve.</param>
        /// <returns>The <see cref="Mba"/> entity with the specified ID.</returns>
        Task<Mba> GetMbaByIdAsync(Guid MbaId);

        /// <summary>
        /// Asynchronously retrieves a list of <see cref="Mba"/> entities.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Mba"/> entities.</returns>
        Task<IList<Mba>> GetAllMbaAsync();

        /// <summary>
        /// Asynchronously retrieves a list of <see cref="Mba"/> entities that are associated with the specified MbaOptions entity.
        /// </summary>
        /// <param name="mbaOptionsId">The ID of the MbaOptions entity to retrieve the associated <see cref="Mba"/> entities for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Mba"/> entities associated with the specified <see cref="MbaOptions"/> entity.</returns>
        Task<IList<Mba>> GetMbaByMbaOptionsAsync(Guid mbaOptionsId);

        /// <summary>
        /// Asynchronously updates a specified <see cref="Mba"/> entity in the database.
        /// </summary>
        /// <param name="mba">The <see cref="Mba"/> entity to update.</param>
        Task UpdateMbAsync(Mba mba);

        /// <summary>
        /// Asynchronously updates a range of <see cref="Mba"/> entities in the database.
        /// </summary>
        /// <param name="mba">The list of <see cref="Mba"/> entities to update.</param>
        Task UpdateRangeMbAsync(IList<Mba> mba);
    }

}
