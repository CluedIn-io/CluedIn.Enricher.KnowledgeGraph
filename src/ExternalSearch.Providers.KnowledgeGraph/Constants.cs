﻿using System;
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

        public static string About { get; set; } = "Knowledge Graph is an enricher which allows you to find entities using the Google Knowledge Graph API";
        public static string Icon { get; set; } = "Resources.knowledge_graph.svg";
        public static string Domain { get; set; } = "https://developers.google.com/knowledge-graph";

        public static AuthMethods AuthMethods { get; set; } = new AuthMethods
        {
            token = new List<Control>()
            {
                
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
