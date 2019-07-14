namespace WebApplication6.Models
{
    public interface IUserManager
    {
        User FindUser(string username, string password);
        string GenerateToken();
        string GenerateSelector();
        string Sha256(string randomString);
        void SetAuthCredentials(string selector, string validator, int username, System.DateTime dateTime);
        int? VerifyAuthCredentials(string selector, string validator);

    }
}