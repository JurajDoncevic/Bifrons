using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses;

public sealed class Tuple
{
    private readonly List<Column> _columns;

    public IReadOnlyList<Column> Columns => _columns;

    public Tuple(List<Column> columns)
    {
        _columns = columns;
    }
}
