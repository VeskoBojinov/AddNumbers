namespace AddNumbers.API.Models
{
    /// <summary>
    /// Represents the response returned after a number is added to the session list.
    /// </summary>
    public class AddNumberResponse
    {
        /// <summary>
        /// The number that was randomly generated and added.
        /// </summary>
        public int Added { get; set; }
    }
}
