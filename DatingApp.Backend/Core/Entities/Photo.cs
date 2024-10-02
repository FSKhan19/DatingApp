using DatingApp.Backend.Core.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Backend.Core.Entities
{
    [Table("Photos")]
    public class Photo: CreationAuditedEntity<int>
    {
        public string? Url { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}
