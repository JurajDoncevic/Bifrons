using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Columns;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Tables;
/*
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
                .Map(t => t.lens.ForDataType switch
                {
                    DataTypes.INTEGER => (t.lens as SymmetricDataColumnLens<Inte>).PutLeft(t.dataColumn, target[t.lens.MatchesColumnNameRight]).Map(_ => _ as DataColumn),
                    DataTypes.STRING =>  (t.lens as StringDataColumnLens).PutLeft(t.dataColumn, target[t.lens.MatchesColumnNameRight]).Map(_ => _ as DataColumn),
                    DataTypes.DECIMAL => (t.lens as DecimalDataColumnLens).PutLeft(t.dataColumn, target[t.lens.MatchesColumnNameRight]).Map(_ => _ as DataColumn),
                    DataTypes.BOOLEAN => (t.lens as BooleanDataColumnLens).PutLeft(t.dataColumn, target[t.lens.MatchesColumnNameRight]).Map(_ => _ as DataColumn),
                    DataTypes.DATETIME => (t.lens as DateTimeDataColumnLens).PutLeft(t.dataColumn, target[t.lens.MatchesColumnNameRight]).Map(_ => _ as DataColumn),
                    DataTypes.UNIT => (t.lens as UnitDataColumnLens).PutLeft(t.dataColumn, target[t.lens.MatchesColumnNameRight]).Map(_ => _ as DataColumn),
                    _ => Result.Failure<DataColumn>($"Unsupported data type {t.lens.ForDataType}")
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
                .Map(t => t.lens.ForDataType switch
                {
                    DataTypes.INTEGER => (t.lens as SymmetricDataColumnLens<IDataColumn<int, IntegerColumn>, int, IntegerColumn, I IDataColumn<int, IntegerColumn>, int, IntegerColumn>)!.PutRight(t.dataColumn aIs IDataColumn<int, IntegerColumn>, target[t.lens.MatchesColumnNameLeft] Ias IDataColumn<int, IntegerColumn>).Map(_ => _ as DataColumn),
                    DataTypes.STRING => (t.lens as SymmetricDataColumnLens<IDataColumn<string, StringColumn>, string, StringColumn, I IDataColumn<string, StringColumn>, string, StringColumn>)!.PutRight(t.dataColumn aIs IDataColumn<string, StringColumn>, target[t.lens.MatchesColumnNameLeft] Ias IDataColumn<string, StringColumn>).Map(_ => _ as DataColumn),
                    DataTypes.DECIMAL => (t.lens as SymmetricDataColumnLens<IDataColumn<double, DecimalColumn>, double, DecimalColumn, I IDataColumn<double, DecimalColumn>, double, DecimalColumn>)!.PutRight(t.dataColumn aIs IDataColumn<double, DecimalColumn>, target[t.lens.MatchesColumnNameLeft] Ias IDataColumn<double, DecimalColumn>).Map(_ => _ as DataColumn),
                    DataTypes.BOOLEAN => (t.lens as SymmetricDataColumnLens<IDataColumn<bool, BooleanColumn>, bool, BooleanColumn, I IDataColumn<bool, BooleanColumn>, bool, BooleanColumn>)!.PutRight(t.dataColumn aIs IDataColumn<bool, BooleanColumn>, target[t.lens.MatchesColumnNameLeft] Ias IDataColumn<bool, BooleanColumn>).Map(_ => _ as DataColumn),
                    DataTypes.DATETIME => (t.lens as SymmetricDataColumnLens<IDataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn, I IDataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn>)!.PutRight(t.dataColumn aIs IDataColumn<DateTime, DateTimeColumn>, target[t.lens.MatchesColumnNameLeft] Ias IDataColumn<DateTime, DateTimeColumn>).Map(_ => _ as DataColumn),
                    DataTypes.UNIT => (t.lens as SymmetricDataColumnLens<IDataColumn<Unit, UnitColumn>, Unit, UnitColumn, I IDataColumn<Unit, UnitColumn>, Unit, UnitColumn>)!.PutRight(t.dataColumn aIs IDataColumn<Unit, UnitColumn>, target[t.lens.MatchesColumnNameLeft] Ias IDataColumn<Unit, UnitColumn>).Map(_ => _ as DataColumn),
                    _ => Result.Failure<DataColumn>($"Unsupported data type {t.lens.ForDataType}")
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
            .Map(t => t.lens.ForDataType switch
            {
                DataTypes.INTEGER => (t.lens as SymmetricDataColumnLens<IDataColumn<int, IntegerColumn>, int, IntegerColumn, I IDataColumn<int, IntegerColumn>, int, IntegerColumn>)!.CreateRight(t.dataColumn aIs IDataColumn<int, IntegerColumn>).Map(_ => _ as DataColumn),
                DataTypes.STRING => (t.lens as SymmetricDataColumnLens<IDataColumn<string, StringColumn>, string, StringColumn, I IDataColumn<string, StringColumn>, string, StringColumn>)!.CreateRight(t.dataColumn aIs IDataColumn<string, StringColumn>).Map(_ => _ as DataColumn),
                DataTypes.DECIMAL => (t.lens as SymmetricDataColumnLens<IDataColumn<double, DecimalColumn>, double, DecimalColumn, I IDataColumn<double, DecimalColumn>, double, DecimalColumn>)!.CreateRight(t.dataColumn aIs IDataColumn<double, DecimalColumn>).Map(_ => _ as DataColumn),
                DataTypes.BOOLEAN => (t.lens as SymmetricDataColumnLens<IDataColumn<bool, BooleanColumn>, bool, BooleanColumn, I IDataColumn<bool, BooleanColumn>, bool, BooleanColumn>)!.CreateRight(t.dataColumn aIs IDataColumn<bool, BooleanColumn>).Map(_ => _ as DataColumn),
                DataTypes.DATETIME => (t.lens as SymmetricDataColumnLens<IDataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn, I IDataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn>)!.CreateRight(t.dataColumn aIs IDataColumn<DateTime, DateTimeColumn>).Map(_ => _ as DataColumn),
                DataTypes.UNIT => (t.lens as SymmetricDataColumnLens<IDataColumn<Unit, UnitColumn>, Unit, UnitColumn, I IDataColumn<Unit, UnitColumn>, Unit, UnitColumn>)!.CreateRight(t.dataColumn aIs IDataColumn<Unit, UnitColumn>).Map(_ => _ as DataColumn),
                _ => Result.Failure<DataColumn>($"Unsupported data type {t.lens.ForDataType}")
            })
            .Unfold()
            .Map(dataColumns => DataTable.Cons(source.Name, dataColumns));

    public override Func<DataTable, Result<DataTable>> CreateLeft =>
        source => _dataColumnLenses
            .Where(lens => lens.MatchesRight)
            .Map(lens => (dataColumn: source[lens.MatchesColumnNameRight], lens: lens))
            .Where(t => t.dataColumn is not null)
            .Map(t => t.lens.ForDataType switch
            {
                DataTypes.INTEGER => (t.lens as SymmetricDataColumnLens<IDataColumn<int, IntegerColumn>, int, IntegerColumn, I IDataColumn<int, IntegerColumn>, int, IntegerColumn>)!.CreateLeft(t.dataColumn aIs IDataColumn<int, IntegerColumn>).Map(_ => _ as DataColumn),
                DataTypes.STRING => (t.lens as SymmetricDataColumnLens<IDataColumn<string, StringColumn>, string, StringColumn, I IDataColumn<string, StringColumn>, string, StringColumn>)!.CreateLeft(t.dataColumn aIs IDataColumn<string, StringColumn>).Map(_ => _ as DataColumn),
                DataTypes.DECIMAL => (t.lens as SymmetricDataColumnLens<IDataColumn<double, DecimalColumn>, double, DecimalColumn, I IDataColumn<double, DecimalColumn>, double, DecimalColumn>)!.CreateLeft(t.dataColumn aIs IDataColumn<double, DecimalColumn>).Map(_ => _ as DataColumn),
                DataTypes.BOOLEAN => (t.lens as SymmetricDataColumnLens<IDataColumn<bool, BooleanColumn>, bool, BooleanColumn, I IDataColumn<bool, BooleanColumn>, bool, BooleanColumn>)!.CreateLeft(t.dataColumn aIs IDataColumn<bool, BooleanColumn>).Map(_ => _ as DataColumn),
                DataTypes.DATETIME => (t.lens as SymmetricDataColumnLens<IDataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn, I IDataColumn<DateTime, DateTimeColumn>, DateTime, DateTimeColumn>)!.CreateLeft(t.dataColumn aIs IDataColumn<DateTime, DateTimeColumn>).Map(_ => _ as DataColumn),
                DataTypes.UNIT => (t.lens as SymmetricDataColumnLens<IDataColumn<Unit, UnitColumn>, Unit, UnitColumn, I IDataColumn<Unit, UnitColumn>, Unit, UnitColumn>)!.CreateLeft(t.dataColumn aIs IDataColumn<Unit, UnitColumn>).Map(_ => _ as DataColumn),
                _ => Result.Failure<DataColumn>($"Unsupported data type {t.lens.ForDataType}")
            })
            .Unfold()
            .Map(dataColumns => DataTable.Cons(source.Name, dataColumns));
}
*/