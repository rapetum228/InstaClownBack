using System.Runtime.Serialization;

namespace Api.Exceptions
{
    [Serializable]
    public class TypeMismatchException : Exception
    {

        public TypeMismatchException(string? message) : base(message)
        {
        }

    }
}