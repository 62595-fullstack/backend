using Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models.Attachment;
using Models.Organization;

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

		public async Task<bool> DeleteAttachment(int attachmentId)
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					var user = await db.Attachment.Where(x => x.Id == attachmentId).ExecuteDeleteAsync();
					if (user == 0)
					{
						return false;
					}
					await db.SaveChangesAsync();
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public async Task<bool> AddOrganizationCoverPhoto(AttachmentDto attachmentDto, int organizationId)
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					Attachments am = new Attachments
					{
						FileName = attachmentDto.FileName,
						FileType = Path.GetExtension(attachmentDto.FileName),
						Content = attachmentDto.Content,
					};
					Organizations organization = await db.Organization.Where(o => o.Id == organizationId).FirstAsync();
					organization.CoverPhotoId = am.Id;
					await db.Attachment.AddAsync(am);
					await db.SaveChangesAsync();
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public async Task<bool> AddOrganizationProfilePicture(AttachmentDto attachmentDto, int organizationId)
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					Attachments am = new Attachments
					{
						FileName = attachmentDto.FileName,
						FileType = Path.GetExtension(attachmentDto.FileName),
						Content = attachmentDto.Content,
					};
					Organizations organization = await db.Organization.Where(o => o.Id == organizationId).FirstAsync();
					organization.ProfilePictureId = am.Id;
					await db.Attachment.AddAsync(am);
					await db.SaveChangesAsync();
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

	}
}