using Giftify.Data;
using Giftify.Interfaces.Repositories;
using Giftify.Models;

namespace Giftify.Repositories;

public interface IOccasionRepository : IBaseRepository<Occasion> { }

public class OccasionRepository : BaseRepository<Occasion>, IOccasionRepository
{
    public OccasionRepository(ApplicationDbContext context) : base(context) { }
}
