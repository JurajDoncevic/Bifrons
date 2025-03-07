﻿namespace Bifrons.Base;

public static class Enumerables
{

    /// <summary>
    /// Map IEnumerable by index: E[T]->(idx->T->R)->E[R]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="target"></param>
    /// <param name="func"></param>
    /// <returns>Mapped enumerable</returns>
    public static IEnumerable<R> Mapi<T, R>(this IEnumerable<T> target, Func<int, T, R> func)
    {
        int idx = 0;
        foreach (var t in target)
        {
            yield return func(idx, t);
            idx++;
        }
    }

    /// <summary>
    /// Map IEnumerable E[T]->(T->R)->E[R]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="target"></param>
    /// <param name="func"></param>
    /// <returns>Mapped enumerable</returns>
    public static IEnumerable<R> Map<T, R>(this IEnumerable<T> target, Func<T, R> func)
    {
        foreach (T t in target)
            yield return func(t);
    }

    /// <summary>
    /// Map operation for a Task: Ta[T]->(T->R)->Ta[R]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="task"></param>
    /// <param name="operation"></param>
    /// <returns>Mapped Task</returns>
    public async static Task<R> Map<T, R>(this Task<T> task, Func<T, R> operation)
        => operation(await task);

}

