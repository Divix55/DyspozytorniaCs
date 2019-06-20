using Dyspozytornia.Models;

namespace Dyspozytornia.Data
{
    public interface ILoginRepository
    {
        bool CreateUser(User user);
        bool LoginUser(string userName, string hashedPassword);
    }
}