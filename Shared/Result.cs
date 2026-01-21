namespace Shared
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public T? Value { get; }
        public Failure? Failure { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        private Result(Failure failure)
        {
            IsSuccess = false;
            Failure = failure;
        }

        public static Result<T> Success(T value) => new(value);

        public static Result<T> Fail(Failure failure) => new(failure);
    }
}
