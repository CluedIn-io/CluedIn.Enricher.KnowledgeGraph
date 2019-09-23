// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KnowledgeGraphExternalSearchProvider.Logging.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the KnowledgeGraphExternalSearchProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core;
using CluedIn.Core.Data.Relational;
using CluedIn.ExternalSearch.DataStore;
using CluedIn.ExternalSearch.Providers.KnowledgeGraph.Model;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph
{
    public partial class KnowledgeGraphExternalSearchProvider
    {
        public void LogResult(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result)
        {
            using (var systemContext = context.ApplicationContext.System.CreateExecutionContext())
            {
                var dataStore   = systemContext.Organization.DataStores.GetDataStore<ExternalSearchGoogleKnowledgeGraphLogRecord>();
                var resultItem  = result.As<Result>();

                if (resultItem.Data.id == null && resultItem.Data.name == null && resultItem.Data.description == null)
                    return;

                var record      = this.CreateRecord(context, query, result, resultItem.Data);

                dataStore.InsertOrUpdate(systemContext, record);
            }
        }

        private ExternalSearchGoogleKnowledgeGraphLogRecord CreateRecord(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, Result resultItem)
        {
            var record = new ExternalSearchGoogleKnowledgeGraphLogRecord {
                Id = ExternalSearchLogIdGenerator.GenerateId(query.ProviderId, query.EntityType,
                    resultItem.id ?? string.Empty, resultItem.name ?? string.Empty,
                    resultItem.description ?? string.Empty),
                ProviderId = query.ProviderId,
                EntityType = query.EntityType,
                ResultId = resultItem.id,
                Name = resultItem.name,
                ResultTypes = resultItem.type != null ? string.Join(", ", resultItem.type) : null,
                Description = resultItem.description,
                Url = resultItem.url
            };

            if (resultItem.detailedDescription != null)
            {
                record.DetailedDescriptionBody          = resultItem.detailedDescription.articleBody;
                record.DetailedDescriptionUrl           = resultItem.detailedDescription.url;
                record.DetailedDescriptionLicenseUrl    = resultItem.detailedDescription.license;
            }

            return record;
        }
    }
}
