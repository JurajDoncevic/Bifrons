using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Columns;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Tables;

public sealed class IdentityLens : SymmetricDataTableLens
{
    private readonly List<ISymmetricDataColumnLens> _dataColumnLenses;

    public IdentityLens(IEnumerable<ISymmetricDataColumnLens> dataColumnLenses)
    {
        _dataColumnLenses = dataColumnLenses.ToList();
    }

    public override Func<DataTable, Option<DataTable>, Result<DataTable>> PutLeft => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _dataColumnLenses
                .Map(lens => (dataColumn: updatedSource[lens.MatchesColumnNameLeft], lens: lens))
                .Where(t => t.dataColumn is not null)
                .Map(t => t.lens.ForDataType switch {
                    DataTypes.INTEGER => (t.lens as SymmetricDataColumnLens<DataColumn<int, IntegerColumn>, int, IntegerColumn, DataColumn<int, IntegerColumn>, int, IntegerColumn>)!.PutLeft(t.dataColumn as DataColumn<int, IntegerColumn>, target[t.lens.MatchesColumnNameRight] as DataColumn<int, IntegerColumn>).Map(_ => _ as IDataColumn),
                    DataTypes.STRING => (t.lens as SymmetricDataColumnLens<DataColumn<string, StringColumn>, string, StringColumn, DataColumn<string, StringColumn>, string, StringColumn>)!.PutLeft(t.dataColumn as DataColumn<string, StringColumn>, target[t.lens.MatchesColumnNameRight] as DataColumn<string, StringColumn>).Map(_ => _ as IDataColumn),
                    DataTypes.DECIMAL => (t.lens as SymmetricDataColumnLens<DataColumn<double, DecimalColumn>, double, DecimalColumn, DataColumn<double, DecimalColumn>, double, DecimalColumn>)!.PutLeft(t.dataColumn as DataColumn<double, DecimalColumn>, target[t.lens.MatchesColumnNameRight] as DataColumn<double, DecimalColumn>).Map(_ => _ as IDataColumn),
                    DataTypes.BOOLEAN => (t.lens as SymmetricDataColumnLens<DataColumn<bool, BooleanColumn>, bool, BooleanColumn, DataColumn<bool, BooleanColumn>, bool, BooleanColumn>)!.PutLeft(t.dataColumn as DataColumn<bool, BooleanColumn>, target[t.lens.MatchesColumnNameRight] as DataColumn<bool, BooleanColumn>).Map(_ => _ as IDataColumn),
                    DataTypes.DATETIME => (t.lens as SymmetricDataColumnLens<DataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn, DataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn>)!.PutLeft(t.dataColumn as DataColumn<DateTime, DateTimeColumn>, target[t.lens.MatchesColumnNameRight] as DataColumn<DateTime, DateTimeColumn>).Map(_ => _ as IDataColumn),
                    DataTypes.UNIT => (t.lens as SymmetricDataColumnLens<DataColumn<Unit, UnitColumn>, Unit, UnitColumn, DataColumn<Unit, UnitColumn>, Unit, UnitColumn>)!.PutLeft(t.dataColumn as DataColumn<Unit, UnitColumn>, target[t.lens.MatchesColumnNameRight] as DataColumn<Unit, UnitColumn>).Map(_ => _ as IDataColumn),
                    _ => Result.Failure<IDataColumn>($"Unsupported data type {t.lens.ForDataType}")
                })
                .Unfold()
                .Map(dataColumns => DataTable.Cons(updatedSource.Name, dataColumns)),
            () => CreateLeft(updatedSource)
            );

    public override Func<DataTable, Option<DataTable>, Result<DataTable>> PutRight => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _dataColumnLenses
                .Map(lens => (dataColumn: updatedSource[lens.MatchesColumnNameRight], lens: lens))
                .Where(t => t.dataColumn is not null)
                .Map(t => t.lens.ForDataType switch {
                    DataTypes.INTEGER => (t.lens as SymmetricDataColumnLens<DataColumn<int, IntegerColumn>, int, IntegerColumn, DataColumn<int, IntegerColumn>, int, IntegerColumn>)!.PutRight(t.dataColumn as DataColumn<int, IntegerColumn>, target[t.lens.MatchesColumnNameLeft] as DataColumn<int, IntegerColumn>).Map(_ => _ as IDataColumn),
                    DataTypes.STRING => (t.lens as SymmetricDataColumnLens<DataColumn<string, StringColumn>, string, StringColumn, DataColumn<string, StringColumn>, string, StringColumn>)!.PutRight(t.dataColumn as DataColumn<string, StringColumn>, target[t.lens.MatchesColumnNameLeft] as DataColumn<string, StringColumn>).Map(_ => _ as IDataColumn),
                    DataTypes.DECIMAL => (t.lens as SymmetricDataColumnLens<DataColumn<double, DecimalColumn>, double, DecimalColumn, DataColumn<double, DecimalColumn>, double, DecimalColumn>)!.PutRight(t.dataColumn as DataColumn<double, DecimalColumn>, target[t.lens.MatchesColumnNameLeft] as DataColumn<double, DecimalColumn>).Map(_ => _ as IDataColumn),
                    DataTypes.BOOLEAN => (t.lens as SymmetricDataColumnLens<DataColumn<bool, BooleanColumn>, bool, BooleanColumn, DataColumn<bool, BooleanColumn>, bool, BooleanColumn>)!.PutRight(t.dataColumn as DataColumn<bool, BooleanColumn>, target[t.lens.MatchesColumnNameLeft] as DataColumn<bool, BooleanColumn>).Map(_ => _ as IDataColumn),
                    DataTypes.DATETIME => (t.lens as SymmetricDataColumnLens<DataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn, DataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn>)!.PutRight(t.dataColumn as DataColumn<DateTime, DateTimeColumn>, target[t.lens.MatchesColumnNameLeft] as DataColumn<DateTime, DateTimeColumn>).Map(_ => _ as IDataColumn),
                    DataTypes.UNIT => (t.lens as SymmetricDataColumnLens<DataColumn<Unit, UnitColumn>, Unit, UnitColumn, DataColumn<Unit, UnitColumn>, Unit, UnitColumn>)!.PutRight(t.dataColumn as DataColumn<Unit, UnitColumn>, target[t.lens.MatchesColumnNameLeft] as DataColumn<Unit, UnitColumn>).Map(_ => _ as IDataColumn),
                    _ => Result.Failure<IDataColumn>($"Unsupported data type {t.lens.ForDataType}")
                })
                .Unfold()
                .Map(dataColumns => DataTable.Cons(updatedSource.Name, dataColumns)),
            () => CreateRight(updatedSource)
            );

    public override Func<DataTable, Result<DataTable>> CreateRight => 
        source => _dataColumnLenses
            .Where(lens => lens.MatchesLeft)
            .Map(lens => (dataColumn: source[lens.MatchesColumnNameLeft], lens: lens))
            .Where(t => t.dataColumn is not null)
            .Map(t => t.lens.ForDataType switch {
                DataTypes.INTEGER => (t.lens as SymmetricDataColumnLens<DataColumn<int, IntegerColumn>, int, IntegerColumn, DataColumn<int, IntegerColumn>, int, IntegerColumn>)!.CreateRight(t.dataColumn as DataColumn<int, IntegerColumn>).Map(_ => _ as IDataColumn),
                DataTypes.STRING => (t.lens as SymmetricDataColumnLens<DataColumn<string, StringColumn>, string, StringColumn, DataColumn<string, StringColumn>, string, StringColumn>)!.CreateRight(t.dataColumn as DataColumn<string, StringColumn>).Map(_ => _ as IDataColumn),
                DataTypes.DECIMAL => (t.lens as SymmetricDataColumnLens<DataColumn<double, DecimalColumn>, double, DecimalColumn, DataColumn<double, DecimalColumn>, double, DecimalColumn>)!.CreateRight(t.dataColumn as DataColumn<double, DecimalColumn>).Map(_ => _ as IDataColumn),
                DataTypes.BOOLEAN => (t.lens as SymmetricDataColumnLens<DataColumn<bool, BooleanColumn>, bool, BooleanColumn, DataColumn<bool, BooleanColumn>, bool, BooleanColumn>)!.CreateRight(t.dataColumn as DataColumn<bool, BooleanColumn>).Map(_ => _ as IDataColumn),
                DataTypes.DATETIME => (t.lens as SymmetricDataColumnLens<DataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn, DataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn>)!.CreateRight(t.dataColumn as DataColumn<DateTime, DateTimeColumn>).Map(_ => _ as IDataColumn),
                DataTypes.UNIT => (t.lens as SymmetricDataColumnLens<DataColumn<Unit, UnitColumn>, Unit, UnitColumn, DataColumn<Unit, UnitColumn>, Unit, UnitColumn>)!.CreateRight(t.dataColumn as DataColumn<Unit, UnitColumn>).Map(_ => _ as IDataColumn),
                _ => Result.Failure<IDataColumn>($"Unsupported data type {t.lens.ForDataType}")
            })
            .Unfold()
            .Map(dataColumns => DataTable.Cons(source.Name, dataColumns));

    public override Func<DataTable, Result<DataTable>> CreateLeft =>
        source => _dataColumnLenses
            .Where(lens => lens.MatchesRight)
            .Map(lens => (dataColumn: source[lens.MatchesColumnNameRight], lens: lens))
            .Where(t => t.dataColumn is not null)
            .Map(t => t.lens.ForDataType switch {
                DataTypes.INTEGER => (t.lens as SymmetricDataColumnLens<DataColumn<int, IntegerColumn>, int, IntegerColumn, DataColumn<int, IntegerColumn>, int, IntegerColumn>)!.CreateLeft(t.dataColumn as DataColumn<int, IntegerColumn>).Map(_ => _ as IDataColumn),
                DataTypes.STRING => (t.lens as SymmetricDataColumnLens<DataColumn<string, StringColumn>, string, StringColumn, DataColumn<string, StringColumn>, string, StringColumn>)!.CreateLeft(t.dataColumn as DataColumn<string, StringColumn>).Map(_ => _ as IDataColumn),
                DataTypes.DECIMAL => (t.lens as SymmetricDataColumnLens<DataColumn<double, DecimalColumn>, double, DecimalColumn, DataColumn<double, DecimalColumn>, double, DecimalColumn>)!.CreateLeft(t.dataColumn as DataColumn<double, DecimalColumn>).Map(_ => _ as IDataColumn),
                DataTypes.BOOLEAN => (t.lens as SymmetricDataColumnLens<DataColumn<bool, BooleanColumn>, bool, BooleanColumn, DataColumn<bool, BooleanColumn>, bool, BooleanColumn>)!.CreateLeft(t.dataColumn as DataColumn<bool, BooleanColumn>).Map(_ => _ as IDataColumn),
                DataTypes.DATETIME => (t.lens as SymmetricDataColumnLens<DataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn, DataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn>)!.CreateLeft(t.dataColumn as DataColumn<DateTime, DateTimeColumn>).Map(_ => _ as IDataColumn),
                DataTypes.UNIT => (t.lens as SymmetricDataColumnLens<DataColumn<Unit, UnitColumn>, Unit, UnitColumn, DataColumn<Unit, UnitColumn>, Unit, UnitColumn>)!.CreateLeft(t.dataColumn as DataColumn<Unit, UnitColumn>).Map(_ => _ as IDataColumn),
                _ => Result.Failure<IDataColumn>($"Unsupported data type {t.lens.ForDataType}")
            })
            .Unfold()
            .Map(dataColumns => DataTable.Cons(source.Name, dataColumns));
}
