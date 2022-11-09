﻿using System.Runtime.Serialization;

namespace Api.Exceptions
{
    [Serializable]
    internal class TypeMismatchException : Exception
    {
        public TypeMismatchException()
        {
        }

        public TypeMismatchException(string? message) : base(message)
        {
        }

        public TypeMismatchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TypeMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}