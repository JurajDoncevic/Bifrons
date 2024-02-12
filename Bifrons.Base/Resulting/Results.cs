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
    public static Result<TData> Success<TData>(TData? data, string message = "Operation successful")
        => new(data, ResultTypes.SUCCESS, message, null);

    /// <summary>
    /// Creates a failure operation result
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="message">Outcome message</param>
    /// <returns></returns>
    public static Result<TData> Failure<TData>(string? message = null)
        => new(default, ResultTypes.FAILURE, message); // default give null for ref types

    /// <summary>
    /// Creates an exception operation result
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="exception">The thrown exception</param>
    /// <param name="message">Outcome message</param>
    /// <returns></returns>
    public static Result<TData> Exception<TData>(Exception exception, string? message = null)
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
            return Exception<T>(e);
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
            return Exception<T>(e);
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
    /// <exception cref="System.Exception">Only if an unknown result type appears</exception>
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
    /// <exception cref="System.Exception">Only if an unknown result type appears</exception>
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
            onSuccess: data => Results.Success(map(data)),
            onFailure: message => Results.Failure<R>(message),
            onException: exception => Results.Exception<R>(exception)
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
            onFailure: message => Results.Failure<R>(message),
            onException: exception => Results.Exception<R>(exception)
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

    public static Result<IEnumerable<T>> Unfold<T>(this IEnumerable<Result<T>> results)
    {
        var data = new List<T>();
        var messages = new List<string>();
        var exceptions = new List<Exception>();

        foreach (var result in results)
        {
            switch (result.ResultType)
            {
                case ResultTypes.SUCCESS:
                    data.Add(result.Data);
                    break;
                case ResultTypes.FAILURE:
                    messages.Add(result.Message);
                    break;
                case ResultTypes.EXCEPTION:
                    exceptions.Add(result.Exception);
                    break;
                default:
                    throw new Exception("Unknown result type");
            }
        }

        if (exceptions.Count > 0)
            return Results.Exception<IEnumerable<T>>(new AggregateException(exceptions), string.Join("\n", messages));
        if (messages.Count > 0)
            return Results.Failure<IEnumerable<T>>(string.Join("\n", messages));
        return Results.Success<IEnumerable<T>>(data);
    }

    public static Result<Either<TLeft, TRight>> Unfold<TLeft, TRight>(this Either<Result<TLeft>, Result<TRight>> target)
        => target.Match(
            left => left.Match(
                value => Results.Success(Either.Left<TLeft, TRight>(value)),
                _ => Results.Failure<Either<TLeft, TRight>>("Left result is a failure"),
                ex => Results.Exception<Either<TLeft, TRight>>(ex, "Left result is an exception:")
            ),
            right => right.Match(
                value => Results.Success(Either.Right<TLeft, TRight>(value)),
                _ => Results.Failure<Either<TLeft, TRight>>("Right result is a failure"),
                ex => Results.Exception<Either<TLeft, TRight>>(ex, "Right result is an exception")
            )
        );
}