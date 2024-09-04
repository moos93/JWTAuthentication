using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IUser
    {
        Task<RegistrationRespons> RegisterUserAsync(RegisterUserDTO RegisterUser);
        Task<LoginRespons> LoginUserAsync(LogInDTO loginDTO);
        
    }
}
