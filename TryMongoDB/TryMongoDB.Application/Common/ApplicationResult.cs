namespace TryMongoDB.Application.Common;

public sealed class ApplicationResult<T>
{
    private ApplicationResult(T? value, bool succeeded, ApplicationErrorType? errorType, string? errorMessage, int statusCode)
    {
        Value = value;
        Succeeded = succeeded;
        ErrorType = errorType;
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
    }

    public T? Value { get; }
    public bool Succeeded { get; }
    public ApplicationErrorType? ErrorType { get; }
    public string? ErrorMessage { get; }
    public int StatusCode { get; }

    public static ApplicationResult<T> Success(T value, int statusCode = 200) =>
        new(value, true, null, null, statusCode);

    public static ApplicationResult<T> Failure(ApplicationErrorType errorType, string errorMessage) =>
        new(default, false, errorType, errorMessage, GetStatusCode(errorType));

    private static int GetStatusCode(ApplicationErrorType errorType) =>
        errorType switch
        {
            ApplicationErrorType.Validation => 400,
            ApplicationErrorType.NotFound => 404,
            ApplicationErrorType.Conflict => 409,
            ApplicationErrorType.Unexpected => 500,
            _ => 400
        };
}
