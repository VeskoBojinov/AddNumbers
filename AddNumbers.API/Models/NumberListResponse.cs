namespace AddNumbers.API.Models
{
    /// <summary>
    /// Represents a response containing a list of numbers and their total count.
    /// </summary>
    public class NumberListResponse
    {
        /// <summary>
        /// The list of integers.
        /// </summary>
        public List<int> Numbers { get; set; }

        /// <summary>
        /// The total count of numbers.
        /// </summary>
        public int Count { get; set; }
    }
}
