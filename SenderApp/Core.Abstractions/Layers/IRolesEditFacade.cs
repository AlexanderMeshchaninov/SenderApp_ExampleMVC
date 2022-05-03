using System.Threading.Tasks;

namespace Core.Abstractions.Layers
{
    public interface IRolesEditFacade<TResponseDto> 
        where TResponseDto : class
    {
        Task<TResponseDto> RolesReadFacade(string item);
    }
}