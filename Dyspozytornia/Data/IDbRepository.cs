using Dyspozytornia.Models;

namespace Dyspozytornia.Data
{
    public interface IDbRepository
    {
        bool CreateUser(User user);
        bool LoginUser(string userName, string hashedPassword);
    }
}