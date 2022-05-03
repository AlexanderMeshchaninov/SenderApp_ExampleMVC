using System.Threading.Tasks;

namespace Core.Abstractions.Layers
{
    public interface ILoginFacade<TResponseDto, TRequestDto> 
        where TResponseDto : class
        where TRequestDto : class
    {
        Task<TResponseDto> LoginFacadeAsync(TRequestDto requestDto);
    }
}