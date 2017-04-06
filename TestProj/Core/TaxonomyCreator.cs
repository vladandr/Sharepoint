using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using T= TestProj.Resources.Resources;

namespace TestProj.Core
{
    public class TaxonomyCreator
    {
        private readonly IDeployer<SiteModelNode> siteDeployer;
        private readonly ClientContext context;

        public TaxonomyCreator(IDeployer<SiteModelNode> siteDeployer, ClientContext context)
        {
            this.siteDeployer = siteDeployer;
            this.context = context;
        }

        public void Deploy()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyTermStore(GetDefaultSiteTermStore(), termStore =>
                {
                    termStore.AddTaxonomyTermGroup(GetVladTestGroup(), group =>
                    {
                        group
                           .AddTaxonomyTermSet(GetDepartmentTermSet(), termSet =>
                            {
                                termSet.AddTaxonomyTerms(GetDepartmentTaxonomyTerms());
                            })
                            .AddTaxonomyTermSet(GetProjectDocumentTypeTermSet(), termSet =>
                            {
                                termSet.AddTaxonomyTerms(GetProjectDocumentTypeTerms());
                            })
                           .AddTaxonomyTermSet(GetProjectStatusTermSet(), termSet =>
                            {
                                termSet.AddTaxonomyTerms(GetProjectStatusTerms());
                            });
                    });
                });
            });

            siteDeployer.Deploy(context, model);
        }


        private TaxonomyTermStoreDefinition GetDefaultSiteTermStore()
        {
            return new TaxonomyTermStoreDefinition
            {
                UseDefaultSiteCollectionTermStore = true
            };
        }

        private TaxonomyTermGroupDefinition GetVladTestGroup()
        {
            return new TaxonomyTermGroupDefinition
            {
                Name = T.TestGroupName
            };
        }

        private TaxonomyTermSetDefinition GetDepartmentTermSet()
        {
            return new TaxonomyTermSetDefinition
            {
                Name = T.DepartmentTermSetName
            };
        }

        private TaxonomyTermSetDefinition GetProjectDocumentTypeTermSet()
        {
            return new TaxonomyTermSetDefinition
            {
                Name = T.ProjectDocumentTypeTermSetName
            };
        }

        private TaxonomyTermSetDefinition GetProjectStatusTermSet()
        {
            return new TaxonomyTermSetDefinition
            {
                Name = T.ProjectStatusTermSetName
            };
        }

        private static IEnumerable<TaxonomyTermDefinition> GetDepartmentTaxonomyTerms()
        {
            yield return new TaxonomyTermDefinition {Name = T.HRDepartmentTermName};
            yield return new TaxonomyTermDefinition {Name = T.MDISDepartmentTermName};
            yield return new TaxonomyTermDefinition {Name = T.QADepartmentTermName};
            yield return new TaxonomyTermDefinition {Name = T.SD1DepartmentTermName};
            yield return new TaxonomyTermDefinition {Name = T.SD2DepartmentTermName};
            yield return new TaxonomyTermDefinition {Name = T.WDDepartmentTermName};
        }

        private static IEnumerable<TaxonomyTermDefinition> GetProjectDocumentTypeTerms()
        {
            yield return new TaxonomyTermDefinition {Name = T.ContractDocumentTypeTermName};
            yield return new TaxonomyTermDefinition {Name = T.RequirementsDocumentTypeTermName};
            yield return new TaxonomyTermDefinition {Name = T.RFXDocumentTypeTermName};
            yield return new TaxonomyTermDefinition {Name = T.TechnicalVersionDocumentTypeTermName};
        }

        private static IEnumerable<TaxonomyTermDefinition> GetProjectStatusTerms()
        {
            yield return new TaxonomyTermDefinition {Name = T.ActivateProjectStatusTermName};
            yield return new TaxonomyTermDefinition {Name = T.CancelledProjectStatusTermName};
            yield return new TaxonomyTermDefinition {Name = T.PreInitPhaseProjectStatusTermName};
            yield return new TaxonomyTermDefinition {Name = T.RejectedProjectStatusTermName};
        }
    }
}
