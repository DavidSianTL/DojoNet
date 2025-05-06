using Examen_mes_abril.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;

namespace Examen_mes_abril.Middleware
{
    public class ViewFallbackMiddleware
    {
        private readonly RequestDelegate _next;

        public ViewFallbackMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICompositeViewEngine viewEngine)
        {
            var originalBody = context.Response.Body;

            using (var memStream = new MemoryStream())
            {
                context.Response.Body = memStream;

                await _next(context);

                // Si el status code es 404 y la URL no corresponde a una acción válida
                if (context.Response.StatusCode == 404)
                {
                    // Aquí se maneja el error 404
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/html";

                    var viewResult = viewEngine.FindView(
                        new ActionContext
                        {
                            HttpContext = context,
                            RouteData = context.GetRouteData(),
                            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
                        },
                        "Error", false
                    );

                    if (viewResult.Success)
                    {
                        using (var writer = new StreamWriter(originalBody))
                        {
                            var viewContext = new ViewContext
                            {
                                HttpContext = context,
                                Writer = writer,
                                View = viewResult.View,
                                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ErrorViewModel>(
                                    metadataProvider: new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                                    modelState: new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()
                                )
                                {
                                    Model = new ErrorViewModel
                                    {
                                        RequestId = context.TraceIdentifier
                                    }
                                }
                            };

                            await viewResult.View.RenderAsync(viewContext);
                        }

                        return;
                    }
                }

                memStream.Position = 0;
                await memStream.CopyToAsync(originalBody);
            }
        }
    }
}
