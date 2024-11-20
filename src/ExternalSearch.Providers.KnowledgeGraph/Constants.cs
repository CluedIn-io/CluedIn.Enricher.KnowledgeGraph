using System;
using System.Collections.Generic;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph
{
    public static class Constants
    {
        public const string ComponentName = "KnowledgeGraph";
        public const string ProviderName = "Knowledge Graph";
        public static readonly Guid ProviderId = Core.Constants.ExternalSearchProviders.GoogleKnowledgeGraphId;
        public struct KeyName
        {
            public const string ApiKey = "apiKey";
            public const string AcceptedEntityType = "acceptedEntityType";
            public const string OrganizationNameKey = "organizationNameKey";
            public const string WebsiteKey = "websiteKey";
        }
        public static string About { get; set; } = "Knowledge Graph is an enricher which allows you to find entities using the Google Knowledge Graph API";
        public static string Icon { get; set; } = "Resources.knowledge_graph.svg";
        public static string Domain { get; set; } = "https://developers.google.com/knowledge-graph";

        public static AuthMethods AuthMethods { get; set; } = new AuthMethods
        {
            Token = new List<Control>()
            {
                new Control()
                {
                    DisplayName = "Key",
                    Type = "input",
                    IsRequired = true,
                    Name = KeyName.ApiKey,
                    Help = "The key to authenticate access to the Google's Knowledge Graph API."
                },
                new Control()
                {
                    DisplayName = "Accepted Entity Type",
                    Type = "input",
                    IsRequired = true,
                    Name = KeyName.AcceptedEntityType,
                    Help = "The entity type that defines the golden records you want to enrich (e.g., /Organization)."
                },
                new Control()
                {
                    DisplayName = "Organization Name Vocabulary Key",
                    Type = "input",
                    IsRequired = false,
                    Name = KeyName.OrganizationNameKey,
                    Help = "The vocabulary key that contains the names of companies you want to enrich (e.g., organization.name)."
                },
                new Control()
                {
                    DisplayName = "Website Vocabulary Key",
                    Type = "input",
                    IsRequired = false,
                    Name = KeyName.WebsiteKey,
                    Help = "The vocabulary key that contains the websites of companies you want to enrich (e.g., organization.website)."
                }
            }
        };

        public static IEnumerable<Control> Properties { get; set; } = new List<Control>()
        {
            // NOTE: Leaving this commented as an example - BF
            //new()
            //{
            //    displayName = "Some Data",
            //    type = "input",
            //    isRequired = true,
            //    name = "someData"
            //}
        };

        public static Guide Guide { get; set; } = null;
        public static IntegrationType IntegrationType { get; set; } = IntegrationType.Enrichment;
    }
}
