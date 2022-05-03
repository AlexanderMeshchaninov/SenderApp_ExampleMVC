using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Abstractions.Layers
{
    public interface IRolesFacade<TResponseDto> 
        where TResponseDto : class
    {
        Task<TResponseDto> RolesFacadeAsync(
            string actionName, 
            string item, 
            List<string> roles = null);
    }
}