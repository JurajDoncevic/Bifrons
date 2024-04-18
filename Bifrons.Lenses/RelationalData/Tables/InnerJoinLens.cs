using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Tables;

public sealed class InnerJoinLens : ISymmetricLens<(TableData, TableData), TableData>
{
    private readonly Relational.Tables.InnerJoinLens _tableLens;

    public InnerJoinLens(Relational.Tables.InnerJoinLens tableLens)
    {
        _tableLens = tableLens;
    }

    public Func<TableData, Option<(TableData, TableData)>, Result<(TableData, TableData)>> PutLeft => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => updatedSource.Columns.Contains(_tableLens.LeftKey) && updatedSource.Columns.Contains(_tableLens.RightKey)
                ? updatedSource.RowData.Fold(
                    (Enumerable.Empty<RowData>(), Enumerable.Empty<RowData>()),
                    (row, sepCols) => 
                    (
                        sepCols.Item1.Append(RowData.Cons(row.ColumnData.Where(cd => target.Item1.Columns.Contains(cd.Column)))),
                        sepCols.Item2.Append(RowData.Cons(row.ColumnData.Where(cd => target.Item2.Columns.Contains(cd.Column))))
                    )).ToIdentity()
                    .Map(sepCols => TableData.Cons(target.Item1.Table, sepCols.Item1)
                                    .Bind(tbl1 => TableData.Cons(target.Item2.Table, sepCols.Item2).Map(tbl2 => (tbl1, tbl2))))
                    .Data
                : Result.Failure<(TableData, TableData)>($"Table {updatedSource.Name} does not contain the specified keys: {_tableLens.LeftKey} and {_tableLens.RightKey}."),
            () => CreateLeft(updatedSource)
        );

    public Func<(TableData, TableData), Option<TableData>, Result<TableData>> PutRight => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _tableLens.PutRight((updatedSource.Item1.Table, updatedSource.Item2.Table), target.Table)
                .Bind(joinedTable => 
                    TableData.Cons(
                        joinedTable, 
                        updatedSource.Item1.RowData.Zip(updatedSource.Item2.RowData, (left, right) => left.Concat(right)))
                        ),
            () => CreateRight(updatedSource)
        );

    public Func<(TableData, TableData), Result<TableData>> CreateRight => 
        source => source.Item1.Columns.Contains(_tableLens.LeftKey) && source.Item2.Columns.Contains(_tableLens.RightKey)
            ? _tableLens.CreateRight((source.Item1.Table, source.Item2.Table))
              .Bind(joinedTable =>
                TableData.Cons(
                    joinedTable, 
                    source.Item1.RowData.Join(
                        source.Item2.RowData, 
                        rd1 => rd1[_tableLens.LeftKey.Name].Value.BoxedData, 
                        rd2 => rd2[_tableLens.RightKey.Name].Value.BoxedData, 
                        (rd1, rd2) => rd1.Concat(rd2)
                        )
                )
              )
            : Result.Failure<TableData>($"Tables {source.Item1.Name} and {source.Item2.Name} do not contain the specified keys: {_tableLens.LeftKey} and {_tableLens.RightKey}.");

    public Func<TableData, Result<(TableData, TableData)>> CreateLeft => 
        source => _tableLens.CreateLeft(source.Table)
                    .Bind(disjoinedTables => 
                        TableData.Cons(
                            disjoinedTables.Item1, 
                            source.RowData.Map(rd => RowData.Cons(rd.ColumnData.TakeWhile(col => col.Column != _tableLens.RightKey)))
                        )
                        .Bind(leftTable => 
                            TableData.Cons(
                                disjoinedTables.Item2, 
                                source.RowData.Map(rd => RowData.Cons(rd.ColumnData.SkipWhile(col => col.Column != _tableLens.RightKey)))
                            )
                            .Map(rightTable => (leftTable, rightTable))
                        ));
}
