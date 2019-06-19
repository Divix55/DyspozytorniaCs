using System.Threading.Tasks;
using Dyspozytornia.Data;
using Dyspozytornia.Models;

namespace Dyspozytornia.Services
{
    public class LoginService : ILoginService
    {
        private readonly IDbRepository dataSource;

        public LoginService(IDbRepository dataSource)
        {
            this.dataSource = dataSource;
        }

        public async Task<bool> Authenticate(string userName, string password)
        {
            return await Task.Run(() => dataSource.LoginUser(userName, password));
        }

        public async Task<bool> Create(User user)
        {
            return await Task.Run(() => dataSource.CreateUser(user));
        }
    }
}