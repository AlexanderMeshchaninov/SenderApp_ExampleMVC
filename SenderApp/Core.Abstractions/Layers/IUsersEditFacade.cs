using System.Threading.Tasks;

namespace Core.Abstractions.Layers
{
    public interface IUsersEditFacade<TResponseDto> 
        where TResponseDto : class
    {
        Task<TResponseDto> UsersReadFacade(string item);
    }
}