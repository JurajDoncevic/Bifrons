using Bifrons.Lenses.RelationalData.Columns;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Tables;

public sealed class IdentityLens : SymmetricTableDataLens
{

    private IdentityLens(Relational.Tables.IdentityLens tableLens, IEnumerable<ISymmetricColumnDataLens> columnDataLenses) 
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
                            (cd, res) => res.Append(GetLensMatchingRight(cd.Name).Match(lens => lens.PutLeft(cd, originalTarget.Map(ot => ot.RowData.First().ColumnData.First(cd2 => cd2.Name == cd.Name))), () => Result.Failure<ColumnData>($"No lens found for column {cd.Name}")))
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
                            (cd, res) => res.Append(GetLensMatchingLeft(cd.Name).Match(lens => lens.PutRight(cd, originalTarget.Map(ot => ot.RowData.First().ColumnData.First(cd2 => cd2.Name == cd.Name))), () => Result.Failure<ColumnData>($"No lens found for column {cd.Name}")))
                        ).Unfold().Map(cd => RowData.Cons(cd))
                    ).Unfold()
                    .Bind(rd => TableData.Cons(table, rd))
                );

    public override Func<TableData, Result<TableData>> CreateRight => 
        source => _tableLens.CreateRight(source.Table)
            .Bind(table => 
                    source.RowData.Map(rd => // for each row data
                        // go by lenses, not by columns - else it misses insert (on left) and delete (on right) cases
                        ColumnDataLenses.Fold(
                            Enumerable.Empty<Result<ColumnData>>(),
                            (cdl, res) => cdl.MatchesLeft
                                ? res.Append(rd[cdl.MatchesColumnNameLeft].Match(cd => cdl.CreateRight(cd), () => Result.Failure<ColumnData>($"No column found for lens {cdl.MatchesRight}")))
                                : res.Append(cdl.CreateRight(UnitColumnData.Cons()))
                        ).Unfold()
                        .Map(row => row.Where(col => !col.IsUnit))
                        .Map(row => RowData.Cons(row))
                    ).Unfold()
                    .Bind(rd => TableData.Cons(table, rd))
                );
            

    public override Func<TableData, Result<TableData>> CreateLeft => 
        source => _tableLens.CreateLeft(source.Table)
            .Bind(table => 
                    source.RowData.Map(rd => 
                        ColumnDataLenses.Fold(
                            Enumerable.Empty<Result<ColumnData>>(),
                            (cdl, res) => cdl.MatchesRight
                                ? res.Append(rd[cdl.MatchesColumnNameRight].Match(cd => cdl.CreateLeft(cd), () => Result.Failure<ColumnData>($"No column found for lens {cdl.MatchesLeft}")))
                                : res.Append(cdl.CreateLeft(UnitColumnData.Cons()))
                        ).Unfold()
                        .Map(row => row.Where(col => !col.IsUnit))
                        .Map(row => RowData.Cons(row))
                    ).Unfold()
                    .Bind(rd => TableData.Cons(table, rd))
                );

    public static Result<IdentityLens> Cons(Relational.Tables.IdentityLens tableLens, IEnumerable<ISymmetricColumnDataLens> columnDataLenses)
        => tableLens.ColumnLenses.All(cdl => columnDataLenses.Any(cdl2 => cdl.MatchesColumnNameLeft == cdl2.MatchesColumnNameLeft))
            ? Result.Success(new IdentityLens(tableLens, columnDataLenses))
            : Result.Failure<IdentityLens>("Column data lenses do not match the table lens.");
}
