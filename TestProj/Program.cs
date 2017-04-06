using System;
using SPMeta2.Syntax.Default;
using TestProj.Configuration;
using TestProj.Core;

namespace TestProj
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationService configuration = new ConfigurationService();
            IConnector connector = new ConnectionService();
            IDeployer<SiteModelNode> siteDeployer = new SiteDeploymentPresenter();
            IDeployer<WebModelNode> webDeployer = new WebDeploymentPresenter();

            var url = configuration.GetConfigurationValue(ConfigurationDesignators.Url);
            var password = configuration.GetConfigurationValue(ConfigurationDesignators.Password);
            var login = configuration.GetConfigurationValue(ConfigurationDesignators.Login);

            var context = connector.Connect(url, login, password);

            var taxonomyCreator = new TaxonomyCreator(siteDeployer, context);
            var projectListCreator = new ProjectListCreator(webDeployer, context);
            var projectDocumentsListCreator = new ProjectDocumentsListCreator(webDeployer, context);

            taxonomyCreator.Deploy();
            projectListCreator.Deploy();
            projectDocumentsListCreator.Deploy();
            Console.WriteLine("Good");
            Console.ReadKey();
        }
    }
}

