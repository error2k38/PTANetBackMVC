namespace Mappers.DTOs
{
    /// <summary>
    /// Represents a data transfer object (DTO) for responses.
    /// </summary>
    public record ResponseDTO
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the collection of error messages, if any.
        /// </summary>
        public IEnumerable<string>? Errors { get; set; }
    }

    /// <summary>
    /// Represents a data transfer object (DTO) for responses that contain a single error message.
    /// </summary>
    public record ResponseSingleErrorDTO
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the error message, if any.
        /// </summary>
        public string? Error { get; set; }
    }

}
