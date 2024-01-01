using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Concatenates two simple symmetric string lenses. Lens regexes have to take into account the preceding lens regexes of the concat.
/// e.g. <c>id(\w+) | id((?!\w+\s)\w+)</c> is a valid lens, but <c>id(\w+) | id(\w+)</c> is not.
/// </summary>
public sealed class ConcatLens : SymmetricStringLens
{
    /// <summary>
    /// Left-hand operand lens of the concat.
    /// </summary>
    private readonly SymmetricStringLens _leftLens;

    /// <summary>
    /// Right-hand operand lens of the concat.
    /// </summary>
    private readonly SymmetricStringLens _rightLens;

    public override Regex LeftRegex => new Regex(_leftLens.LeftRegex.ToString() + _rightLens.LeftRegex.ToString());
    public override Regex RightRegex => new Regex(_leftLens.RightRegex.ToString() + _rightLens.RightRegex.ToString());


    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="leftLens">Left-hand operand lens of the concat.</param>
    /// <param name="rightLens">Right-hand operand lens of the concat.</param>
    private ConcatLens(SymmetricStringLens leftLens, SymmetricStringLens rightLens)
    {
        _leftLens = leftLens;
        _rightLens = rightLens;
    }

    public override Func<string, Option<string>, Result<string>> PutLeft =>
        (updatedSource, originalTarget) =>
        {
            if (!originalTarget)
            {
                return CreateRight(updatedSource);
            }

            var originalTargetValue = originalTarget.Value;

            var result = _leftLens.PutRight(updatedSource, originalTarget)
                .Map(leftLensRes => (
                    leftLensRes,
                    unmatchedSourcePrefix: _leftLens.LeftRegex.GetNonMatchingValueFromStart(updatedSource),
                    unmatchedSourceSuffix: _leftLens.LeftRegex.GetNonMatchingValueToEnd(updatedSource),
                    unmatchedTargetPrefix: _leftLens.RightRegex.GetNonMatchingValueFromStart(originalTargetValue),
                    unmatchedTargetSuffix: _leftLens.RightRegex.GetNonMatchingValueToEnd(originalTargetValue)
                ))
                .Bind(res => _rightLens.PutRight(res.unmatchedSourceSuffix, res.unmatchedTargetSuffix)
                                        .Map(rightLensRes => (
                                            lensRes: res.leftLensRes + rightLensRes,
                                            unmatchedSourcePrefix: res.unmatchedSourcePrefix,
                                            unmatchedSourceSuffix: _rightLens.LeftRegex.GetNonMatchingValueToEnd(res.unmatchedSourceSuffix),
                                            unmatchedTargetPrefix: res.unmatchedTargetPrefix,
                                            unmatchedTargetSuffix: _rightLens.RightRegex.GetNonMatchingValueToEnd(res.unmatchedTargetSuffix)
                                        )))
                                        .Map(res => res.unmatchedTargetPrefix + /*res.unmatchedSourcePrefix +*/ res.lensRes + /*res.unmatchedSourceSuffix +*/ res.unmatchedTargetSuffix);

            return result;

        };

    public override Func<string, Option<string>, Result<string>> PutRight =>
        (updatedSource, originalTarget) =>
        {
            if (!originalTarget)
            {
                return CreateRight(updatedSource);
            }

            var originalTargetValue = originalTarget.Value;

            var result = _leftLens.PutRight(updatedSource, originalTarget)
                .Map(leftLensRes => (
                    leftLensRes,
                    unmatchedSourcePrefix: _leftLens.LeftRegex.GetNonMatchingValueFromStart(updatedSource),
                    unmatchedSourceSuffix: _leftLens.LeftRegex.GetNonMatchingValueToEnd(updatedSource),
                    unmatchedTargetPrefix: _leftLens.RightRegex.GetNonMatchingValueFromStart(originalTargetValue),
                    unmatchedTargetSuffix: _leftLens.RightRegex.GetNonMatchingValueToEnd(originalTargetValue)
                ))
                .Bind(res => _rightLens.PutRight(res.unmatchedSourceSuffix, res.unmatchedTargetSuffix)
                                        .Map(rightLensRes => (
                                            lensRes: res.leftLensRes + rightLensRes,
                                            unmatchedSourcePrefix: res.unmatchedSourcePrefix,
                                            unmatchedSourceSuffix: _rightLens.LeftRegex.GetNonMatchingValueToEnd(res.unmatchedSourceSuffix),
                                            unmatchedTargetPrefix: res.unmatchedTargetPrefix,
                                            unmatchedTargetSuffix: _rightLens.RightRegex.GetNonMatchingValueToEnd(res.unmatchedTargetSuffix)
                                        )))
                                        .Map(res => res.unmatchedTargetPrefix + /*res.unmatchedSourcePrefix +*/ res.lensRes + /*res.unmatchedSourceSuffix +*/ res.unmatchedTargetSuffix);

            return result;
        };

    public override Func<string, Result<string>> CreateRight =>
        source =>
        {
            var result = _leftLens.CreateRight(source)
                .Map(leftLensResult => (
                    leftLensResult,
                    unmatchedPrefix: _leftLens.LeftRegex.GetNonMatchingValueFromStart(source),
                    unmatchedSuffix: _leftLens.LeftRegex.GetNonMatchingValueToEnd(source)
                ))
                .Bind(res => _rightLens.CreateRight(res.unmatchedSuffix)
                                        .Map(rightLensRes => res.leftLensResult + rightLensRes));

            return result;
        };

    public override Func<string, Result<string>> CreateLeft =>
        source =>
        {
            var result = _leftLens.CreateLeft(source)
                .Map(leftLensResult => (
                    leftLensResult,
                    unmatchedPrefix: _leftLens.RightRegex.GetNonMatchingValueFromStart(source),
                    unmatchedSuffix: _leftLens.RightRegex.GetNonMatchingValueToEnd(source)
                ))
                .Bind(res => _rightLens.CreateLeft(res.unmatchedSuffix)
                                        .Map(rightLensRes => res.leftLensResult + rightLensRes));

            return result;
        };

    /// <summary>
    /// Constructs a concat lens.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    /// <returns>A concat lens</returns>
    public static ConcatLens Cons(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => new(lhsLens, rhsLens);
}
