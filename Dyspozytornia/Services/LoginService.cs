using System.Threading.Tasks;
using Dyspozytornia.Data;
using Dyspozytornia.Models;

namespace Dyspozytornia.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _dataSource;

        public LoginService(ILoginRepository dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<bool> Authenticate(string userName, string password)
        {
            return await Task.Run(() => _dataSource.LoginUser(userName, password));
        }

        public async Task<bool> Create(User user)
        {
            return await Task.Run(() => _dataSource.CreateUser(user));
        }
    }
}