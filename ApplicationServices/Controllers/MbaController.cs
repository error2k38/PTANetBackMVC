using Domain.Entities;
using Mapper.Mappers;
using Mappers.DTOs;
using Microsoft.AspNetCore.Mvc;
using Repository.Abstract.MBAbstract;

namespace PTNetBackMVC.Controllers
{
    // Define the route and output type for the controller
    [Route("api/[controller]"), Produces(typeof(JsonResult))]
    [ApiController]
    public class MbaController : ControllerBase
    {
        // Define a logger and a repository as private readonly fields
        private readonly ILogger<MbaController> _logger;
        private readonly IMbaRepository _mbaRepository;

        // Initialize the logger and the repository in the constructor
        public MbaController(ILogger<MbaController> logger, IMbaRepository mbaRepository)
        {
            _logger = logger;
            _mbaRepository = mbaRepository;
        }

        #region GetActions

        /// <summary>
        /// Gets the MBAs by MBA options.
        /// </summary>
        /// <param name="id">The identifier of the MBA option.</param>
        /// <returns>A <see cref="Task{ActionResult{IEnumerable{MbaDto}}}"/> object containing the asynchronous operation
        /// that returns the list of MBAs of an associated option.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status400BadRequest"/> if it could not be completed.
        [HttpGet(nameof(GetMbasByMbaOptions) + "/{id}")]
        public async Task<ActionResult<IEnumerable<MbaDto>>> GetMbasByMbaOptions([FromRoute] Guid id)
        {
            // Begin a database transaction
            _mbaRepository.BeginTransaction();

            var mbas = await _mbaRepository.GetMbaByMbaOptionsAsync(id);

            if (mbas is null)
            {
                var msg = "Has error occurred";
                _logger.LogError($"{nameof(GetMbasByMbaOptions)} -> {msg}");
                _mbaRepository.RollbackTransaction();
                return BadRequest(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var mbasDto = mbas.Select(mba => mba.Map());

            _mbaRepository.CommitTransaction();
            return Ok(mbasDto);
        }

        /// <summary>
        /// Gets the MBA by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the MBA.</param>
        /// <returns>A <see cref="Task{ActionResult{MbaDto}}}"/> object containing the asynchronous operation
        /// that returns the MBA of the given identifier.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status404NotFound"/> if the MBA could not be found.
        [HttpGet(nameof(GetMbaById) + "/{id}")]
        public async Task<ActionResult<MbaDto>> GetMbaById([FromRoute] Guid id)
        {
            // Begin a database transaction
            _mbaRepository.BeginTransaction();

            var mba = await _mbaRepository.GetMbaByIdAsync(id);

            if (mba is null)
            {
                var msg = "Mbas not found";
                _logger.LogError($" {nameof(GetMbaById)} -> {msg}");
                _mbaRepository.RollbackTransaction();
                return NotFound(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var mbaDto = mba.Map();
            _mbaRepository.CommitTransaction();
            return Ok(mbaDto);
        }

        #endregion GetActions

        #region Post Actions

        /// <summary>
        /// Creates a new MBA.
        /// </summary>
        /// <param name="request">The request to create a new MBA.</param>
        /// <returns>A <see cref="Task{ActionResult{MbaDto}}}"/> object containing the asynchronous operation
        /// that returns the created MBA.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status400BadRequest"/> if it could not be completed.
        /// <see cref="StatusCodes.Status404NotFound"/> if the MBA could not be found.
        [HttpPost(nameof(CreateMba))]
        public async Task<ActionResult<MbaDto>> CreateMba([FromBody] MbaCreationDto request)
        {
            // Begin a database transaction
            var _mbaOptionsRepository = (IMbaOptionsRepository)_mbaRepository;
            _mbaRepository.BeginTransaction();

            var AllMbas = await _mbaRepository.GetAllMbaAsync();

            if (AllMbas.Any(mba => mba.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)
                || mba.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase)))
            {
                var msg = "There is already an entity with that data";
                _logger.LogError($"{nameof(CreateMba)} -> {msg}");
                _mbaRepository.RollbackTransaction();
                return BadRequest(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var mbaOptionsParent = await _mbaOptionsRepository.GetMbaOptionsByIdAsync(request.MbaOptionsId);

            if (mbaOptionsParent is null)
            {
                var msg = "MbaOptions parent not found";
                _logger.LogError($"{nameof(CreateMba)} -> {msg}");
                _mbaRepository.RollbackTransaction();
                return NotFound(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var mba = await _mbaRepository.CreateMbAsync(request.Code, request.Name, request.MbaOptionsId);

            if (mba is null)
            {
                var msg = "Has error occurred";
                _logger.LogError($"{nameof(CreateMba)} -> {msg}");
                _mbaRepository.RollbackTransaction();
                return BadRequest(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var mbaDto = mba.Map();
            _mbaRepository.CommitTransaction();
            return Ok(mbaDto);
        }

        /// <summary>
        /// Adds a range of MBAs.
        /// </summary>
        /// <returns>A <see cref="Task{ActionResult{IEnumerable{MbaDto}}}"/> object containing the asynchronous operation
        /// that returns the list of added MBAs.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status400BadRequest"/> if it could not be completed.
        [HttpPost(nameof(AddMbaByRange))]
        public async Task<ActionResult<IEnumerable<MbaDto>>> AddMbaByRange()
        {
            // Begin a database transaction
            _mbaRepository.BeginTransaction();

            var mbaList = new List<Mba>();
            mbaList = (await _mbaRepository.AddRangeMbAsync(mbaList)).ToList();

            if (mbaList is null)
            {
                var msg = "Has error occurred";
                _logger.LogError($"{nameof(AddMbaByRange)} -> {msg}");
                _mbaRepository.RollbackTransaction();
                return BadRequest(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var mbaListDto = mbaList.Select(mba => mba.Map());
            _mbaRepository.CommitTransaction();
            return Ok(mbaListDto);
        }

        /// <summary>
        /// Updates an existing MBA.
        /// </summary>
        /// <param name="request">The request to update an MBA.</param>
        /// <returns>A <see cref="Task{ActionResult}"/> object containing the asynchronous operation.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status404NotFound"/> if the MBA could not be found.
        [HttpPost(nameof(UpdateMba))]
        public async Task<ActionResult> UpdateMba([FromBody] MbaDto request)
        {
            // Begin a database transaction
            _mbaRepository.BeginTransaction();

            var oldMba = await _mbaRepository.GetMbaByIdAsync(request.MbaId);
            if (oldMba is null)
            {
                var msg = "Mba not found";
                _logger.LogError($"{nameof(UpdateMba)} -> {msg}");
                _mbaRepository.RollbackTransaction();
                return NotFound(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var AllMbas = await _mbaRepository.GetAllMbaAsync();

            if (AllMbas.Any(mba => mba.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)
                || mba.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase))
                && !oldMba.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase))
            {
                var msg = "There is already an entity with that data";
                _logger.LogError($"{nameof(UpdateMba)} -> {msg}");
                _mbaRepository.RollbackTransaction();
                return BadRequest(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            oldMba.Update(request);
            await _mbaRepository.UpdateMbAsync(oldMba);
            _mbaRepository.CommitTransaction();

            return Ok(new ResponseDTO { IsSuccessful = true });
        }

        #endregion Post Actions

        #region Delete Actions

        /// <summary>
        /// Deletes an existing MBA.
        /// </summary>
        /// <param name="id">The identifier of the MBA to delete.</param>
        /// <returns>A <see cref="Task{ActionResult}"/> object containing the asynchronous operation.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status404NotFound"/> if the MBA could not be found.
        [HttpDelete(nameof(DeleteMba) + "/{id}")]
        public async Task<ActionResult> DeleteMba([FromRoute] Guid id)
        {
            // Begin a database transaction
            _mbaRepository.BeginTransaction();

            var mba = await _mbaRepository.GetMbaByIdAsync(id);
            if (mba is null)
            {
                var msg = "Mba not found";
                _logger.LogError($"{nameof(DeleteMba)} -> {msg}");
                _mbaRepository.RollbackTransaction();
                return NotFound(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            await _mbaRepository.DeleteMbAsync(mba);
            _mbaRepository.CommitTransaction();

            return Ok(new ResponseDTO { IsSuccessful = true });
        }

        #endregion Delete Actions
    }

}

