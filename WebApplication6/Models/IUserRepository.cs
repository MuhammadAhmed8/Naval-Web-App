namespace WebApplication6.Models
{
    public interface IUserRepository
    {
        User GetUser(string username);
        string GetPassword(string username);
        void SetTokens(string selector, string token, int username, System.DateTime expires);
        Auth_tokens GetTokens(string selector);
    }
}