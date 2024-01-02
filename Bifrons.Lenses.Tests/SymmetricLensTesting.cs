using Bifrons.Base;
using Bifrons.Lenses.Symmetric;

namespace Bifrons.Lenses.Tests;

/// <summary>
/// Test declarations for SIMPLE symmetric lenses
/// </summary>
public abstract class ISymmetricLensTesting
{
    /// <summary>
    /// Should confirm that putL x (createR x) = x
    /// </summary>
    [Fact(DisplayName = $"CREATE_PUT_RL rule test")]
    public abstract void CreatePutRLTest();

    /// <summary>
    /// Should confirm that putR y (createL y) = y
    /// </summary>
    [Fact(DisplayName = $"CREATE_PUT_LR rule test")]
    public abstract void CreatePutLRTest();

    /// <summary>
    /// Should confirm that putL x (putR y x) = x
    /// </summary>
    [Fact(DisplayName = $"PUT_RL rule test")]
    public abstract void PutRLTest();

    /// <summary>
    /// Should confirm that putR y (putL x y) = y
    /// </summary>
    [Fact(DisplayName = $"PUT_LR rule test")]
    public abstract void PutLRTest();
}

/// <summary>
/// Prewritten tests for SIMPLE symmetric lenses
/// </summary>
/// <typeparam name="TLeft"></typeparam>
/// <typeparam name="TRight"></typeparam>
public abstract class SymmetricLensTestingFramework<TLeft, TRight> : ISymmetricLensTesting
{
    protected abstract TLeft _left { get; }
    protected abstract TRight _right { get; }

    protected abstract BaseSymmetricLens<TLeft, TRight> _lens { get; }

    public override void CreatePutLRTest()
    {
        var result =
        _lens.CreateLeft(_right)
            .Bind(left => _lens.PutRight(left, Option.Some(_right)));

        Assert.True(result);
        Assert.Equal(_right, result.Data);
    }

    public override void CreatePutRLTest()
    {
        var result =
        _lens.CreateRight(_left)
            .Bind(right => _lens.PutLeft(right, Option.Some(_left)));

        Assert.True(result);
        Assert.Equal(_left, result.Data);
    }

    public override void PutLRTest()
    {
        var result =
        _lens.PutLeft(_right, Option.Some(_left))
            .Bind(left => _lens.PutRight(left, Option.Some(_right)));

        Assert.True(result);
        Assert.Equal(_right, result.Data);
    }

    public override void PutRLTest()
    {
        var result =
        _lens.PutRight(_left, Option.Some(_right))
            .Bind(right => _lens.PutLeft(right, Option.Some(_left)));

        Assert.True(result);
        Assert.Equal(_left, result.Data);
    }

}