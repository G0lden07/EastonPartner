using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EastonPartners.Domain.DTOs.Authentication;
using EastonPartners.Domain.Entities.Identity;

namespace EastonPartners.Infrastructure.Authentication.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<ApplicationUser?> GetById(string id);
    }
}
