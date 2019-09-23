// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClearBitVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the ClearBitVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph.Vocabularies
{
    /// <summary>The clear bit vocabulary.</summary>
    public static class KnowledgeGraphVocabulary
    {
        /// <summary>
        /// Initializes static members of the <see cref="KnowledgeGraphVocabulary" /> class.
        /// </summary>
        static KnowledgeGraphVocabulary()
        {
            Person = new KnowledgeGraphPersonVocabulary();
            Organization = new KnowledgeGraphOrganizationVocabulary();
        }

        /// <summary>Gets the organization.</summary>
        /// <value>The organization.</value>
        public static KnowledgeGraphPersonVocabulary Person { get; private set; }
        public static KnowledgeGraphOrganizationVocabulary Organization { get; private set; }
    }
}