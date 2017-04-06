using Microsoft.SharePoint.Client;
using SPMeta2.Models;

namespace TestProj.Core
{
    public interface IDeployer<T> where T: TypedModelNode
    {
        void Deploy(ClientContext clientContext, T model);
    }
}
