using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Syntax.Default;

namespace TestProj.Core
{
    public class SiteDeploymentPresenter: IDeployer<SiteModelNode>
    {
        public void Deploy(ClientContext clientContext, SiteModelNode model)
        {
            var csomProvisionService = new StandardCSOMProvisionService();
            csomProvisionService.DeploySiteModel(clientContext, model);
        }
    }
}
