using AutoMapper;
using MeLike.Data.Entities;
using MeLike.Data.Enums;
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
        private readonly IMongoQueryable<Post> _posts;
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;

        public PostsService(IMeLikeContext context, IMapper mapper, IUsersService usersService)
        {
            _context = context;
            _mapper = mapper;
            _usersService = usersService;
            _posts = _context.Posts.AsQueryable().OrderByDescending(p => p.PublishDate);
        }

        public async Task<PostViewModel> GetPostById(string postId)
        {
            var post = await _posts.SingleOrDefaultAsync(p => p.Id == postId);
            await UpdateViews(post);

            return _mapper.Map<PostViewModel>(post);
        }

        public async Task<IEnumerable<PostViewModel>> GetAllPosts(PageViewModel page)
        {
            var posts = await _posts
                .Skip(page.Number * page.Size)
                .Take(page.Size)
                .ToListAsync();

            await UpdateViews(posts);

            return _mapper.Map<IEnumerable<PostViewModel>>(posts);
        }

        public async Task<IEnumerable<PostViewModel>> GetPostsByUserLogin(string userLogin, PageViewModel page)
        {
            var posts = await _posts
                .Where(p => p.Author == userLogin)
                .Skip(page.Number * page.Size)
                .Take(page.Size)
                .ToListAsync();

            await UpdateViews(posts);

            return _mapper.Map<IEnumerable<PostViewModel>>(posts);
        }

        public async Task<IEnumerable<PostViewModel>> GetPostsByUserFriends(PageViewModel page)
        {
            var posts = await _posts
                .Where(p => _usersService.User.Friends.Contains(p.Author))
                .Skip(page.Number * page.Size)
                .Take(page.Size)
                .ToListAsync();

            await UpdateViews(posts);

            return _mapper.Map<IEnumerable<PostViewModel>>(posts);
        }

        public async Task<PostViewModel> FetchChanges(PostViewModel post)
        {
            return _mapper.Map(await _posts.SingleOrDefaultAsync(p => p.Id == post.Id), post);
        }

        public Task Create(PostViewModel post)
        {
            if (string.IsNullOrWhiteSpace(post.Text)) {
                return Task.FromResult(1);
            }

            var entity = _mapper.Map<Post>(post);
            entity.PublishDate = DateTime.Now;
            entity.Author = _usersService.User.Login;
            entity.Emotions = new List<Emotion>();
            entity.Comments = new List<Comment>();

            return _context.Posts.InsertOneAsync(entity);
        }

        public async Task AddEmotion(PostViewModel post, EmotionType emotionType)
        {
            var emotion = new Emotion { Author = _usersService.User.Login, Type = emotionType };
            
            var setter =
                post.Emotions.Any(e => e.Author == emotion.Author && e.Type == emotion.Type)
                    ? Builders<Post>.Update.Pull(p => p.Emotions, emotion)
                    : Builders<Post>.Update.Push(el => el.Emotions, emotion);

            await _context.Posts.UpdateOneAsync(el => el.Id == post.Id, setter);
            await FetchChanges(post);
        }

        public async Task AddComment(PostViewModel post, string text)
        {
            var comment = new Comment { Author = _usersService.User.Login, Text = text };

            var setter = Builders<Post>.Update.Push(el => el.Comments, comment);

            await _context.Posts.UpdateOneAsync(el => el.Id == post.Id, setter);
            await FetchChanges(post);
        }

        private async Task UpdateViews(IEnumerable<Post> posts)
        {
            foreach (var post in posts)
            {
                await UpdateViews(post);
            }
        }

        private Task UpdateViews(Post post)
        {
            post.Views += 1;
            return _context.Posts.UpdateOneAsync(
                p => p.Id == post.Id,
                Builders<Post>.Update.Set(p => p.Views, post.Views));
        }
    }
}