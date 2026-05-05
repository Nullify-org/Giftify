using Giftify.Interfaces.Repositories;

namespace Giftify.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
}
