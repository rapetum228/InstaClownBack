﻿using System.Runtime.Serialization;

namespace Api.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public string? Model { get; set; }

        public override string Message => $"{Model} is not found";
        

    }

    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException()
        {
            Model = "User";
        }

    }
    public class PostNotFoundException : NotFoundException
    {
        public PostNotFoundException()
        {
            Model = "Post";
        }

    }

    public class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException()
        {
            Model = "Comment";
        }

    }
}