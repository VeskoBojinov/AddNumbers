namespace AddNumbers.API.Services
{
    public interface ISessionNumberService
    {
        List<int> GetAll();
        int GetCount();
        int GetSum();
        int AddRandom();
        void Clear();
    }
}
