namespace Bifrons.Lenses;

public abstract class BaseAsymmetricLens<TSource, TView>
{

    protected BaseAsymmetricLens()
    {
    }

    public abstract Func<TView, Option<TSource>, Result<TSource>> Put { get; }

    public abstract Func<TSource, Result<TView>> Get { get; }

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

    public override bool Equals(object? obj)
    {
        return obj is not null && 
               obj is BaseAsymmetricLens<TSource, TView> lens &&
               Get.Equals(lens.Get) &&
               Put.Equals(lens.Put);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Get, Put);
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
