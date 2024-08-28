using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Columns;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Tables;

public sealed class IdentityLens : SymmetricTableDataLens
{
    private readonly ISet<Column> _identityColumns;
    private IdentityLens(Relational.Tables.IdentityLens tableLens, IEnumerable<ISymmetricColumnDataLens> columnDataLenses, Option<IEnumerable<Column>> identityColumns = default) 
        : base(tableLens, Option.Some(columnDataLenses))
    {
        _identityColumns = identityColumns.Match(ic => ic.ToHashSet(), () => []);
    }

    public override Func<TableData, Option<TableData>, Result<TableData>> PutLeft => 
        (updatedSource, originalTarget) => 
            _tableLens.PutLeft(updatedSource.Table, originalTarget.Map(_ => _.Table))
                .Bind(leftTable =>
                    originalTarget.Match(
                        originalTarget => Result.AsResult(() =>{
                            // see if you can find a matching row from the updated source in the originalTarget
                            // includes both data from L and L & R; not from only R
                            var leftRowDataMatches = updatedSource.RowData
                                .Select(rightRow => (rightRow: rightRow, idRow: GetIdentityRow(rightRow, _identityColumns)))
                                .Select(tuple => (rightRow: tuple.rightRow, leftRowMatch: FindMatchingRowData(tuple.idRow, originalTarget.RowData)));
                            

                            var leftAdaptedRowData = leftRowDataMatches
                                .Select(tuple => {
                                    var rightRow = tuple.rightRow;
                                    var leftRowMatch = tuple.leftRowMatch;
                                    return leftRowMatch.Match(
                                        leftRow => ColumnDataLenses.Fold(
                                            Enumerable.Empty<Result<ColumnData>>(),
                                            (cdl, res) => cdl.MatchesRight
                                                ? res.Append(rightRow[cdl.MatchesColumnNameRight]
                                                    .Match(
                                                        cd => cdl.PutLeft(cd, leftRow[cd.Column.Name]),
                                                        () => Result.Failure<ColumnData>($"No column found for lens {cdl.MatchesLeft}")
                                                    )
                                                )
                                                : res.Append(cdl.PutLeft(UnitColumnData.Cons(), leftRow.ColumnData.First(cd2 => cd2.Name == cdl.MatchesColumnNameLeft)))
                                        ).Unfold(),
                                        () => ColumnDataLenses.Fold(
                                            Enumerable.Empty<Result<ColumnData>>(),
                                            (cdl, res) => cdl.MatchesRight
                                                ? res.Append(rightRow[cdl.MatchesColumnNameRight]
                                                    .Match(
                                                        cd => cdl.PutLeft(cd, Option.None<ColumnData>()), 
                                                        () => Result.Failure<ColumnData>($"No column found for lens {cdl.MatchesLeft}")
                                                    )
                                                )
                                                : res.Append(cdl.PutLeft(UnitColumnData.Cons(), Option.None<ColumnData>()))
                                        ).Unfold()
                                    );
                                    })
                                .Select(cds => cds.Map(cd => RowData.Cons(cd.Where(_ => !_.IsUnit))))
                                .Unfold();
                            
                            // find rows in the updated source that are not in the original target
                            // this is the remainder of the L data not matched previously
                            var unmatchedLeftRowData = updatedSource.RowData.Where(leftRow => FindMatchingRowData(GetIdentityRow(leftRow, _identityColumns), originalTarget.RowData).IsNone);
                            // combine the matched and unmatched data
                            var rowData = leftAdaptedRowData.Map(rows => rows.Concat(unmatchedLeftRowData));
                            return rowData;
                        }).Bind(rowData => TableData.Cons(leftTable, rowData)),
                        () => CreateLeft(updatedSource)
                    )              
                );

