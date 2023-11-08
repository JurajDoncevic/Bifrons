namespace Bifrons.Base;

public static class Results
{
    #region LIFTING
    /// <summary>
    /// Creates a successful operation result
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="data">Result data</param>
    /// <param name="message">Outcome message</param>
    /// <returns></returns>
    public static Result<TData> OnSuccess<TData>(TData? data, string message = "Operation successful")
        => new(data, ResultTypes.SUCCESS, message, null);

    /// <summary>
    /// Creates a failure operation result
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="message">Outcome message</param>
    /// <returns></returns>
    public static Result<TData> OnFailure<TData>(string? message = null)
        => new(default, ResultTypes.FAILURE, message); // default give null for ref types

    /// <summary>
    /// Creates an exception operation result
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="exception">The thrown exception</param>
    /// <param name="message">Outcome message</param>
    /// <returns></returns>
    public static Result<TData> OnException<TData>(Exception exception, string? message = null)
    {
        exception ??= new Exception("Unknown exception");
        return new Result<TData>(default, ResultTypes.EXCEPTION, message, exception);
    }

    /// <summary>
    /// Creates a result from an operation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="operation"></param>
    /// <returns></returns>
    public static Result<T> AsResult<T>(Func<Result<T>> operation)
    {
        try
        {
            return operation();
        }
        catch (Exception e)
        {
            return OnException<T>(e);
        }
    }

    /// <summary>
    /// Creates an async result from an operation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="operation"></param>
    /// <returns></returns>
    public static async Task<Result<T>> AsResult<T>(Func<Task<Result<T>>> operation)
    {
        try
        {
            return await operation();
        }
        catch (Exception e)
        {
            return OnException<T>(e);
        }
    }

    #endregion

    #region MATCHING

    /// <summary>
    /// Matches the result type and convalesces to the appropriate value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="result"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onFailure"></param>
    /// <param name="onException"></param>
    /// <returns></returns>
    /// <exception cref="Exception">Only if an unknown result type appears</exception>
    public static R Match<T, R>(this Result<T> result, Func<T, R> onSuccess, Func<string, R> onFailure, Func<Exception, R>? onException = null)
        => result.ResultType switch
        {
            ResultTypes.SUCCESS => onSuccess(result.Data),
            ResultTypes.FAILURE => onFailure(result.Message),
            ResultTypes.EXCEPTION => onException != null ? onException(result.Exception) : onFailure(result.Message),
            _ => throw new Exception("Unknown result type")
        };

    /// <summary>
    /// Matches asynchronously the result type and convalesces to the appropriate value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="result"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onFailure"></param>
    /// <param name="onException"></param>
    /// <returns></returns>
    /// <exception cref="Exception">Only if an unknown result type appears</exception>
    public static async Task<R> Match<T, R>(this Task<Result<T>> result, Func<T, R> onSuccess, Func<string, R> onFailure, Func<Exception, R>? onException = null)
        => await result.ContinueWith(t => t.Result.Match(onSuccess, onFailure, onException));

    #endregion

    #region MAPPING
    /// <summary>
    /// Functor-maps the result data to a new value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="result"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static Result<R> Map<T, R>(this Result<T> result, Func<T, R> map)
        => result.Match(
            onSuccess: data => Results.OnSuccess(map(data)),
            onFailure: message => Results.OnFailure<R>(message),
            onException: exception => Results.OnException<R>(exception)
        );

    /// <summary>
    /// Functor-maps asynchronously the result data to a new value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="result"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static async Task<Result<R>> Map<T, R>(this Task<Result<T>> result, Func<T, R> map)
        => await result.ContinueWith(t => t.Result.Map(map));

    #endregion

    #region BINDING

    /// <summary>
    /// Monad-binds the result data to a new result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="result"></param>
    /// <param name="bind"></param>
    /// <returns></returns>
    public static Result<R> Bind<T, R>(this Result<T> result, Func<T, Result<R>> bind)
        => result.Match(
            onSuccess: data => bind(data),
            onFailure: message => Results.OnFailure<R>(message),
            onException: exception => Results.OnException<R>(exception)
        );

    /// <summary>
    /// Monad-binds asynchronously the result data to a new result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="result"></param>
    /// <param name="bind"></param>
    /// <returns></returns>
    public static async Task<Result<R>> Bind<T, R>(this Task<Result<T>> result, Func<T, Result<R>> bind)
        => await result.ContinueWith(t => t.Result.Bind(bind));

    #endregion

    public static Option<T> ToOption<T>(this Result<T> result)
        => result.Match(
            value => Option.Some(value),
            _ => Option.None<T>(),
            _ => Option.None<T>()
        );
}