using Mappers.DTOs;
using Mappers.;
using Repository.Abstract.MBAbstract;
using System.Text.Json;

namespace ApplicationServices.Extensions
{
    // Define a static class MbaDataBaseSaving
    public static class MbaDataBaseSaving
    {
        // Define an asynchronous method AddMbaOptionsDataBase
        public static async Task AddMbaOptionsDataBase(IMbaOptionsRepository _mbaOptionsRepository, HttpClient _httpClient)
        {
            try
            {
                // Send a GET request to the specified Uri as an asynchronous operation
                var response = await _httpClient.GetAsync("https://api.opendata.esett.com/EXP04/MBAOptions");

                // If the HTTP response status is not successful, throw an exception
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error {response.StatusCode}: Unable to retrieve data from the API.");
                }

                // Read the content of the HTTP response message as a string
                var content = await response.Content.ReadAsStringAsync();
                List<MbaOptionsSerializableDto> mbaOptionsListDtos;

                try
                {
                    // Deserialize the JSON string to a list of MbaOptionsSerializableDto objects
                    mbaOptionsListDtos = JsonSerializer.Deserialize<List<MbaOptionsSerializableDto>>(content);
                }
                catch (JsonException)
                {
                    // If the deserialization fails, throw a JsonException
                    throw new JsonException("Error: Unable to deserialize the API response.");
                }

                // If the deserialized data is null, throw an exception
                if (mbaOptionsListDtos is null)
                {
                    throw new Exception("Error: The deserialized data is null.");
                }

                // Begin a database transaction
                _mbaOptionsRepository.BeginTransaction();
                var _mbaRepository = (IMbaRepository)_mbaOptionsRepository;

                // For each MbaOptionsDto in the list of MbaOptionsDtos
                foreach (var mbaOptionsDto in mbaOptionsListDtos)
                {
                    // Map the MbaOptionsDto to a MbaOption object
                    var mbaOption = mbaOptionsDto.Map();
                    // Create a new MbaOption in the database
                    mbaOption = await _mbaOptionsRepository.CreateMbaOptionsAsync(mbaOption.Country, mbaOption.CountryCode);
                    // Commit the database transaction
                    _mbaOptionsRepository.PartialCommit();
                    // Map the Mbas of the MbaOptionsDto to a list of MbaOptionDtos
                    var mbaOptionsDtos = mbaOptionsDto.Mbas.Select(mba => mba.Map(mbaOption.MbaOptionsId)).ToList();
                    // Add the list of MbaOptionDtos to the database
                    await _mbaRepository.AddRangeMbAsync(mbaOptionsDtos);
                }

                _mbaOptionsRepository.CommitTransaction();
            }
            catch (Exception ex)
            {
                // If an error occurs, write the error message to the console
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }


}
