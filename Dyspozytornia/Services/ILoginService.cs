using System.Threading.Tasks;
using Dyspozytornia.Models;

namespace Dyspozytornia.Services
{
    public interface ILoginService
    {
        Task<bool> Authenticate(string userName, string password);
        Task<bool> Create(User user);
    }
}