using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Estranged.Lfs.Data
{
    public class DictionaryAuthenticator : IAuthenticator
    {
        private readonly IDictionary<string, string> credentials;

        public DictionaryAuthenticator(IDictionary<string, string> credentials)
        {
            this.credentials = credentials;
        }

        public Task Authenticate(string username, string password, LfsPermission requiredPermission)
        {
            if (!credentials.ContainsKey(username) || credentials[username] != password)
            {
                throw new InvalidOperationException($"Invalid username/password combination");
            }

            return Task.CompletedTask;
        }
    }
}
