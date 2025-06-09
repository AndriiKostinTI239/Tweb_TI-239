// IAuthService.cs
using FRM.Core.DTOs;
using System.Threading.Tasks;
using FRM.Core.DTOs;


namespace FRM.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> SignUpAsync(SignUpDto dto);
        Task<bool> SignInAsync(SignInDto dto);
    }
}