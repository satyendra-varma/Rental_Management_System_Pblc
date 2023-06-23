namespace Rental_mangement_system.ImageDeleteService
{
    public interface ImageDeleteService
    {
        Task<bool> DeleteImageAsync(String imgName);
    }
}
