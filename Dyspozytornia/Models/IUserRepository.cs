namespace Dyspozytornia.Models
{
    public interface IUserRepository
    {
        UserModel getUserById(int Id);
    }
}