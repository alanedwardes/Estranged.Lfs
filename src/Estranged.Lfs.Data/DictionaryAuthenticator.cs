using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Estranged.Lfs.Data
{
    public class DictionaryAuthenticator : IAuthenticator
    {
        private readonly IReadOnlyDictionary<string, string> credentials;

        public DictionaryAuthenticator(IReadOnlyDictionary<string, string> credentials)
        {
            this.credentials = credentials;
        }

        public Task Authenticate(string username, string password, LfsPermission requiredPermission, CancellationToken token)
        {
            if (!credentials.ContainsKey(username) || credentials[username] != password)
            {
                throw new InvalidOperationException($"Invalid username/password combination");
            }

            return Task.CompletedTask;
        }
    }
}
