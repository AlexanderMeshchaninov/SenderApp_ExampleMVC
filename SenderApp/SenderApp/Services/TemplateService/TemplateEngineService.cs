using System;
using System.IO;
using System.Threading.Tasks;
using Core.Abstractions.Services;
using Microsoft.AspNetCore.Hosting;
using RazorEngine;
using RazorEngine.Compilation.ReferenceResolver;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using SenderApp.Models.DTO.Requests;

namespace SenderApp.Services.TemplateService
{
    public interface ITemplateEngineService : ITemplateEngineService<RequestTemplateDto>
    {
    }

    public sealed class TemplateEngineService : ITemplateEngineService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TemplateEngineService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        
        public async Task<string> TemplateEngine(
            string templateFileName, 
            string templateKey, 
            Type modelType, 
            RequestTemplateDto request)
        {
            SetRazorEngineConfiguration();
            
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            //Получаем путь к папке шаблонов + название файла шаблона
            var templateFile = $"{wwwRootPath}/TEMPLATES/{templateFileName}.cshtml";

            string templateSource;
            
            using (var streamReader = new StreamReader(templateFile))
            {
                templateSource = await streamReader.ReadToEndAsync();
            }
            
            //Шаблонизатор Razor
            var report = Engine.Razor.RunCompile(
                templateSource, 
                templateKey, 
                null, 
                new 
                {
                    Email = $"{request.Email}", 
                    Message = $"{request.Message}",
                    Template = $"{request.Template}",
                    CallBackUrl = $"{request.CallBackUrl}"
                });

            return await Task.FromResult(report);
        }
        
        private static void SetRazorEngineConfiguration()
        {
            //Конфигурация RazonEngine
            var config = new TemplateServiceConfiguration();
            config.Language = Language.CSharp;
            //config.EncodedStringFactory = new RawStringFactory();
            //config.EncodedStringFactory = new HtmlEncodedStringFactory();
            config.AllowMissingPropertiesOnDynamic = true;
            config.ReferenceResolver = new UseCurrentAssembliesReferenceResolver();
            var service = RazorEngineService.Create(config);
            Engine.Razor = service;
        }
    }
}