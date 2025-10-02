namespace AddNumbers.API.Models
{
    /// <summary>
    /// Represents the response containing the sum of all numbers stored in session.
    /// </summary>
    public class SumResponse
    {
        /// <summary>
        /// The total sum of all numbers in the session.
        /// </summary>
        public int Sum { get; set; }
    }
}
