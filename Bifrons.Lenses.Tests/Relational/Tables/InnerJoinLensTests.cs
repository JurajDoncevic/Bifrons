using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Relational.Tables.Tests;

public class InnerJoinLensTests : SymmetricLensTestingFramework<(Table, Table), Table>
{
    protected override (Table, Table) _left => 
        (Table.Cons("People",
        [
            Column.Cons("IdPerson", DataTypes.INTEGER),
            Column.Cons("PersonName", DataTypes.STRING),
            Column.Cons("PersonDoB", DataTypes.DATETIME),
            Column.Cons("PersonAddress", DataTypes.STRING),
            Column.Cons("DepartmentId", DataTypes.INTEGER)
        ]),
        Table.Cons("Departments", 
        [
            Column.Cons("IdDepartment", DataTypes.INTEGER),
            Column.Cons("DepartmentName", DataTypes.STRING),
            Column.Cons("DepartmentAddress", DataTypes.STRING)
        ]));

    protected override Table _right
        => Table.Cons("PeopleWithDepartments",
        [
            Column.Cons("IdPerson", DataTypes.INTEGER),
            Column.Cons("PersonName", DataTypes.STRING),
            Column.Cons("PersonDoB", DataTypes.DATETIME),
            Column.Cons("PersonAddress", DataTypes.STRING),
            Column.Cons("DepartmentId", DataTypes.INTEGER),
            Column.Cons("IdDepartment", DataTypes.INTEGER),
            Column.Cons("DepartmentName", DataTypes.STRING),
            Column.Cons("DepartmentAddress", DataTypes.STRING)
        ]);

    protected override ((Table, Table) originalSource, Table expectedOriginalTarget, Table updatedTarget, (Table, Table) expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => (
            _left,
            _right,
            Table.Cons("PeopleWithDepartments",
            [
                Column.Cons("IdPerson", DataTypes.INTEGER),
                Column.Cons("PersonName", DataTypes.STRING),
                Column.Cons("PersonDoB", DataTypes.DATETIME),
                Column.Cons("PersonAddress", DataTypes.STRING),
                Column.Cons("DepartmentId", DataTypes.INTEGER),
                Column.Cons("IdDepartment", DataTypes.INTEGER),
                Column.Cons("DepartmentName", DataTypes.STRING),
                Column.Cons("DepartmentAddress", DataTypes.STRING)
            ]),
            _left
        );

    protected override (Table originalSource, (Table, Table) expectedOriginalTarget, (Table, Table) updatedTarget, Table expectedUpdatedSource) _roundTripWithLeftSideUpdateData 
        => (
            _right,
            _left,
            (
                Table.Cons("People",
                [
                    Column.Cons("IdPerson", DataTypes.INTEGER),
                    Column.Cons("PersonName", DataTypes.STRING),
                    Column.Cons("PersonDoB", DataTypes.DATETIME),
                    Column.Cons("PersonAddress", DataTypes.STRING),
                    Column.Cons("DepartmentId", DataTypes.INTEGER)
                ]),
                Table.Cons("Departments", 
                [
                    Column.Cons("IdDepartment", DataTypes.INTEGER),
                    Column.Cons("DepartmentName", DataTypes.STRING),
                    Column.Cons("DepartmentAddress", DataTypes.STRING)
                ])
            ),
            _right
        );

    protected override ISymmetricLens<(Table, Table), Table> _lens =>
        InnerJoinLens.Cons("PeopleWithDepartments", Column.Cons("DepartmentId", DataTypes.INTEGER), Column.Cons("IdDepartment", DataTypes.INTEGER));
}
