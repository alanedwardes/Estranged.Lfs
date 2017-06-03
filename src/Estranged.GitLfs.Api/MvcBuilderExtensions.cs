using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Narochno.Primitives.Json;
using System.Linq;
using System.Reflection;

namespace Estranged.GitLfs.Api
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddGitLfs(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddApplicationPart(typeof(MvcBuilderExtensions).GetTypeInfo().Assembly);
            mvcBuilder.AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new EnumStringConverter());
                options.SerializerSettings.Converters.Add(new OptionalJsonConverter());
            });
            mvcBuilder.AddMvcOptions(options =>
            {
                JsonOutputFormatter jsonOutput = options.OutputFormatters.OfType<JsonOutputFormatter>().First();
                jsonOutput.SupportedMediaTypes.Add(GitLfsConstants.GitLfsMediaType);

                JsonInputFormatter jsonInput = options.InputFormatters.OfType<JsonInputFormatter>().First();
                jsonInput.SupportedMediaTypes.Add(GitLfsConstants.GitLfsMediaType);
            });
            return mvcBuilder;
        }
    }
}
