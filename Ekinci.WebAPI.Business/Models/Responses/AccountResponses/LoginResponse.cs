using Ekinci.Common.JwtModels;

namespace Ekinci.WebAPI.Business.Models.Responses.AccountResponses
{
    public class LoginResponse
    {
        public bool IsNewUser { get; set; }
        public Token Token { get; set; }
    }    
}