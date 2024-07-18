using Mappers.DTOs;
using Mapper.Mappers;
using Microsoft.AspNetCore.Mvc;
using Repository.Abstract.MBAbstract;
using System.Text.Json;

namespace PTNetBackMVC.Controllers
{
    // Define the route and output type for the controller
    [Route("api/[controller]"), Produces(typeof(JsonResult))]
    [ApiController]
    public class MbaOptionsController : ControllerBase
    {
        // Define a logger, a repository, and an HttpClient as private readonly fields
        private readonly ILogger<MbaOptionsController> _logger;
        private readonly IMbaOptionsRepository _mbaOptionsRepository;
        private readonly HttpClient _httpClient;

        // Initialize the logger, the repository, and the HttpClient in the constructor
        public MbaOptionsController(ILogger<MbaOptionsController> logger, IMbaOptionsRepository mbaOptionsRepository, HttpClient httpClient)
        {
            _logger = logger;
            _mbaOptionsRepository = mbaOptionsRepository;
            _httpClient = httpClient;
        }

        #region GetActions

        /// <summary>
        /// Gets the MBA options.
        /// </summary>
        /// <returns>A <see cref="Task{ActionResult{IEnumerable{MbaOptionsDto}}}"/> object containing the asynchronous operation
        /// that returns the list of MBA options.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status400BadRequest"/> if it could not be completed.
        [HttpGet(nameof(GetMbasOptions))]
        public async Task<ActionResult<IEnumerable<MbaOptionsDto>>> GetMbasOptions()
        {
            // Begin a database transaction
            _mbaOptionsRepository.BeginTransaction();

            var mbasOptions = await _mbaOptionsRepository.GetMbasOptionsAsync();

            if (mbasOptions is null)
            {
                var msg = "Has error occurred";
                _logger.LogError($"{nameof(GetMbasOptions)} -> {msg}");
                _mbaOptionsRepository.RollbackTransaction();
                return BadRequest(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var mbasOptionsDto = mbasOptions.Select(mbaOptions => mbaOptions.Map());

            _mbaOptionsRepository.CommitTransaction();
            return Ok(mbasOptionsDto);
        }

        /// <summary>
        /// Gets the MBA options by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the MBA option.</param>
        /// <returns>A <see cref="Task{ActionResult{MbaOptionsDto}}}"/> object containing the asynchronous operation
        /// that returns the MBA options of the given identifier.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status404NotFound"/> if the MBA options could not be found.
        [HttpGet(nameof(GetMbaOptionsById) + "/{id}")]
        public async Task<ActionResult<MbaOptionsDto>> GetMbaOptionsById([FromRoute] Guid id)
        {
            // Begin a database transaction
            _mbaOptionsRepository.BeginTransaction();

            var mbaOptions = await _mbaOptionsRepository.GetMbaOptionsByIdAsync(id);

            if (mbaOptions is null)
            {
                var msg = "MbaOptions not found";
                _logger.LogError($" {nameof(GetMbaOptionsById)} -> {msg}");
                _mbaOptionsRepository.RollbackTransaction();
                return NotFound(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var mbaOptionsDto = mbaOptions.Map();
            _mbaOptionsRepository.CommitTransaction();
            return Ok(mbaOptionsDto);
        }

        #endregion GetActions

        #region Post Actions

        /// <summary>
        /// Creates a new MBA option.
        /// </summary>
        /// <param name="request">The request to create a new MBA option.</param>
        /// <returns>A <see cref="Task{ActionResult{MbaOptionsDto}}}"/> object containing the asynchronous operation
        /// that returns the created MBA option.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status400BadRequest"/> if it could not be completed.
        [HttpPost(nameof(CreateMbaOptions))]
        public async Task<ActionResult<MbaOptionsDto>> CreateMbaOptions([FromBody] MbaOptionsCreateDto request)
        {
            // Begin a database transaction
            _mbaOptionsRepository.BeginTransaction();

            var allMbaOptions = await _mbaOptionsRepository.GetMbasOptionsAsync();

            if (allMbaOptions.Any(mbaOptions => mbaOptions.Country.Equals(request.Country, StringComparison.OrdinalIgnoreCase)
                || mbaOptions.CountryCode.Equals(request.CountryCode, StringComparison.OrdinalIgnoreCase)))
            {
                var msg = "There is already an entity with that data";
                _logger.LogError($"{nameof(CreateMbaOptions)} -> {msg}");
                _mbaOptionsRepository.RollbackTransaction();
                return BadRequest(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var mbaOtions = await _mbaOptionsRepository.CreateMbaOptionsAsync(request.Country, request.CountryCode);

            if (mbaOtions is null)
            {
                var msg = "Has error occurred";
                _logger.LogError($"{nameof(CreateMbaOptions)} -> {msg}");
                _mbaOptionsRepository.RollbackTransaction();
                return BadRequest(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var mbaOptionsDto = mbaOtions.Map();
            _mbaOptionsRepository.CommitTransaction();
            return Ok(mbaOptionsDto);
        }

        /// <summary>
        /// Adds a range of MBA options.
        /// </summary>
        /// <returns>A <see cref="Task{ActionResult{IEnumerable{MbaOptionsDto}}}"/> object containing the asynchronous operation
        /// that returns the list of added MBA options.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status400BadRequest"/> if it could not be completed.
        [HttpPost(nameof(AddMbaOptionsByRange))]
        public async Task<ActionResult<IEnumerable<MbaOptionsDto>>> AddMbaOptionsByRange()
        {
            var response = await _httpClient.GetAsync("https://api.opendata.esett.com/EXP04/MBAOptions");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();
            var mbaOptionsListDtos = JsonSerializer.Deserialize<List<MbaOptionsSerializableDto>>(content);

            if (mbaOptionsListDtos is null)
            {
                var msg = "Has error occurred";
                _logger.LogError($"{nameof(AddMbaOptionsByRange)} -> {msg}");
            }

            await AddMbaOptionsTodatabase(mbaOptionsListDtos);

            _mbaOptionsRepository.CommitTransaction();
            return Ok(mbaOptionsListDtos);
        }

        /// <summary>
        /// Updates an existing MBA option.
        /// </summary>
        /// <param name="request">The request to update an MBA option.</param>
        /// <returns>A <see cref="Task{ActionResult}"/> object containing the asynchronous operation.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status404NotFound"/> if the MBA option could not be found.
        [HttpPost(nameof(UpdateMbaOptions))]
        public async Task<ActionResult> UpdateMbaOptions([FromBody] MbaOptionsDto request)
        {
            // Begin a database transaction
            _mbaOptionsRepository.BeginTransaction();

            var oldMbaOptions = await _mbaOptionsRepository.GetMbaOptionsByIdAsync(request.MbaOptionsId);
            if (oldMbaOptions is null)
            {
                var msg = "Mba options not found";
                _logger.LogError($"{nameof(UpdateMbaOptions)} -> {msg}");
                _mbaOptionsRepository.RollbackTransaction();
                return NotFound(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            var allMbaOptions = await _mbaOptionsRepository.GetMbasOptionsAsync();

            if (allMbaOptions.Any(mbaOptions => mbaOptions.Country.Equals(request.Country, StringComparison.OrdinalIgnoreCase)
               || mbaOptions.CountryCode.Equals(request.CountryCode, StringComparison.OrdinalIgnoreCase))
                && !oldMbaOptions.Country.Equals(request.Country, StringComparison.OrdinalIgnoreCase))
            {
                var msg = "There is already an entity with that data";
                _logger.LogError($"{nameof(UpdateMbaOptions)} -> {msg}");
                _mbaOptionsRepository.RollbackTransaction();
                return BadRequest(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            oldMbaOptions.Update(request);
            await _mbaOptionsRepository.UpdateMbaOptionsAsync(oldMbaOptions);
            _mbaOptionsRepository.CommitTransaction();

            return Ok(new ResponseDTO { IsSuccessful = true });
        }

        #endregion Post Actions

        #region Delete Actions

        /// <summary>
        /// Deletes an existing MBA option.
        /// </summary>
        /// <param name="id">The identifier of the MBA option to delete.</param>
        /// <returns>A <see cref="Task{ActionResult}"/> object containing the asynchronous operation.</returns>
        /// <see cref="StatusCodes.Status200OK"/> if it was executed correctly.
        /// <see cref="StatusCodes.Status404NotFound"/> if the MBA option could not be found.
        [HttpDelete(nameof(DeleteMbaOptions) + "/{id}")]
        public async Task<ActionResult> DeleteMbaOptions([FromRoute] Guid id)
        {
            // Begin a database transaction
            _mbaOptionsRepository.BeginTransaction();

            var mbaOptions = await _mbaOptionsRepository.GetMbaOptionsByIdAsync(id);
            if (mbaOptions is null)
            {
                var msg = "Mba options not found";
                _logger.LogError($"{nameof(DeleteMbaOptions)} -> {msg}");
                _mbaOptionsRepository.RollbackTransaction();
                return NotFound(new ResponseSingleErrorDTO { IsSuccessful = false, Error = msg });
            }

            await _mbaOptionsRepository.DeleteMbaOptionsAsync(mbaOptions);
            _mbaOptionsRepository.CommitTransaction();

            return Ok(new ResponseDTO { IsSuccessful = true });
        }

        #endregion Delete Actions

        #region Helpers

        /// <summary>
        /// Adds a list of MBA options to the database.
        /// </summary>
        /// <param name="mbaOptionsListDtos">The list of MBA options to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task AddMbaOptionsTodatabase(List<MbaOptionsSerializableDto>? mbaOptionsListDtos)
        {
            _mbaOptionsRepository.BeginTransaction();
            var _mbaRepository = (IMbaRepository)_mbaOptionsRepository;

            foreach (var mbaOptionsDto in mbaOptionsListDtos)
            {
                var mbaOption = mbaOptionsDto.Map();
                mbaOption = await _mbaOptionsRepository.CreateMbaOptionsAsync(mbaOption.Country, mbaOption.CountryCode);
                _mbaOptionsRepository.PartialCommit();
                var mbaOptionsDtos = mbaOptionsDto.Mbas.Select(mba => mba.Map(mbaOption.MbaOptionsId)).ToList();
                await _mbaRepository.AddRangeMbAsync(mbaOptionsDtos);
            }
        }

        #endregion Helpers
    }
}