    public override Func<TableData, Option<TableData>, Result<TableData>> PutRight => 
        (updatedSource, originalTarget) => 
            _tableLens.PutRight(updatedSource.Table, originalTarget.Map(_ => _.Table))
                .Bind(rightTable =>
                    originalTarget.Match(
                        originalTarget => Result.AsResult(() =>{
                            // see if you can find a matching row from the updated source in the originalTarget
                            // includes both data from L and L & R; not from only R
                            var rightRowDataMatches = updatedSource.RowData
                                .Select(leftRow => (leftRow: leftRow, idRow: GetIdentityRow(leftRow, _identityColumns)))
                                .Select(tuple => (leftRow: tuple.leftRow, rightRowMatch: FindMatchingRowData(tuple.idRow, originalTarget.RowData)));                                

                            var rightAdaptedRowData = rightRowDataMatches
                                .Select(tuple => {
                                    var leftRow = tuple.leftRow;
                                    var rightRowMatch = tuple.rightRowMatch;
                                    return rightRowMatch.Match(
                                        rightRow => ColumnDataLenses.Fold(
                                            Enumerable.Empty<Result<ColumnData>>(),
                                            (cdl, res) => cdl.MatchesLeft
                                                ? res.Append(leftRow[cdl.MatchesColumnNameLeft]
                                                     .Match(
                                                        cd => cdl.PutRight(cd, rightRow[cd.Column.Name]), 
                                                        () => Result.Failure<ColumnData>($"No column found for lens {cdl.MatchesRight}")
                                                    )
                                                )
                                                : res.Append(cdl.PutRight(UnitColumnData.Cons(), rightRow.ColumnData.First(cd2 => cd2.Name == cdl.MatchesColumnNameRight)))
                                        ).Unfold(),
                                        () => ColumnDataLenses.Fold(
                                            Enumerable.Empty<Result<ColumnData>>(),
                                            (cdl, res) => cdl.MatchesLeft
                                                ? res.Append(leftRow[cdl.MatchesColumnNameLeft]
                                                     .Match(
                                                        cd => cdl.PutRight(cd, Option.None<ColumnData>()), 
                                                        () => Result.Failure<ColumnData>($"No column found for lens {cdl.MatchesRight}")
                                                    )
                                                )
                                                : res.Append(cdl.PutRight(UnitColumnData.Cons(), Option.None<ColumnData>()))
                                        ).Unfold()
                                    );
                                    })
                                .Select(cds => cds.Map(cd => RowData.Cons(cd.Where(_ => !_.IsUnit))))
                                .Unfold();

                            // find rows in the original target that are not in the updated source
                            // this is the remainder of the R data not matched previously
                            var unmatchedRightRowData = originalTarget.RowData.Where(rightRow => FindMatchingRowData(GetIdentityRow(rightRow, _identityColumns), updatedSource.RowData).IsNone);
                            // combine the matched and unmatched data
                            var rowData = rightAdaptedRowData.Map(rows => rows.Concat(unmatchedRightRowData));
                            return rowData;
                        }).Bind(rowData => TableData.Cons(rightTable, rowData)),
                        () => CreateRight(updatedSource)
                    )              
                );

    public override Func<TableData, Result<TableData>> CreateRight => 
        source => _tableLens.CreateRight(source.Table)
            .Bind(table => 
                    source.RowData.Map(rd => // for each row data
                        // go by lenses, not by columns - else it misses insert (on left) and delete (on right) cases
                        ColumnDataLenses.Fold(
                            Enumerable.Empty<Result<ColumnData>>(),
                            (cdl, res) => cdl.MatchesLeft
                                ? res.Append(rd[cdl.MatchesColumnNameLeft]
                                    .Match(
                                        cd => cdl.CreateRight(cd), 
                                        () => Result.Failure<ColumnData>($"No column found for lens {cdl.MatchesRight}")
                                    )
                                )
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
                                ? res.Append(rd[cdl.MatchesColumnNameRight]
                                    .Match(
                                        cd => cdl.CreateLeft(cd), 
                                        () => Result.Failure<ColumnData>($"No column found for lens {cdl.MatchesLeft}")
                                    )
                                )
                                : res.Append(cdl.CreateLeft(UnitColumnData.Cons()))
                        ).Unfold()
                        .Map(row => row.Where(col => !col.IsUnit))
                        .Map(row => RowData.Cons(row))
                    ).Unfold()
                    .Bind(rd => TableData.Cons(table, rd))
                );

    public static Result<IdentityLens> Cons(Relational.Tables.IdentityLens tableLens, IEnumerable<ISymmetricColumnDataLens> columnDataLenses, Option<IEnumerable<Column>> identityColumns = default)
    {
        if(!tableLens.ColumnLenses.All(cdl => columnDataLenses.Any(cdl2 => cdl.MatchesColumnNameLeft == cdl2.MatchesColumnNameLeft)))
        {
            return Result.Failure<IdentityLens>("Column data lenses do not match the table lens.");
        }
        if(identityColumns.IsSome && !identityColumns.Value.All(c => columnDataLenses.Any(cdl => cdl.MatchesColumnNameLeft == c.Name)))
        {
            return Result.Failure<IdentityLens>("Column data lenses do not match the table lens.");
        }

        return Result.Success(new IdentityLens(tableLens, columnDataLenses, identityColumns));
    }

    private static Option<RowData> FindMatchingRowData(RowData identityRow, IEnumerable<RowData> rows)
    {
        return rows.FirstOrDefault(
                row => row.ColumnData.Where(cd => identityRow.Columns.Contains(cd.Column))
                    .All(cd => identityRow[cd.Column.Name].Match(cd2 => cd2.BoxedData?.Equals(cd.BoxedData) ?? true, () => false)
                )).ToOption();
    }

    private static RowData GetIdentityRow(RowData row, IEnumerable<Column> identityColumns)
    {
        return RowData.Cons(row.ColumnData.Where(cd => identityColumns.Any(c => c.Name == cd.Name)));
    }
}
