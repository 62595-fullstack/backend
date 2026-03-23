using Models.Post;

namespace Models.Attachment;

public class Attachments
{
    public int Id { get; set; }

    public required string FileName { get; set; }
    public required string FileType { get; set; }

    public required byte[] Content { get; set; } = null!;

    public required DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public int? PostId { get; set; }
    public Posts? Post { get; set; } = null!;
}
