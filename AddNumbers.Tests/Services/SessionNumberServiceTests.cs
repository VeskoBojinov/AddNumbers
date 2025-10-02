using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using AddNumbers.API.Services;
using System.Text.Json;
using System.Text;

namespace AddNumbers.Tests.Services
{
    public class SessionNumberServiceTests
    {
        private readonly Mock<ISession> _sessionMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly DefaultHttpContext _httpContext;

        private readonly SessionNumberService _service;

        private const string SessionKey = "NumberList";

        public SessionNumberServiceTests()
        {
            _sessionMock = new Mock<ISession>();

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = _sessionMock.Object;

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(_httpContext);

            _service = new SessionNumberService(_httpContextAccessorMock.Object);
        }

        private void SetupSessionWithNumbers(List<int> numbers)
        {
            var json = JsonSerializer.Serialize(numbers);
            var jsonBytes = Encoding.UTF8.GetBytes(json);

            // Out parameter setup trick
            _sessionMock.Setup(s => s.TryGetValue(SessionKey, out jsonBytes)).Returns(true);

            // Handle Set() to update TryGetValue behavior for future reads
            _sessionMock.Setup(s => s.Set(SessionKey, It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, val) =>
                {
                    _sessionMock.Setup(s => s.TryGetValue(SessionKey, out val)).Returns(true);
                });
        }

        [Fact]
        public void GetAll_ReturnsDefaultList_WhenSessionIsEmpty()
        {
            // No value set for session — simulate missing session key
            byte[] unused;
            _sessionMock.Setup(s => s.TryGetValue(SessionKey, out unused)).Returns(false);

            var result = _service.GetAll();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void AddRandom_AddsNumberToList()
        {
            var initial = new List<int> { 1, 2 };
            SetupSessionWithNumbers(initial);

            var added = _service.AddRandom();
            var all = _service.GetAll();

            Assert.Contains(added, all);
            Assert.Equal(3, all.Count);
        }

        [Fact]
        public void Clear_EmptiesTheList()
        {
            var numbers = new List<int> { 1, 2, 3 };
            SetupSessionWithNumbers(numbers);

            _service.Clear();

            var result = _service.GetAll();
            Assert.Empty(result);
        }

        [Fact]
        public void GetSum_ReturnsCorrectValue()
        {
            var numbers = new List<int> { 5, 15, 30 };
            SetupSessionWithNumbers(numbers);

            var sum = _service.GetSum();
            Assert.Equal(50, sum);
        }

        [Fact]
        public void GetCount_ReturnsCorrectValue()
        {
            var numbers = new List<int> { 1, 2, 3, 4 };
            SetupSessionWithNumbers(numbers);

            var count = _service.GetCount();
            Assert.Equal(4, count);
        }
    }
}