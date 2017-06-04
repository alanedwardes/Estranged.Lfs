using System.Collections.Generic;

namespace Estranged.Lfs.Data
{
    public class DictionaryAuthenticator : IAuthenticator
    {
        private readonly IDictionary<string, string> credentials;

        public DictionaryAuthenticator(IDictionary<string, string> credentials)
        {
            this.credentials = credentials;
        }

        public bool Authenticate(string username, string password)
        {
            return credentials.ContainsKey(username) && credentials[username] == password;
        }
    }
}
