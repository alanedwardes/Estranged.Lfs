using Estranged.Lfs.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Estranged.Lfs.Api
{
    public static class MvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddGitLfs(this IMvcCoreBuilder builder)
        {
            builder.AddJsonFormatters();
            builder.AddApplicationPart(typeof(LfsConstants).GetTypeInfo().Assembly);
            builder.AddMvcOptions(options =>
            {
                options.Filters.Add(new ProducesAttribute(LfsConstants.LfsMediaType.MediaType.Buffer));
                options.Filters.Add(new TypeFilterAttribute(typeof(BasicAuthFilter)));

                JsonOutputFormatter jsonOutput = options.OutputFormatters.OfType<JsonOutputFormatter>().First();
                jsonOutput.SupportedMediaTypes.Add(LfsConstants.LfsMediaType);

                JsonInputFormatter jsonInput = options.InputFormatters.OfType<JsonInputFormatter>().First();
                jsonInput.SupportedMediaTypes.Add(LfsConstants.LfsMediaType);
            });
            return builder;
        }
    }
}
