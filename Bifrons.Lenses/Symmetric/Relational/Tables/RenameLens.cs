﻿using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Tables;

public sealed class RenameLens : SymmetricTableLens
{
    private readonly string _sourceTableName;
    private readonly string _targetTableName;

    public override string TargetTableName => _sourceTableName;

    private RenameLens(string sourceTableName, string targetTableName)
    {
        _sourceTableName = sourceTableName;
        _targetTableName = targetTableName;
    }

    public override Func<Table, Option<Table>, Result<Table>> PutLeft =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(Table.Cons(_sourceTableName, target.Columns)),
                () => CreateLeft(updatedSource)
            );

    public override Func<Table, Option<Table>, Result<Table>> PutRight =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(Table.Cons(_targetTableName, target.Columns)),
                () => CreateRight(updatedSource)
            );

    public override Func<Table, Result<Table>> CreateRight =>
        source => Result.Success(Table.Cons(_targetTableName, source.Columns));

    public override Func<Table, Result<Table>> CreateLeft =>
        source => Result.Success(Table.Cons(_sourceTableName, source.Columns));

    public static RenameLens Cons(string sourceTableName, string destinationTableName)
        => new(sourceTableName, destinationTableName);
}
