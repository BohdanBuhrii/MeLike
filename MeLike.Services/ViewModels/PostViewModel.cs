using MeLike.Data.Entities;
using System;
using System.Collections.Generic;

namespace MeLike.Services.ViewModels
{
    public class PostViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public DateTime PublishDate { get; set; }

        public List<Emotion> Emotions { get; set; }
         
        public List<Comment> Comments { get; set; }

        public int Views { get; set; }
    }
}
