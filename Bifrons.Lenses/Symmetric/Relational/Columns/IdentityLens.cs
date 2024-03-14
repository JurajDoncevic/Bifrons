﻿using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Columns
;

public sealed class IdentityLens : SymmetricColumnLens
{
    private readonly string _columnName;

    private IdentityLens(string columnName)
    {
        _columnName = columnName;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft => 
        (updatedSource, originalTarget) => 
            originalTarget.Match(
                target => Result.Success(target),
                () => Result.Success(Column.Cons(_columnName, updatedSource.DataType))
                );

    public override Func<Column, Option<Column>, Result<Column>> PutRight => 
        (updatedSource, originalTarget) => 
            originalTarget.Match(
                target => Result.Success(target),
                () => Result.Success(Column.Cons(_columnName, updatedSource.DataType))
                );

    public override Func<Column, Result<Column>> CreateRight => 
        source => Result.Success(Column.Cons(_columnName, source.DataType));

    public override Func<Column, Result<Column>> CreateLeft => 
        source => Result.Success(Column.Cons(_columnName, source.DataType));

    public static IdentityLens Cons(string columnName)
        => new(columnName);
}