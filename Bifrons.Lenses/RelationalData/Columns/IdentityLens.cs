using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Strings;

namespace Bifrons.Lenses.RelationalData.Columns;

public abstract class IdentityLens<TDataColumn, TData, TColumn>
    : SymmetricDataColumnLens<TDataColumn, TData, TColumn, TDataColumn, TData, TColumn>
    where TDataColumn : DataColumn<TData, TColumn>
    where TColumn : Column, IColumn<TData>
{
    public IdentityLens(SymmetricColumnLens columnLens, ISymmetricLens<TData, TData> dataLens) 
        : base(columnLens, dataLens)
    {
    }

    public override Func<TDataColumn, Option<TDataColumn>, Result<TDataColumn>> PutLeft => 
        (updatedSource, originalTarget) 
            => originalTarget.Match(
                target => _columnLens.PutLeft(updatedSource.Column, target.Column)
                            .Bind(column => updatedSource.Data.Zip(target.Data, (l, r) => _dataLens.PutLeft(l, r))
                                .Unfold()
                                .Map(data => IDataColumn.Cons((column as TColumn)!, data) as TDataColumn)
                                )!,
                () => CreateLeft(updatedSource)
                )!;

    public override Func<TDataColumn, Option<TDataColumn>, Result<TDataColumn>> PutRight => 
        (updatedSource, originalTarget) 
            => originalTarget.Match(
                target => _columnLens.PutRight(updatedSource.Column, target.Column)
                            .Bind(column => updatedSource.Data.Zip(target.Data, (l, r) => _dataLens.PutRight(l, r))
                                .Unfold()
                                .Map(data => IDataColumn.Cons((column as TColumn)!, data) as TDataColumn)
                                )!,
                () => CreateRight(updatedSource)
                )!;

    public override Func<TDataColumn, Result<TDataColumn>> CreateRight => 
        source => _columnLens.CreateRight(source.Column)
                    .Bind(column 
                        => source.Data.Fold(
                                Enumerable.Empty<Result<TData>>(), 
                                (data, res) =>  res.Append(_dataLens.CreateRight(data))
                                ).Unfold()
                                .Map(data => IDataColumn.Cons((column as TColumn)!, data) as TDataColumn)
                        )!;

    public override Func<TDataColumn, Result<TDataColumn>> CreateLeft => 
        source => _columnLens.CreateLeft(source.Column)
                    .Bind(column 
                        => source.Data.Fold(
                                Enumerable.Empty<Result<TData>>(), 
                                (data, res) =>  res.Append(_dataLens.CreateLeft(data))
                                ).Unfold()
                                .Map(data => IDataColumn.Cons((column as TColumn)!, data) as TDataColumn)
                        )!;
}


public class IntegerIdentityLens : IdentityLens<IntegerDataColumn, int, IntegerColumn>
{
    public IntegerIdentityLens(SymmetricColumnLens columnLens, ISymmetricLens<int, int> dataLens) 
        : base(columnLens, dataLens)
    {
    }

    public static IntegerIdentityLens Cons(SymmetricColumnLens columnLens, ISymmetricLens<int, int> dataLens)
        => new(columnLens, dataLens);
}

public class StringIdentityLens : IdentityLens<StringDataColumn, string, StringColumn>
{
    public StringIdentityLens(SymmetricColumnLens columnLens, ISymmetricLens<string, string> dataLens) 
        : base(columnLens, dataLens)
    {
    }

    public static StringIdentityLens Cons(SymmetricColumnLens columnLens, SymmetricStringLens dataLens)
        => new(columnLens, dataLens);
}