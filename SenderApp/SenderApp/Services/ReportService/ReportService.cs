using System;
using System.IO;
using System.Threading.Tasks;
using Core.Abstractions.Services;
using Microsoft.AspNetCore.Hosting;
using SenderApp.Models.DTO.Requests;
using SenderApp.Services.TemplateService;

namespace SenderApp.Services.ReportService
{
    public interface IReportService : IReportService<RequestTemplateDto>
    {
    }

    public sealed class ReportService : IReportService
    {
        private readonly ITemplateEngineService _templateEngineService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportService(
            IWebHostEnvironment webHostEnvironment, 
            ITemplateEngineService templateEngineService)
        {
            _webHostEnvironment = webHostEnvironment;
            _templateEngineService = templateEngineService;
        }

        public async Task<string> GenerateReportAsync(
            string reportFileName,
            string templateFileName,
            string templateKey, 
            RequestTemplateDto request, 
            string output = "")
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            
            if (request is null)
            {
                return await Task.FromResult(string.Empty);
            }
            
            //Получаем путь к папке репорт + название файла
            if (string.IsNullOrWhiteSpace(output))
            {
                output = Path.Combine(
                    wwwRootPath + "/REPORTS/",
                    $"{reportFileName}{DateTime.Now.ToString("yymmssfff")}.html");
            }
            
            //Получаем путь к папке шаблонов + название файла шаблона
            var templateFile = $"{wwwRootPath}/TEMPLATES/{templateFileName}.cshtml";

            File.Copy(templateFile,output);
            
            var report = await _templateEngineService.TemplateEngine(
                templateFileName, 
                templateKey, 
                null, 
                request);
            
            using (var streamWriter = new StreamWriter(output))
            {
                await streamWriter.WriteAsync(report);
            }
            
            return await Task.FromResult(report);
        }
    }
}