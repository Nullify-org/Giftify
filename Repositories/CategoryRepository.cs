<<<<<<< HEAD
﻿using Giftify.Data;
using Giftify.Interfaces.Repositories;
using Giftify.Models;
using Microsoft.Identity.Client;

namespace Giftify.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {

        }
=======
﻿using Giftify.Interfaces.Repositories;

namespace Giftify.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
>>>>>>> 1e4876017e0475229b7cb5fd3cb178f81b5991d5
    }
}
