using System;
using System.Threading.Tasks;

namespace Core.Abstractions.Services
{
    public interface ITemplateEngineService<TRequestObject> where TRequestObject : class
    {
        Task<string> TemplateEngine(
            string templateFileName, 
            string templateKey, 
            Type modelType,
            TRequestObject requestObject);
    }
}