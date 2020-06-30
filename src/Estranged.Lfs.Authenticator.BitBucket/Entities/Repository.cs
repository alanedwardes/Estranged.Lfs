using System;
using System.Runtime.Serialization;

namespace Estranged.Lfs.Authenticator.BitBucket.Entities
{
    [DataContract]
    public sealed class Repository
    {
        [DataMember(Name = "scm")]
        public string Scm { get; set; }

        [DataMember(Name = "website")]
        public string Website { get; set; }

        [DataMember(Name = "has_wiki")]
        public bool HasWiki { get; set; }

        [DataMember(Name = "uuid")]
        public string Uuid { get; set; }

        [DataMember(Name = "links")]
        public RepositoryLinks Links { get; set; }

        [DataMember(Name = "fork_policy")]
        public string ForkPolicy { get; set; }

        [DataMember(Name = "full_name")]
        public string FullName { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "project")]
        public Project Project { get; set; }

        [DataMember(Name = "language")]
        public string Language { get; set; }

        [DataMember(Name = "created_on")]
        public DateTimeOffset CreatedOn { get; set; }

        [DataMember(Name = "mainbranch")]
        public Project Mainbranch { get; set; }

        [DataMember(Name = "workspace")]
        public Workspace Workspace { get; set; }

        [DataMember(Name = "has_issues")]
        public bool HasIssues { get; set; }

        [DataMember(Name = "owner")]
        public Owner Owner { get; set; }

        [DataMember(Name = "updated_on")]
        public DateTimeOffset UpdatedOn { get; set; }

        [DataMember(Name = "size")]
        public int Size { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "slug")]
        public string Slug { get; set; }

        [DataMember(Name = "is_private")]
        public bool IsPrivate { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}