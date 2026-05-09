namespace Giftify.Interfaces.Services;

using Giftify.ViewModels.Occasions;

public interface IOccasionService
{
    Task<IEnumerable<OccasionVM>> GetAllOccasionsAsync();
}
