using Giftify.Interfaces;
using Giftify.Interfaces.Services;
using Giftify.ViewModels.Occasions;

namespace Giftify.Services;

public class OccasionService : IOccasionService
{
    private readonly IUnitOfWork _unitOfWork;

    public OccasionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<OccasionVM>> GetAllOccasionsAsync()
    {
        var occasions = await _unitOfWork.Occasions.GetAllAsync();
        return occasions
            .Where(o => o.IsActive)
            .Select(o => new OccasionVM { Id = o.Id, Name = o.Name });
    }
}
