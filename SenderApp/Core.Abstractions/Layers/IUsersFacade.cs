using System.Threading.Tasks;

namespace Core.Abstractions.Layers
{
    public interface IUsersFacade<TResponseDto, TRequestDto> 
        where TResponseDto : class
        where TRequestDto : class
    {
        Task<TResponseDto> UsersFacadeAsync(string actionName, TRequestDto requestDto);
        Task<TResponseDto> UserChangePasswordFacade(TRequestDto requestDto);
    }
}