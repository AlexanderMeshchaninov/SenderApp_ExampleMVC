using System.Threading.Tasks;

namespace Core.Abstractions.Data.RolesRepository
{
    public interface IRolesEditRepository<TResponseDto> 
        where TResponseDto : class
    {
        Task<TResponseDto> ReadRoleByIdAsync(string id);
    }
}