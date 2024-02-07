// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KnowledgeGraphOrganizationVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the KnowledgeGraphOrganizationVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph.Vocabularies
{
    /// <summary>The knowledge graph organization vocabulary.</summary>
    /// <seealso cref="CluedIn.Core.Data.Vocabularies.SimpleVocabulary" />
    public class KnowledgeGraphOrganizationVocabulary : SimpleVocabulary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgeGraphOrganizationVocabulary"/> class.
        /// </summary>
        public KnowledgeGraphOrganizationVocabulary()
        {
            this.VocabularyName = "Knowledge Graph Organization";
            this.KeyPrefix      = "knowledgeGraph.organization";
            this.KeySeparator   = ".";
            this.Grouping       = EntityType.Organization;

            this.Url                            = this.Add(new VocabularyKey("url",                         VocabularyKeyDataType.Uri));
            this.Description                    = this.Add(new VocabularyKey("description"));
            this.DetailedDescriptionBody        = this.Add(new VocabularyKey("detailedDescriptionBody"));
            this.DetailedDescriptionLicense     = this.Add(new VocabularyKey("detailedDescriptionLicense"));
            this.DetailedDescriptionUrl         = this.Add(new VocabularyKey("detailedDescriptionUrl"));

        }

        public VocabularyKey Url { get; protected set; }
        public VocabularyKey Description { get; set; }
        public VocabularyKey DetailedDescriptionBody { get; set; }
        public VocabularyKey DetailedDescriptionLicense { get; set; }
        public VocabularyKey DetailedDescriptionUrl { get; set; }
    }
}
