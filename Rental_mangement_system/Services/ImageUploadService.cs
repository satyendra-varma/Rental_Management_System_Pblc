

namespace Rental_mangement_system.ImageUploadService
{
    public interface ImageUploadService
    {
        Task<string> UploadImageAsync(IFormFile image);
    }
}
