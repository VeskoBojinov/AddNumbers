using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AddNumbers.API.Services
{
    /// <summary>
    /// Service that manages a list of numbers stored in session state.
    /// </summary>
    public class SessionNumberService : ISessionNumberService
    {
        private const string SessionKey = "NumberList";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly Random _random = new();

        public SessionNumberService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext!.Session;
        }

        /// <summary>
        /// Retrieves the current list of numbers from the session.
        /// </summary>
        /// <returns>A list of integers.</returns>
        public List<int> GetAll() => GetNumbers();

        /// <summary>
        /// Gets the count of numbers in the session.
        /// </summary>
        public int GetCount() => GetNumbers().Count;

        /// <summary>
        /// Sums all numbers stored in session.
        /// </summary>
        public int GetSum() => GetNumbers().Sum();

        /// <summary>
        /// Adds a randomly generated number (0–999) to the list.
        /// </summary>
        /// <returns>The added number.</returns>
        public int AddRandom()
        {
            var numbers = GetNumbers();
            int newNumber = _random.Next(0, 1000);
            numbers.Add(newNumber);
            SaveNumbers(numbers);
            return newNumber;
        }

        /// <summary>
        /// Clears all numbers from the session.
        /// </summary>
        public void Clear() => SaveNumbers(new List<int>());

        // Private helper: load the list from session or initialize it
        private List<int> GetNumbers()
        {
            var json = _session.GetString(SessionKey);
            if (json != null)
                return JsonSerializer.Deserialize<List<int>>(json)!;

            var initial = new List<int> ();
            SaveNumbers(initial);
            return initial;
        }

        // Private helper: saves the list to session
        private void SaveNumbers(List<int> numbers)
        {
            _session.SetString(SessionKey, JsonSerializer.Serialize(numbers));
        }
    }
}
