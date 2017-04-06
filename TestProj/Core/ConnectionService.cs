using Microsoft.SharePoint.Client;
using TestProj.Extensions;

namespace TestProj.Core
{
    public class ConnectionService: IConnector
    {
        public ClientContext Connect(string webFullUrl, string login, string password)
        {
            var clientContext = new ClientContext(webFullUrl);
            clientContext.Credentials = new SharePointOnlineCredentials(login, password.ToSecureString());
            return clientContext;
        }
    }
}
