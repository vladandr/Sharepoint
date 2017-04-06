using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using T = TestProj.Resources.Resources;

namespace TestProj.Core
{
    public class ProjectDocumentsListCreator
    {
        private readonly IDeployer<WebModelNode> webDeployer;
        private readonly ClientContext context;
        private LookupFieldDefinition belongToProjectField;
        private UserFieldDefinition documentResponsibleField;
        private TaxonomyFieldDefinition documentTypeField;
        private DateTimeFieldDefinition expirationDateField;
        private ListDefinition projectDocumentsList;
        private ContentTypeDefinition projectDocumentContemtType;
        private ListViewDefinition projectDocumentsListView;

        public ProjectDocumentsListCreator(IDeployer<WebModelNode> webDeployer, ClientContext context)
        {
            this.webDeployer = webDeployer;
            this.context = context;
        }

        public void Deploy()
        {
            Create();
            var model = SPMeta2Model.NewWebModel(site =>
            {
                site
                    .AddTaxonomyFields(GetTaxonomyFields())
                    .AddDateTimeFields(GetDateTimeFields())
                    .AddUserFields(GetUserFields())
                    .AddLookupFields(GetLookupFields())
                    .AddContentType(projectDocumentContemtType, contentType =>
                    {
                        contentType.AddContentTypeFieldLinks(GetAllFields());
                    })
                    .AddList(projectDocumentsList, list =>
                    {
                        list
                            .AddContentTypeLink(projectDocumentContemtType)
                            .AddListView(projectDocumentsListView);
                    });

            });

            webDeployer.Deploy(context, model);
        }

        private void Create()
        {
            belongToProjectField = GetBelognToProjectField();
            documentResponsibleField = GetDocumentResponsibleField();
            documentTypeField = GetDocumentTypeField();
            expirationDateField = GetExpirationDateField();
            projectDocumentsList = GetProjectDocumentsList();
            projectDocumentContemtType = GetProjectDocumentContemtType();
            projectDocumentsListView = GetProjectDocumentsListView();
        }

        private IEnumerable<FieldDefinition> GetAllFields()
        {
            yield return belongToProjectField;
            yield return documentResponsibleField;
            yield return documentTypeField;
            yield return expirationDateField;
        }


        private IEnumerable<LookupFieldDefinition> GetLookupFields()
        {
            yield return belongToProjectField;
        }

        private IEnumerable<TaxonomyFieldDefinition> GetTaxonomyFields()
        {
            yield return documentTypeField;
        }

        private IEnumerable<UserFieldDefinition> GetUserFields()
        {
            yield return documentResponsibleField;
        }

        private IEnumerable<DateTimeFieldDefinition> GetDateTimeFields()
        {
            yield return expirationDateField;
        }

        private TaxonomyFieldDefinition GetDocumentTypeField()
        {
            return new TaxonomyFieldDefinition
            {
                UseDefaultSiteCollectionTermStore = true,
                Id = Guid.NewGuid(),
                Title = T.DocumentTypeFieldTitleName,
                InternalName = T.DocumentTypeFieldInternalName,
                Group = T.Group,
                TermGroupName = T.TestGroupName,
                TermSetName = T.ProjectDocumentTypeTermSetName
            };
        }

        private UserFieldDefinition GetDocumentResponsibleField()
        {
            return new UserFieldDefinition
            {
                Id = Guid.NewGuid(),
                Title = T.DocumentResponsibleFieldTitleName,
                InternalName = T.DocumentResponsibleFieldInternalName,
                Group = T.Group
            };
        }

        private DateTimeFieldDefinition GetExpirationDateField()
        {
            return new DateTimeFieldDefinition
            {
                Id = Guid.NewGuid(),
                Title = T.ExpirationDateFieldTitle,
                InternalName = T.ExpirationDateFieldInternalName,
                DisplayFormat = BuiltInDateTimeFieldFormatType.DateOnly,
                Group = T.Group
            };
        }

        private LookupFieldDefinition GetBelognToProjectField()
        {
            return new LookupFieldDefinition
            {
                Title = T.BelongToProjectFieldTitleName,
                InternalName = T.BelongToProjectFieldInternalName,
                Group = T.Group,
                Id = Guid.NewGuid(),
                LookupListUrl = T.ProjectsListCustomUrlName
            };
        }

        private ListDefinition GetProjectDocumentsList()
        {
            return new ListDefinition
            {
                Title = T.ProjectDocumentsListTitleName,
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                ContentTypesEnabled = true,
                CustomUrl = T.ProjectDocumentsListCustomUrlName
            };
        }

        private ContentTypeDefinition GetProjectDocumentContemtType()
        {
            return new ContentTypeDefinition
            {
                Name = T.ProjectContemtTypeName,
                Id = Guid.NewGuid(),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = T.Group
            };
        }

        private ListViewDefinition GetProjectDocumentsListView()
        {
            return new ListViewDefinition
            {
                Title = "Project Documents",                
                Query = CreateQuery(),
                Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.Title,
                    T.DocumentTypeFieldInternalName,
                    T.ExpirationDateFieldInternalName,
                    T.BelongToProjectFieldInternalName,
                    T.DocumentResponsibleFieldInternalName
                }
            };
        }

        private string CreateQuery()
        {
            var createdQuery = new StringBuilder();
            createdQuery.Append("<Where>");
            createdQuery.Append("</Where>");
            createdQuery.Append("<GroupBy Collapse='TRUE'>");
            createdQuery.Append($"<FieldRef Name='{T.BelongToProjectFieldInternalName}'/>");
            createdQuery.Append("</GroupBy>");
            return createdQuery.ToString();
        }
    }
}
