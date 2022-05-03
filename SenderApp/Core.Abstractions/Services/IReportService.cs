using System.Threading.Tasks;

namespace Core.Abstractions.Services
{
    public interface IReportService<TRequestObject> where TRequestObject : class
    {
        Task<string> GenerateReportAsync(
            string reportFileName, 
            string templateFileName, 
            string templateKey, 
            TRequestObject requestObject, 
            string output = "");
    }
}