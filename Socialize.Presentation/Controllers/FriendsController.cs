using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.UseCases.ReadPosts;
using Socialize.Core.Domain.Entities;
using Socialize.Infrastructure.Identity.Models;
using Socialize.Presentation.Models.Friendships;
using Socialize.Presentation.Models.Posts;
using System.Linq.Expressions;

namespace Socialize.Presentation.Controllers
{
    public class FriendsController : Controller
    {
        private readonly IEntityService<Friendship> _friendshipService;
        private readonly IEntityService<User> _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FriendsController(IEntityService<Friendship> friendshipService, UserManager<ApplicationUser> userManager, IEntityService<User> userService)
        {
            _friendshipService = friendshipService;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<IActionResult> Index(FriendshipSearchViewModel sentFriendshipSearchViewModel, CancellationToken cancellationToken)
        {
            FriendshipSearchViewModel friendshipSearchViewModel = new FriendshipSearchViewModel();
            Guid currentUserId = Guid.Parse(_userManager.GetUserId(User));

            Expression<Func<Friendship, object>>[] includes = { 
               friendship => friendship.Friend,
               friendship => friendship.User
            };

            Expression<Func<Friendship, bool>> filter = friendship => friendship.UserId == currentUserId || friendship.FriendId == currentUserId;

            ICollection<Friendship> friendsCollection = await _friendshipService.GetByFilter(filter, cancellationToken, true, false, includes);
            List<User> friends = friendsCollection.Select(friendship => friendship.FriendId == currentUserId ? friendship.User  : friendship.Friend).ToList();
            friendshipSearchViewModel.Friends = friends;

            if(!string.IsNullOrWhiteSpace(sentFriendshipSearchViewModel.Username))
            {
                ICollection<User> usersCollection = await _userService.GetByFilter(user => user.Username.Contains(sentFriendshipSearchViewModel.Username) && user.Id != currentUserId, cancellationToken);
                friendshipSearchViewModel.Users = usersCollection.ToList();
                friendshipSearchViewModel.Searched = true;
            }
            return View(friendshipSearchViewModel);
        }

        public async Task<IActionResult> Add(Guid friendId, CancellationToken cancellationToken)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            Guid currentUserId = Guid.Parse(currentUser.Id);

            Friendship friendship = new Friendship
            {
                UserId = currentUserId,
                FriendId = friendId
            };

            Expression<Func<Friendship, bool>> filter = (f => f.FriendId == friendId && f.UserId == currentUserId || f.FriendId == currentUserId && f.UserId == friendId);
            ICollection<Friendship> wereFriendsBefore = await _friendshipService.GetByFilter(filter, cancellationToken, false, true);

            if(wereFriendsBefore.Count == 0) await _friendshipService.AddAsync(friendship, cancellationToken);
            else
            {
                Friendship resendFriendship = wereFriendsBefore.First();
                resendFriendship.Deleted = false;
                await _friendshipService.UpdateAsync(resendFriendship, cancellationToken);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid friendId, CancellationToken cancellationToken)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            Guid currentUserId = Guid.Parse(currentUser.Id);
            Expression<Func<Friendship, bool>> filter = (f => f.FriendId == friendId && f.UserId == currentUserId || f.FriendId == currentUserId && f.UserId == friendId);
            ICollection<Friendship> friendshipCollection = await _friendshipService.GetByFilter(filter, cancellationToken, false);
            Friendship? friendship = friendshipCollection.FirstOrDefault();
            if (friendship is not null) await _friendshipService.DeleteAsync(friendship.Id, cancellationToken);
            return RedirectToAction("Index");
        }
    }
}
