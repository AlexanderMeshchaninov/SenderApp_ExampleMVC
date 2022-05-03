using System.Threading.Tasks;

namespace Core.Abstractions.Layers
{
    public interface IRegisterFacade<TResponseDto, TRequestDto> 
        where TResponseDto : class
        where TRequestDto : class
    {
        Task<TResponseDto> RegisterFacadeAsync(string actionName, TRequestDto requestDto);
    }
}