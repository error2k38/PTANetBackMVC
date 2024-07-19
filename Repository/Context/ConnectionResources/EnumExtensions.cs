using System.ComponentModel;

namespace Repository.Context.ConnectionResources
{
    /// <summary>
    /// Provides extension methods to enhance the functionality of enums.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Retrieves the description of an enum value.
        /// </summary>
        /// <param name="value">
        /// The enum value.
        /// </param>
        /// <returns>
        /// A string containing the description of the enum value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the description does not exist.
        /// </exception>
        public static string GetDescription(this Enum value)
        {
            // Get the field information for this enum value
            var field = value.GetType().GetField(value.ToString());

            // Get the DescriptionAttribute for this field, if it exists
            if (field != null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                // Return the description
                return attribute.Description;
            }

            // If no DescriptionAttribute exists, throw an exception
            throw new ArgumentNullException($"No description found for enum value {value}", nameof(value));
        }
    }

}
