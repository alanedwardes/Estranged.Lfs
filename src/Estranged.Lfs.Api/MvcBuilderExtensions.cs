using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Narochno.Primitives.Json;
using System.Linq;
using System.Reflection;

namespace Estranged.Lfs.Api
{
    public static class MvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddGitLfs(this IMvcCoreBuilder builder)
        {
            builder.AddJsonFormatters();
            builder.AddApplicationPart(typeof(MvcBuilderExtensions).GetTypeInfo().Assembly);
            builder.AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new EnumStringConverter());
                options.SerializerSettings.Converters.Add(new OptionalJsonConverter());
            });
            builder.AddMvcOptions(options =>
            {
                options.Filters.Add(new ProducesAttribute(LfsConstants.GitLfsMediaType.MediaType));

                JsonOutputFormatter jsonOutput = options.OutputFormatters.OfType<JsonOutputFormatter>().First();
                jsonOutput.SupportedMediaTypes.Add(LfsConstants.GitLfsMediaType);

                JsonInputFormatter jsonInput = options.InputFormatters.OfType<JsonInputFormatter>().First();
                jsonInput.SupportedMediaTypes.Add(LfsConstants.GitLfsMediaType);
            });
            return builder;
        }
    }
}
