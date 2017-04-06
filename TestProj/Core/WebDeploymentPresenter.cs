using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Syntax.Default;

namespace TestProj.Core
{
    public class WebDeploymentPresenter: IDeployer<WebModelNode>
    {
        public void Deploy(ClientContext clientContext, WebModelNode model)
        {
            var csomProvisionService = new StandardCSOMProvisionService();
            csomProvisionService.DeployWebModel(clientContext, model);
        }
    }
}
