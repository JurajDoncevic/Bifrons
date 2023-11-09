namespace Bifrons.Lenses;

/// <summary>
/// Base class for monadic asymmetric lenses.
/// </summary>
/// <typeparam name="TSource">Type of source</typeparam>
/// <typeparam name="TView">Type of view</typeparam>
public abstract class BaseAsymmetricLens<TSource, TView>
{

    protected BaseAsymmetricLens()
    {
    }

    /// <summary>
    /// For a given updated view and original source returns an updated source
    /// </summary>
    public abstract Func<TView, Option<TSource>, Result<TSource>> Put { get; }

    /// <summary>
    /// For a given source returns a view
    /// </summary>
    public abstract Func<TSource, Result<TView>> Get { get; }

    /// <summary>
    /// For a given view returns a source; no original source is provided
    /// </summary>
    public abstract Func<TView, Result<TSource>> Create { get; }

    /// <summary>
    /// For a given source returns a view
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Result<TView> CallGet(TSource source) => Get(source);

    /// <summary>
    /// For a given updated view and original source returns an updated source
    /// </summary>
    /// <param name="updatedView"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public Result<TSource> CallPut(TView updatedView, Option<TSource> source) => Put(updatedView, source);

    /// <summary>
    /// For a given source returns a view; no original source is provided
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    public Result<TSource> CallCreate(TView view) => Create(view);

    public override bool Equals(object? obj)
    {
        return obj is not null && 
               obj is BaseAsymmetricLens<TSource, TView> lens &&
               Get.Equals(lens.Get) &&
               Put.Equals(lens.Put) &&
               Create.Equals(lens.Create);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Get, Put, Create);
    }

    public static bool operator ==(BaseAsymmetricLens<TSource, TView> left, BaseAsymmetricLens<TSource, TView> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(BaseAsymmetricLens<TSource, TView> left, BaseAsymmetricLens<TSource, TView> right)
    {
        return !(left == right);
    }
}
