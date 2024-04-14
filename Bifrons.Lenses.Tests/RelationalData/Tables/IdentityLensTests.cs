using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Columns;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.RelationalData.Tables.Tests;

public sealed class IdentityLensTests : SymmetricLensTestingFramework<TableData, TableData>
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
    private Relational.Columns.IdentityLens nameColLens => Relational.Columns.IdentityLens.Cons("Name");
    private Relational.Columns.IdentityLens dobColLens => Relational.Columns.IdentityLens.Cons("DOB");
    private Relational.Columns.IdentityLens isAdminColLens => Relational.Columns.IdentityLens.Cons("IsAdmin");
    private Relational.Columns.IdentityLens hoursClockedColLens => Relational.Columns.IdentityLens.Cons("HoursClocked");
    private Relational.Tables.IdentityLens TableLens
        => Relational.Tables.IdentityLens.Cons(
            "People",
            [IdColLens, nameColLens, dobColLens, isAdminColLens, hoursClockedColLens]);

    #endregion STRUCTURAL LENSES

    #region STRUCTURED DATA LENSES
    private IntegerIdentityLens IdLens => IntegerIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("Id"), Integers.IdentityLens.Cons());
    private StringIdentityLens NameLens => StringIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("Name"), Strings.IdentityLens.Cons());
    private DateTimeIdentityLens DobLens => DateTimeIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("DOB"), DateTimes.IdentityLens.Cons());
    private BooleanIdentityLens IsAdminLens => BooleanIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("IsAdmin"), Booleans.IdentityLens.Cons());
    private DecimalIdentityLens HoursClockedLens => DecimalIdentityLens.Cons(Relational.Columns.IdentityLens.Cons("HoursClocked"), Decimals.IdentityLens.Cons());

    #endregion STRUCTURED DATA LENSES

    protected override TableData _left =>
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
            ]
        ).Data ?? throw new Exception("Failed to create left side data.");

    protected override TableData _right =>
        TableData.Cons(
            Table.Cons(
                "People",
                [
                    IdCol,
                    NameCol,
                    DobCol,
                    IsAdminCol,
                    HoursClockedCol
                ]),
            [
            RowData.Cons(
                [
                ColumnData.Cons(IdCol, 1).Data,
                ColumnData.Cons(NameCol, "Alice").Data,
                ColumnData.Cons(DobCol, new DateTime(1990, 1, 1)).Data,
                ColumnData.Cons(IsAdminCol, true).Data,
                ColumnData.Cons(HoursClockedCol, 37.0).Data
                ]),
            ]
        ).Data ?? throw new Exception("Failed to create right side data.");

    private TableData Updated =>
        TableData.Cons(
            Table,
            [
            RowData.Cons(
                [
                ColumnData.Cons(IdCol, 1).Data,
                ColumnData.Cons(NameCol, "Bob").Data,
                ColumnData.Cons(DobCol, new DateTime(1992, 12, 31)).Data,
                ColumnData.Cons(IsAdminCol, false).Data,
                ColumnData.Cons(HoursClockedCol, 42.0).Data
                ]),
            ]
        ).Data ?? throw new Exception("Failed to create updated left side data.");

    protected override (TableData originalSource, TableData expectedOriginalTarget, TableData updatedTarget, TableData expectedUpdatedSource) _roundTripWithRightSideUpdateData 
        => (_left, _right, Updated, Updated) ;

    protected override (TableData originalSource, TableData expectedOriginalTarget, TableData updatedTarget, TableData expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (_right, _left, Updated, Updated);

    protected override ISymmetricLens<TableData, TableData> _lens
        => IdentityLens.Cons(
            TableLens,
            [
                IdLens,
                NameLens,
                DobLens,
                IsAdminLens,
                HoursClockedLens
            ]).Data ?? throw new Exception("Failed to create TableData IdentityLens.");
}
