
using Microsoft.SharePoint.Client;

namespace TestProj.Core
{
    public interface IConnector
    {
        ClientContext Connect(string webFullUrl, string login, string password);
    }
}
