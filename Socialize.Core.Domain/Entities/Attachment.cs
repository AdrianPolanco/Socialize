

using Socialize.Core.Domain.Entities.Base;
using Socialize.Core.Domain.Enums;

namespace Socialize.Core.Domain.Entities
{
    public class Attachment: Entity
    {
        public string Url { get; set; }
        public AttachmentTypes Type { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
