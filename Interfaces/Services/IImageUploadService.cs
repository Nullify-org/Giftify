namespace Giftify.Interfaces.Services;

public interface IImageUploadService
{

        
        Task<string?> UploadAsync(IFormFile? file);
        void Delete(string? relativeUrl);
    }


