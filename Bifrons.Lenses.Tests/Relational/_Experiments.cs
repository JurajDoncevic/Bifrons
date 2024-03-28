namespace Bifrons.Lenses.Relational.Tests;

using Bifrons.Lenses.Relational.Model;
using Columns = Bifrons.Lenses.Relational.Columns;
using Tables = Bifrons.Lenses.Relational.Tables;
public class _Experiments
{
    [Fact]
    public void CreateColumns()
    {
        //------------------ People table ------------------
        var col_id = Columns.IdentityLens.Cons("Id");
        var col_firstName = Columns.IdentityLens.Cons("FirstName");
        var col_lastName = Columns.IdentityLens.Cons("LastName");
        var col_dateOfBirth = Columns.RenameLens.Cons("DateOfBirth", "DoB");
        var col_address = Columns.DeleteLens.Cons("Address");
        var col_email = Columns.InsertLens.Cons("Email");
        var col_phone = Columns.DeleteLens.Cons("Phone");
        var tbl_people = Tables.IdentityLens.Cons("People", col_id, col_firstName, col_lastName, col_dateOfBirth);

        //------------------ Roles table ------------------
        var col_roleId = Columns.IdentityLens.Cons("Id");
        var col_roleName = Columns.IdentityLens.Cons("RoleName");
        var tbl_roles = Tables.IdentityLens.Cons("Roles", col_roleId, col_roleName);

        //------------------ PeopleRoles table ------------------
        var col_peopleRolesId = Columns.IdentityLens.Cons("Id");
        var col_peopleId = Columns.IdentityLens.Cons("PeopleId");
        var col_roleId2 = Columns.IdentityLens.Cons("RoleId");
        var tbl_peopleRoles = Tables.IdentityLens.Cons("PeopleRoles", col_peopleRolesId, col_peopleId, col_roleId2);

        //------------------ Departments table ------------------
        var col_departmentId = Columns.IdentityLens.Cons("Id");
        var col_departmentName = Columns.IdentityLens.Cons("DepartmentName");
        var tbl_departments = Tables.IdentityLens.Cons("Departments", col_departmentId, col_departmentName);

        //------------------ Users table ------------------
        var tbl_users = Tables.DeleteLens.Cons("Users");


        Assert.NotNull(tbl_people);
        Assert.NotNull(tbl_roles);
        Assert.NotNull(tbl_peopleRoles);
        Assert.NotNull(tbl_departments);
        Assert.NotNull(tbl_users);
    }
}
