using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Services.Email
{
    public class EmailSender: IEmailSender
    {
        public EmailAuthOptions Options { get; set; }
        public IConfiguration Configuration { get; }
        public IServiceProvider Service;
        private readonly ILogger<EmailSender> log;
        public EmailSender(IOptions<EmailAuthOptions> optionsAccessor, IServiceProvider service, IConfiguration configuration, ILogger<EmailSender> logger)
        {
            Service = service;
            Options = optionsAccessor.Value;
            Configuration = configuration;
            log = logger;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }
        public async Task Execute(string apiKey, string subject, string message, string email)
        {

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Configuration["Email"], Configuration["Name"]);
            var to = new EmailAddress(email);

            var plainTextContent = message;
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            try
            {
                var response = await client.SendEmailAsync(msg);

            }
            catch (Exception e)
            {
                log.LogCritical("Error in Sending Email.\n"+e.ToString());
            }
        }
    }
    public static class ViewToStringRenderer
    {
        public static async Task<string> RenderViewToStringAsync<TModel>(IServiceProvider requestServices, string viewName, TModel model)
        {
            var viewEngine = requestServices.GetRequiredService(typeof(IRazorViewEngine)) as IRazorViewEngine;
            ViewEngineResult viewEngineResult = viewEngine.GetView(null, viewName, false);
            if (viewEngineResult.View == null)
            {
                throw new Exception("Could not find the View file. Searched locations:\r\n" + string.Join("\r\n", viewEngineResult.SearchedLocations));
            }
            else
            {
                IView view = viewEngineResult.View;
                var httpContextAccessor = (IHttpContextAccessor)requestServices.GetRequiredService(typeof(IHttpContextAccessor));
                var actionContext = new ActionContext(httpContextAccessor.HttpContext, new RouteData(), new ActionDescriptor());
                var tempDataProvider = requestServices.GetRequiredService(typeof(ITempDataProvider)) as ITempDataProvider;

                var outputStringWriter = new StringWriter();
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model },
                    new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                    outputStringWriter,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return outputStringWriter.ToString();
            }
        }
    }
}
