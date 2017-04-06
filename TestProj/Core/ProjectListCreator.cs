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
    public class ProjectListCreator
    {
        private readonly IDeployer<WebModelNode> webDeployer;
        private readonly ClientContext context;
        private TaxonomyFieldDefinition projectStatusField;
        private DateTimeFieldDefinition startDateField;
        private UserFieldDefinition teamField;
        private TaxonomyFieldDefinition departmentField;
        private DateTimeFieldDefinition endDateField;
        private UserFieldDefinition projectManagerField;
        private NoteFieldDefinition projectValueField;
        private ContentTypeDefinition projectContemtType;
        private ListDefinition projectsList;
        private ListViewDefinition managedProjectsListView;

        public ProjectListCreator(IDeployer<WebModelNode> webDeployer, ClientContext context)
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
                    .AddNoteFields(GetNoteFields())
                    .AddContentType(projectContemtType, contentType =>
                    {
                        contentType.AddContentTypeFieldLinks(GetAllFields());
                    })
                    .AddList(projectsList, list =>
                    {
                        list
                            .AddContentTypeLink(projectContemtType)
                            .AddListView(managedProjectsListView);
                    });
            });

            webDeployer.Deploy(context, model);
        }

        public void Create()
        {
            projectStatusField = GetProjectStatusField();
            startDateField = GetStartDateField();
            teamField = GetTeamField();
            departmentField = GetDepartmentField();
            endDateField = GetEndDateField();
            projectManagerField = GetProjectManagerField();
            projectValueField = GetProjectValueField();
            projectContemtType = GetProjectContemtType();
            projectsList = GetProjectsList();
            managedProjectsListView = GetManagedProjectsListView();
        }

        private IEnumerable<FieldDefinition> GetAllFields()
        {
            yield return projectStatusField;
            yield return startDateField;
            yield return teamField;
            yield return departmentField;
            yield return endDateField;
            yield return projectManagerField;
            yield return projectValueField;
        }

        private IEnumerable<NoteFieldDefinition> GetNoteFields()
        {
            yield return projectValueField;
        }

        private IEnumerable<UserFieldDefinition> GetUserFields()
        {
            yield return projectManagerField;
            yield return teamField;
        }

        private IEnumerable<DateTimeFieldDefinition> GetDateTimeFields()
        {
            yield return startDateField;
            yield return endDateField;
        }

        private IEnumerable<TaxonomyFieldDefinition> GetTaxonomyFields()
        {
            yield return projectStatusField;
            yield return departmentField;
        }

        private TaxonomyFieldDefinition GetProjectStatusField()
        {
            return new TaxonomyFieldDefinition
            {
                UseDefaultSiteCollectionTermStore = true,
                Id = Guid.NewGuid(),
                Title = T.ProjectStatusFieldTitle,
                InternalName = T.ProjectStatusFieldTitleInternalName,
                Group = T.Group,
                TermGroupName = T.TestGroupName,
                TermSetName = T.ProjectStatusTermSetName
            };
        }

        private DateTimeFieldDefinition GetStartDateField()
        {
            return new DateTimeFieldDefinition
            {
                Id = Guid.NewGuid(),
                Title = T.StartDateFieldTitle,
                InternalName = T.StartDateFieldInternalName,
                DisplayFormat = BuiltInDateTimeFieldFormatType.DateOnly,
                Group = T.Group
            };
        }

        private DateTimeFieldDefinition GetEndDateField()
        {
            return new DateTimeFieldDefinition
            {
                Id = Guid.NewGuid(),
                Title = T.EndDateFieldTitle,
                InternalName = T.EndDateFieldInternalName,
                DisplayFormat = BuiltInDateTimeFieldFormatType.DateOnly,
                Group = T.Group
            };
        }

        private UserFieldDefinition GetProjectManagerField()
        {
            return new UserFieldDefinition
            {
                Id = Guid.NewGuid(),
                Title = T.ProjectManagerFieldTitle,
                InternalName = T.ProjectManagerFieldInternalName,
                Group = T.Group
            };
        }

        private UserFieldDefinition GetTeamField()
        {
            return new UserFieldDefinition
            {
                Id = Guid.NewGuid(),
                Title = T.TeamFieldTitleName,
                InternalName = T.TeamFieldInternalName,
                Group = T.Group,
                //not working (Field or property \"AllowMultipleValues\" does not exist) error in deploy
                AllowMultipleValues = true 
            };
        }

        private TaxonomyFieldDefinition GetDepartmentField()
        {
            return new TaxonomyFieldDefinition
            {
                UseDefaultSiteCollectionTermStore = true,
                Id = Guid.NewGuid(),
                Title = T.DepartmentFieldTitleName,
                InternalName = T.DepartmentFieldInternalName,
                Group = T.Group,
                TermGroupName = T.TestGroupName,
                TermSetName = T.DepartmentTermSetName
            };
        }

        private NoteFieldDefinition GetProjectValueField()
        {
            return new NoteFieldDefinition
            {
                Id = Guid.NewGuid(),
                Title = T.ProjectValueFieldTitleName,
                InternalName = T.ProjectValueFieldInternalName,
                Group = T.Group
            };
        }

        private ContentTypeDefinition GetProjectContemtType()
        {
            return new ContentTypeDefinition
            {
                Name = T.ProjectContemtTypeName,
                Id = Guid.NewGuid(),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = T.Group
            };
        }

        private ListDefinition GetProjectsList()
        {
            return new ListDefinition
            {
                
                Title = T.ProjectsListTitleName,
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                ContentTypesEnabled = true,
                CustomUrl = T.ProjectsListCustomUrlName
            };
        }

        private ListViewDefinition GetManagedProjectsListView()
        {
            return new ListViewDefinition
            {
                Title = T.ManagedProjectsListViewTitleName,
                Query = CreateQuery(),
                Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.Title,
                    T.ProjectStatusFieldTitleInternalName,
                    T.StartDateFieldInternalName,
                    T.EndDateFieldInternalName,
                    T.ProjectManagerFieldInternalName,
                    T.TeamFieldInternalName,
                    T.DepartmentFieldInternalName,
                    T.ProjectValueFieldInternalName
                }
            };
        }

        private string CreateQuery()
        {
            var createdQuery = new StringBuilder();
            createdQuery.Append("<Where>");
            createdQuery.Append("<Eq>");
            createdQuery.Append($"<FieldRef Name='{T.ProjectManagerFieldInternalName}'/>");
            createdQuery.Append("<Value Type='Integer'><UserID/></Value>");
            createdQuery.Append("</Eq>");
            createdQuery.Append("</Where>");
            return createdQuery.ToString();
        }
    }
}
