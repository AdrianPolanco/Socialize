

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Socialize.Core.Application.Dtos;
using Socialize.Core.Application.Repositories;
using Socialize.Core.Domain.Entities;
using Socialize.Infrastructure.Identity.Context;
using System.Linq.Expressions;

namespace Socialize.Infrastructure.Identity.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Post> _posts;
        private readonly IMapper _mapper;

        public PostRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _posts = context.Posts;
            _mapper = mapper;
        }

        public async Task<PostsPageDto> GetPosts(GetPostsDto getPostsDto, CancellationToken cancellationToken, bool readOnly, Expression<Func<Post, bool>> filter = null)
        {
            // Definición de la consulta base ordenada por Id
            IQueryable<Post> query = _posts.OrderBy(p => p.Id);
            int takeAmount = getPostsDto.PageSize + 1; // Cantidad de posts a tomar más uno

            // Aplicar cursor si está presente
            if (getPostsDto.Cursor is not null)
            {
                if (getPostsDto.IsNextPage)
                {
                    // Para la siguiente página, obtener posts con Id mayor al cursor
                    query = query.Where(p => p.Id > getPostsDto.Cursor);
                }
                else
                {
                    // Para la página anterior, obtener posts con Id menor al cursor y ordenar descendentemente
                    query = query.Where(p => p.Id < getPostsDto.Cursor)
                                 .OrderByDescending(p => p.Id);

                    takeAmount = getPostsDto.PageSize; // No necesitamos el extra para la página anterior
                }
            }

            if(filter is not null)
            {
                // Aplicar filtro si está presente
                query = query.Where(filter);
            }

            // Tomar la cantidad específica de posts
            query = query.Take(takeAmount);

            // Incluir entidades relacionadas (User y Comments)
            query = query.Include(p => p.User).Include(p => p.Comments).Include(p => p.Attachment);

            // Aplicar AsNoTracking si se solicita solo lectura
            if (readOnly)
            {
                query = query.AsNoTracking();
            }

            // Obtener la lista de posts
            List<Post> posts = await query.ToListAsync(cancellationToken);

            // Mapear los posts a DTOs usando AutoMapper
            List<PostDto> postsDto = _mapper.Map<List<PostDto>>(posts);

            // Determinar si es la primera página
            bool isFirstPage = !getPostsDto.Cursor.HasValue
                || (getPostsDto.Cursor.HasValue && postsDto.First().Id == _posts.OrderBy(p => p.Id).First().Id);

            // Determinar si hay página siguiente
            bool hasNextPage = postsDto.Count > getPostsDto.PageSize
                || (getPostsDto.Cursor is not null && getPostsDto.IsNextPage is false);

            // Ajustar la lista de posts DTOs si se excedió el tamaño de página
            if (postsDto.Count > getPostsDto.PageSize)
            {
                postsDto.RemoveAt(postsDto.Count - 1);
            }

            // Obtener el Id del siguiente post si existe
            Guid? nextId = hasNextPage ? postsDto.Last().Id : null;

            // Obtener el Id del post anterior si existe
            Guid? previousId = postsDto.Count > 0 && !isFirstPage ? postsDto.First().Id : null;

            // Retornar un objeto PostsPageDto con la lista de posts DTOs y la información de paginación
            return new PostsPageDto(postsDto, nextId, previousId, isFirstPage);
        }
    }
}
