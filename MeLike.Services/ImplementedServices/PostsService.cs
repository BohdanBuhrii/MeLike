using AutoMapper;
using MeLike.Data.Entities;
//using MeLike.Data.Extentions;
using MeLike.Data.Interfaces;
using MeLike.Services.Interfaces;
using MeLike.Services.ViewModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeLike.Services.ImplementedServices
{
    public class PostsService : IPostsService
    {
        private readonly IMeLikeContext _context;
        private readonly IMapper _mapper;

        public PostsService(IMeLikeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostViewModel>> GetAllPosts() 
        {
            var posts = await _context.Posts.AsQueryable().ToListAsync();

            return posts.Select(p => _mapper.Map<PostViewModel>(p));
        }

        public Task Create(PostViewModel post)
        {
            var entity = _mapper.Map<Post>(post);
            entity.PublishDate = DateTime.Now;

            return _context.Posts.InsertOneAsync(entity);
        }            
    }
}
