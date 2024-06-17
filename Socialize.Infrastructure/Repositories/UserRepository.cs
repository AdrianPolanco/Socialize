using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Socialize.Core.Domain.Entities;
using Socialize.Core.Domain.Repositories.Base;
using Socialize.Infrastructure.Identity.Context;
using Socialize.Infrastructure.Identity.Models;
using Socialize.Infrastructure.Identity.Repositories.Base;

namespace Socialize.Infrastructure.Identity.Repositories
{
    public class UserRepository : Repository<User>,IRepository<User>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public UserRepository(IDbContextFactory<ApplicationDbContext> contextFactory, UserManager<ApplicationUser> userManager, IMapper mapper) : base(contextFactory)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public override async Task<User> CreateAsync(User entity, CancellationToken cancellationToken)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(entity);
            user.Id = Guid.NewGuid().ToString();
            await _userManager.CreateAsync(user, entity.Password);
            ApplicationUser createdUser = await _userManager.FindByNameAsync(entity.Username);
            User domainUser = _mapper.Map<User>(createdUser);
            domainUser.Id = Guid.Parse(createdUser.Id);
            
            return await base.CreateAsync(domainUser, cancellationToken);
        }
    }
}
