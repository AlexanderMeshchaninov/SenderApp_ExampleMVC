using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Abstractions.Data.RolesRepository
{
    public interface IRolesRepository<TResponseDto> 
        where TResponseDto : class
    {
        Task<TResponseDto> CreateRoleAsync(string name);
        Task<TResponseDto> UpdateRoleByIdAsync(string id, List<string> roles);
        Task<TResponseDto> DeleteRoleByIdAsync(string id);
    }
}