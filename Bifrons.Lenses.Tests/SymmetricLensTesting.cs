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

    /// <summary>
    /// Should confirm that right-side updates are propagated to the left
    /// </summary>
    [Fact(DisplayName = $"Round-trip with right-side update")]
    public abstract void RoundTrip_WithRightSideUpdate();

    /// <summary>
    /// Should confirm that left-side updates are propagated to the right
    /// </summary>
    [Fact(DisplayName = $"Round-trip with left-side update")]
    public abstract void RoundTrip_WithLeftSideUpdate();
}

/// <summary>
/// Prewritten tests for SIMPLE symmetric lenses
/// </summary>
/// <typeparam name="TLeft"></typeparam>
/// <typeparam name="TRight"></typeparam>
public abstract class SymmetricLensTestingFramework<TLeft, TRight> : ISymmetricLensTesting
{
    /// <summary>
    /// The left-side data for testing rules
    /// </summary>
    protected abstract TLeft _left { get; }
    /// <summary>
    /// The right-side data for testing rules
    /// </summary>
    protected abstract TRight _right { get; }

    /// <summary>
    /// Source and target data for testing round-triping with right-side updates. Tuple elements are in the order of the round-trip.
    /// </summary>
    protected abstract (TLeft originalSource, TRight expectedOriginalTarget, TRight updatedTarget, TLeft expectedUpdatedSource) _roundTripWithRightSideUpdateData { get; }

    /// <summary>
    /// Source and target data for testing round-triping with left-side updates. Tuple elements are in the order of the round-trip.
    /// </summary>
    protected abstract (TRight originalSource, TLeft expectedOriginalTarget, TLeft updatedTarget, TRight expectedUpdatedSource) _roundTripWithLeftSideUpdateData { get; }


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

    public override void RoundTrip_WithRightSideUpdate()
    {
        (TLeft originalSource, TRight expectedOriginalTarget, TRight updatedTarget, TLeft expectedUpdatedSource) = _roundTripWithRightSideUpdateData;

        var createRightResult = _lens.CreateRight(originalSource);
        var putLeftResult = createRightResult.Bind(target => _lens.PutLeft(updatedTarget, Option.Some(originalSource)));

        Assert.True(createRightResult);
        Assert.Equal(expectedOriginalTarget, createRightResult.Data);
        Assert.True(putLeftResult);
        Assert.Equal(expectedUpdatedSource, putLeftResult.Data);
    }

    public override void RoundTrip_WithLeftSideUpdate()
    {
        (TRight originalSource, TLeft expectedOriginalTarget, TLeft updatedTarget, TRight expectedUpdatedSource) = _roundTripWithLeftSideUpdateData;

        var createLeftResult = _lens.CreateLeft(originalSource);
        var putRightResult = createLeftResult.Bind(target => _lens.PutRight(updatedTarget, Option.Some(originalSource)));

        Assert.True(createLeftResult);
        Assert.Equal(expectedOriginalTarget, createLeftResult.Data);
        Assert.True(putRightResult);
        Assert.Equal(expectedUpdatedSource, putRightResult.Data);
    }

}