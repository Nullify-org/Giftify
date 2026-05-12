using Giftify.Interfaces;
using Giftify.Interfaces.Services;

namespace Giftify.Services;

public class OccasionService : IOccasionService
{
    private readonly IUnitOfWork _unitOfWork;

    public OccasionService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }
}
