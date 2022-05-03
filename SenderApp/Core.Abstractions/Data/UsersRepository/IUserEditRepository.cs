using System.Threading.Tasks;

namespace Core.Abstractions.Data.UsersRepository
{
    public interface IUsersEditRepository<TResponseDto> 
        where TResponseDto : class
    {
        Task<TResponseDto> ReadUserByIdAsync(string id);
    }
}