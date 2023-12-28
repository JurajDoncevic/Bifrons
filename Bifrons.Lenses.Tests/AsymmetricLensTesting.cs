namespace Bifrons.Lenses.Tests;

public abstract class AsymmetricLensTesting
{
    /// <summary>
    /// Should confirm that get(put v s) = v
    /// </summary>
    [Fact(DisplayName = $"PUTGET rule test")]
    public abstract void PutGetTest();

    /// <summary>
    /// Should confirm that put(get s) s = s
    /// </summary>
    [Fact(DisplayName = $"GETPUT rule test")]
    public abstract void GetPutTest();

    /// <summary>
    /// Should confirm that get(create v) = v
    /// </summary>
    [Fact(DisplayName = $"CREATEGET rule test")]
    public abstract void CreateGetTest();

    /// <summary>
    /// Should confirm that put v (put v s) = put v s
    /// </summary>
    [Fact(DisplayName = $"PUTTWICE rule test")]
    public abstract void PutTwiceTest();

    /// <summary>
    /// Should confirm that put v' (put v s) = put v' s
    /// </summary>
    [Fact(DisplayName = $"PUTPUT rule test")]
    public abstract void PutPutTest();

}


/// <summary>
/// Prewritten tests for SIMPLE symmetric lenses
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TView"></typeparam>
public abstract class AsymmetricLensTestingFramework<TSource, TView> : AsymmetricLensTesting
{
    protected abstract TSource _source { get; }
    protected abstract TView _view { get; }
    protected abstract TView _updatedView { get; }
    protected abstract TSource _updatedSource { get; }

    protected abstract BaseAsymmetricLens<TSource, TView> _lens { get; }

    public override void PutGetTest()
    {
        var result = _lens
            .Put(_view, _source)
            .Bind(_lens.Get);

        Assert.True(result);
        Assert.Equal(_view, result.Data);
    }

    public override void GetPutTest()
    {
        var result = _lens
            .Get(_source)
            .Bind(view => _lens.Put(view, _source));

        Assert.True(result);
        Assert.Equal(_source, result.Data);
    }

    public override void CreateGetTest()
    {
        var result = _lens
            .Create(_view)
            .Bind(_lens.Get);

        Assert.True(result);
        Assert.Equal(_view, result.Data);
    }

    public override void PutTwiceTest()
    {
        var resultLeft = _lens
            .Put(_view, _source)
            .Bind(_ => _lens.Put(_view, _source));

        var resultRight = _lens
            .Put(_view, _source);

        Assert.True(resultRight);
        Assert.True(resultLeft);
        Assert.Equal(resultLeft, resultRight);
    }

    public override void PutPutTest()
    {
        var resultLeft = _lens
            .Put(_view, _source)
            .Bind(_ => _lens.Put(_updatedView, _source));

        var resultRight = _lens
            .Put(_updatedView, _source);

        Assert.True(resultRight);
        Assert.True(resultLeft);
        Assert.Equal(resultLeft, resultRight);
    }

}