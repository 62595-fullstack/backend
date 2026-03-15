using Microsoft.EntityFrameworkCore;
using Models.Attachment;
using Models.User;

namespace backend.getdata
{
    public class DataAttachment
    {
        public async Task<Attachments?> GetAttachment(int attachmentId)
        {
            try
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    return await db.Attachment.Where(a => a.Id == attachmentId).FirstAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> SaveFileToPost(IFormFile file, int postId)
        {
            try
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    using (var stream = file.OpenReadStream())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            byte[] fileBytes = memoryStream.ToArray();
                            Attachments am = new Attachments
                            {
                                FileName = file.FileName,
                                FileType = file.GetType().ToString(),
                                Content = fileBytes,
                                CreatedDate = DateTime.UtcNow,
                                PostId = postId,
                            };
                            await db.Attachment.AddAsync(am);
                            await db.SaveChangesAsync();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

