using System.Threading.Tasks;

namespace Core.Abstractions.Data.UsersRepository
{
    public interface IUsersRepository<TResponseDto, TRequestDto> 
        where TResponseDto : class
        where TRequestDto : class
    {
        Task<TResponseDto> UpdateUserByIdAsync(string id, TRequestDto requestDto);
        Task<TResponseDto> DeleteUserByIdAsync(string id);
    }
}