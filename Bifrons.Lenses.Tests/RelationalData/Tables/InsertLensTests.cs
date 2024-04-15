using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Columns;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.RelationalData.Tables.Tests;

public sealed class InsertLensTests : SymmetricLensTestingFramework<TableData, TableData>
{
    #region STRUCTURE
    private Column IdCol => IntegerColumn.Cons("Id");
    private Column NameCol => StringColumn.Cons("Name");
    private Column DobCol => DateTimeColumn.Cons("DOB");
    private Column IsAdminCol => BooleanColumn.Cons("IsAdmin");
    private Column HoursClockedCol => DecimalColumn.Cons("HoursClocked");

    private Table Table
        => Table.Cons(
            "People",
            [
                IdCol,
                NameCol,
                DobCol,
                IsAdminCol,
                HoursClockedCol
            ]);
    #endregion STRUCTURE

    #region STRUCTURAL LENSES

    private Relational.Columns.IdentityLens IdColLens => Relational.Columns.IdentityLens.Cons("Id");
    private Relational.Columns.IdentityLens NameColLens => Relational.Columns.IdentityLens.Cons("Name");
    private Relational.Columns.IdentityLens DobColLens => Relational.Columns.IdentityLens.Cons("DOB");
    private Relational.Columns.IdentityLens IsAdminColLens => Relational.Columns.IdentityLens.Cons("IsAdmin");
    private Relational.Columns.IdentityLens HoursClockedColLens => Relational.Columns.IdentityLens.Cons("HoursClocked");
    private Relational.Tables.InsertLens TableLens
        => Relational.Tables.InsertLens.Cons("People", Table);

    #endregion STRUCTURAL LENSES

    #region STRUCTURED DATA LENSES
    private IntegerIdentityLens IdLens => IntegerIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("Id"), Integers.IdentityLens.Cons());
    private StringIdentityLens NameLens => StringIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("Name"), Strings.IdentityLens.Cons());
    private DateTimeIdentityLens DobLens => DateTimeIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("DOB"), DateTimes.IdentityLens.Cons());
    private BooleanIdentityLens IsAdminLens => BooleanIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("IsAdmin"), Booleans.IdentityLens.Cons());
    private DecimalIdentityLens HoursClockedLens => DecimalIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("HoursClocked"), Decimals.IdentityLens.Cons());

    #endregion STRUCTURED DATA LENSES

    protected override TableData _left => 
        TableData.ConsUnit().Data ?? throw new Exception("Failed to create left side data.");

    protected override TableData _right =>
        TableData.Cons(
            Table
        ).Data ?? throw new Exception("Failed to create right side data.");

    private TableData Updated =>
        TableData.Cons(
            Table,
            [
            RowData.Cons(
                [
                ColumnData.Cons(IdCol, 1).Data,
                ColumnData.Cons(NameCol, "Alice").Data,
                ColumnData.Cons(DobCol, new DateTime(1990, 1, 1)).Data,
                ColumnData.Cons(IsAdminCol, true).Data,
                ColumnData.Cons(HoursClockedCol, 37.0).Data
                ]),
            RowData.Cons(
                [
                ColumnData.Cons(IdCol, 2).Data,
                ColumnData.Cons(NameCol, "Robert").Data,
                ColumnData.Cons(DobCol, new DateTime(1992, 12, 31)).Data,
                ColumnData.Cons(IsAdminCol, false).Data,
                ColumnData.Cons(HoursClockedCol, 42.0).Data
                ]),
            RowData.Cons(
                [
                ColumnData.Cons(IdCol, 4).Data,
                ColumnData.Cons(NameCol, "David").Data,
                ColumnData.Cons(DobCol, new DateTime(1998, 3, 22)).Data,
                ColumnData.Cons(IsAdminCol, true).Data,
                ColumnData.Cons(HoursClockedCol, 45.0).Data
                ])
            ]
        ).Data ?? throw new Exception("Failed to create updated left side data.");

    protected override (TableData originalSource, TableData expectedOriginalTarget, TableData updatedTarget, TableData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, Updated, _left);

    protected override (TableData originalSource, TableData expectedOriginalTarget, TableData updatedTarget, TableData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, Updated, _right);

    protected override ISymmetricLens<TableData, TableData> _lens 
        => RelationalData.Tables.InsertLens.Cons(TableLens);
}
