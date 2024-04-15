using Bifrons.Lenses.Relational.Tables;
using Bifrons.Lenses.RelationalData.Columns;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Tables;

public sealed class RenameLens : SymmetricTableDataLens
{
    private RenameLens(Relational.Tables.RenameLens tableLens, IEnumerable<ISymmetricColumnDataLens> columnDataLenses) 
        : base(tableLens, Option.Some(columnDataLenses))
    {
    }

    public override Func<TableData, Option<TableData>, Result<TableData>> PutLeft => 
        (updatedSource, originalTarget) => 
            _tableLens.PutLeft(updatedSource.Table, originalTarget.Map(_ => _.Table))
                .Bind(table => 
                    updatedSource.RowData.Map(rd => 
                        rd.ColumnData.Fold(
                            Enumerable.Empty<Result<ColumnData>>(), 
                            (cd, res) => res.Append(this[cd.Name].Match(lens => lens.PutLeft(cd, originalTarget.Map(ot => ot.RowData.First().ColumnData.First(cd2 => cd2.Name == cd.Name))), () => Result.Failure<ColumnData>($"No lens found for column {cd.Name}")))
                        ).Unfold().Map(cd => RowData.Cons(cd))
                    ).Unfold()
                    .Bind(rd => TableData.Cons(table, rd))
                );

    public override Func<TableData, Option<TableData>, Result<TableData>> PutRight => 
        (updatedSource, originalTarget) => 
            _tableLens.PutRight(updatedSource.Table, originalTarget.Map(_ => _.Table))
                .Bind(table => 
                    updatedSource.RowData.Map(rd => 
                        rd.ColumnData.Fold(
                            Enumerable.Empty<Result<ColumnData>>(), 
                            (cd, res) => res.Append(this[cd.Name].Match(lens => lens.PutRight(cd, originalTarget.Map(ot => ot.RowData.First().ColumnData.First(cd2 => cd2.Name == cd.Name))), () => Result.Failure<ColumnData>($"No lens found for column {cd.Name}")))
                        ).Unfold().Map(cd => RowData.Cons(cd))
                    ).Unfold()
                    .Bind(rd => TableData.Cons(table, rd))
                );

    public override Func<TableData, Result<TableData>> CreateRight => 
        source => _tableLens.CreateRight(source.Table)
            .Bind(table => 
                    source.RowData.Map(rd => 
                        rd.ColumnData.Fold(
                            Enumerable.Empty<Result<ColumnData>>(), 
                            (cd, res) => res.Append(this[cd.Name].Match(lens => lens.CreateRight(cd), () => Result.Failure<ColumnData>($"No lens found for column {cd.Name}")))
                        ).Unfold().Map(cd => RowData.Cons(cd))
                    ).Unfold()
                    .Bind(rd => TableData.Cons(table, rd))
                );
            

    public override Func<TableData, Result<TableData>> CreateLeft => 
        source => _tableLens.CreateLeft(source.Table)
            .Bind(table => 
                    source.RowData.Map(rd => 
                        rd.ColumnData.Fold(
                            Enumerable.Empty<Result<ColumnData>>(), 
                            (cd, res) => res.Append(this[cd.Name].Match(lens => lens.CreateLeft(cd), () => Result.Failure<ColumnData>($"No lens found for column {cd.Name}")))
                        ).Unfold().Map(cd => RowData.Cons(cd))
                    ).Unfold()
                    .Bind(rd => TableData.Cons(table, rd))
                );

    public static Result<RenameLens> Cons(Relational.Tables.RenameLens tableLens, IEnumerable<ISymmetricColumnDataLens> columnDataLenses)
        => tableLens.ColumnLenses.All(cdl => columnDataLenses.Any(cdl2 => cdl.MatchesColumnNameLeft == cdl2.MatchesColumnNameLeft))
            ? Result.Success(new RenameLens(tableLens, columnDataLenses))
            : Result.Failure<RenameLens>("Column data lenses do not match the table lens.");
}
