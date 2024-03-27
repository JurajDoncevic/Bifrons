using Bifrons.Lenses.Symmetric.Relational.Data.Columns;
using Bifrons.Lenses.Symmetric.Relational.Data.Model;
using Bifrons.Lenses.Symmetric.Relational.Tables;

namespace Bifrons.Lenses.Symmetric.Relational.Data.Tables;

public abstract class DataTableLens : ISymmetricLens<DataTable, DataTable>
{
    protected readonly SymmetricTableLens _tableLens;
    protected readonly List<IDataColumnLens> _dataColumnLenses;

    public SymmetricTableLens TableLens => _tableLens;

    public IReadOnlyList<IDataColumnLens> DataColumnLenses => _dataColumnLenses;

    protected DataTableLens(SymmetricTableLens tableLens, IEnumerable<IDataColumnLens> dataColumnLenses)
    {
        _tableLens = tableLens;
        _dataColumnLenses = dataColumnLenses.ToList();
    }

    public abstract Func<DataTable, Option<DataTable>, Result<DataTable>> PutLeft { get; }

    public abstract Func<DataTable, Option<DataTable>, Result<DataTable>> PutRight { get; }

    public abstract Func<DataTable, Result<DataTable>> CreateRight { get; }

    public abstract Func<DataTable, Result<DataTable>> CreateLeft { get; }


}