// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClearBitOrganizationVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the ClearBitOrganizationVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph.Vocabularies
{
    /// <summary>The clear bit organization vocabulary.</summary>
    /// <seealso cref="CluedIn.Core.Data.Vocabularies.SimpleVocabulary" />
    public class KnowledgeGraphPersonVocabulary : SimpleVocabulary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgeGraphPersonVocabulary"/> class.
        /// </summary>
        public KnowledgeGraphPersonVocabulary()
        {
            this.VocabularyName = "Knowledge Graph Person";
            this.KeyPrefix = "knowledgeGraph.person";
            this.KeySeparator   = ".";
            this.Grouping       = EntityType.Person;

            this.GooglePlusId = this.Add(new VocabularyKey("googlePlusId"));
            this.ImageUrl = this.Add(new VocabularyKey("domain", VocabularyKeyDataType.Uri));
            this.Description = this.Add(new VocabularyKey("description", VocabularyKeyDataType.Text));

            this.AddMapping(this.GooglePlusId, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.SocialGooglePlus);
        }

        public VocabularyKey GooglePlusId { get; protected set; }
        public VocabularyKey ImageUrl { get; protected set; }
        public VocabularyKey Description { get; set; }
    }
}
