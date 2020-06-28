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
            builder.AddNewtonsoftJson();
            builder.AddApplicationPart(typeof(LfsConstants).GetTypeInfo().Assembly);
            builder.AddMvcOptions(options =>
            {
                options.Filters.Add(new ProducesAttribute(LfsConstants.LfsMediaType.MediaType.Buffer));
                options.Filters.Add(new TypeFilterAttribute(typeof(BasicAuthFilter)));

                foreach (InputFormatter input in options.InputFormatters.OfType<NewtonsoftJsonInputFormatter>())
                {
                    input.SupportedMediaTypes.Add(LfsConstants.LfsMediaType);
                }

                foreach (OutputFormatter output in options.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>())
                {
                    output.SupportedMediaTypes.Add(LfsConstants.LfsMediaType);
                }
            });
            return builder;
        }
    }
}
