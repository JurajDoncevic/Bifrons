namespace Bifrons.Base;

/// <summary>
/// Terminal type.
/// </summary>
public struct Unit { };


/// <summary>
/// Unit extension methods.
/// </summary>
public static class UnitExt
{
    /// <summary>
    /// Unit value.
    /// </summary>
    public static Unit Unit() => default(Unit);

    /// <summary>
    /// Ignore value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_"></param>
    /// <returns>A Unit value</returns>
    public static Unit Ignore<T>(this T target) => Unit();
}