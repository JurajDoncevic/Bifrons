using System;
using Integers = Bifrons.Lenses.Integers;
using Strings = Bifrons.Lenses.Strings;
using CrossType = Bifrons.Lenses.CrossType;
using DateTimes = Bifrons.Lenses.DateTimes;
using Columns = Bifrons.Lenses.Relational.Columns;
using Tables = Bifrons.Lenses.Relational.Tables;
using Bifrons.Lenses;


namespace Bifrons.Experiments;

public sealed class PaperExamples
{
    [Fact]
    public void Paper_BeautifyExample()
    {
        var csvLine = "John;Doe;35;New York";
        var exp_csvLine = "John;Doe;35;New York";
        var exp_beautified = "Name: John Doe, Age: 35, City: New York";

        var l_semi = Strings.DeleteLens.Cons(";");
        var l_space = Strings.InsertLens.Cons(" ");
        var l_comma = Strings.InsertLens.Cons(", ");
        var l_name = Strings.IdentityLens.Cons(@"[a-zA-Z]+");
        var l_nameT = Strings.InsertLens.Cons("Name: ");
        var l_age = Strings.IdentityLens.Cons(@"\d+");
        var l_ageT = Strings.InsertLens.Cons("Age: ");
        var l_city = Strings.IdentityLens.Cons(@"[a-zA-Z ]+");
        var l_cityT = Strings.InsertLens.Cons("City: ");

        var l_beautify = l_nameT & l_name & l_semi & l_space & l_name & l_semi & l_comma  // name
            & l_ageT & l_age & l_semi & l_comma // age
            & l_cityT & l_city; // city

        var res_beautified = l_beautify.CreateRight(csvLine);
        Assert.True(res_beautified);
        var beautified = res_beautified.Data;
        Assert.Equal(exp_beautified, beautified);

        var res_csvLine = l_beautify.CreateLeft(beautified);
        Assert.True(res_csvLine);
        var resultingCsvLine = res_csvLine.Data;
        Assert.Equal(exp_csvLine, resultingCsvLine);
    }

    [Fact]
    public void Paper_OtherTypesExample()
    {
        var l_id = Integers.IdentityLens.Cons();
        var l_add1 = Integers.AddLens.Cons(1);
        var l_add5 = Integers.AddLens.Cons(5);
        var l_sub3 = Integers.SubLens.Cons(3);

        var l_comp = l_id >> l_add1 >> l_add5 >> l_sub3;

        int left = 10;
        int expectedRight = 13;
        int expectedUpdatedLeft = 8;

        var right = l_comp.CreateRight(left); // 10 + (1 + 5 - 3) = 13
        Assert.True(right);
        Assert.Equal(expectedRight, right.Data);

        var updatedRight = right.Data - 2; // 13 - 2 = 11 
        var updatedLeft = l_comp.PutLeft(updatedRight, left); // (-1 -5 + 3) + 11 = 8

        Assert.True(updatedLeft);
        Assert.Equal(expectedUpdatedLeft, updatedLeft.Data);

        // CrossType example
        var l_intStr = CrossType.IntegerStringLens.Cons();
        var l_compStr = Combinators.Compose(l_comp, l_intStr);
        var rightString = l_compStr.CreateRight(left); // 10+(1+5-3)=13>>"13"

        Assert.True(rightString);
        Assert.Equal("13", rightString.Data);


    }

    [Fact]
    public void Paper_Combinators()
    {
        // (str<=>datetime)>>(datetime<=>int)>>(int<=>int)
        var l_ifStr = Combinators.Compose(
            Combinators.Compose(
                CrossType.StringDateTimeLens.Cons(),
                DateTimes.DayLens.Cons()
            ),
            Integers.SubLens.Cons(1)
        );
        // (datetime<=>int)>>(int<=>int)
        var l_ifDateTime = Combinators.Compose(
            DateTimes.DayLens.Cons(),
            Integers.SubLens.Cons(1)
        );
        // (str<=>datetime)>>(datetime<=>int)>>(int<=>int) | (datetime<=>int)>>(int<=>int)
        var l_or = Combinators.Or(l_ifStr, l_ifDateTime);

        var dateTimeString = Either.Left<string, DateTime>(new DateTime(1992, 12, 31).ToString("yyyy-MM-ddTHH:mm:ss")); // as string
        var dateTime = Either.Right<string, DateTime>(new DateTime(1992, 12, 31)); // as DateTime

        var res_fromString = l_or.CreateRight(dateTimeString);
        var res_fromDateTime = l_or.CreateRight(dateTime);

        Assert.True(res_fromString);
        Assert.True(res_fromString.Data.IsLeft);
        Assert.Equal(30, res_fromString.Data.Left);

        Assert.True(res_fromDateTime);
        Assert.True(res_fromDateTime.Data.IsRight);
        Assert.Equal(30, res_fromDateTime.Data.Right);

        var res_returnOriginalDateString = l_or.PutLeft(
            Either.Left<int, int>(res_fromString.Data.Left),
            dateTimeString
            );

        Assert.True(res_returnOriginalDateString);
        Assert.True(res_returnOriginalDateString.Data.IsLeft);
        Assert.Equal(dateTimeString, res_returnOriginalDateString.Data);
    }

    [Fact]
    public void Paper_RelationalLensesExample()
    {
        //------------------ People table ------------------
        var col_id = Columns.IdentityLens.Cons("Id");
        var col_firstName = Columns.IdentityLens.Cons("FirstName");
        var col_lastName = Columns.IdentityLens.Cons("LastName");
        var col_dateOfBirth = Columns.RenameLens.Cons("DateOfBirth", "DoB");
        var col_address = Columns.DeleteLens.Cons("Address");
        var col_email = Columns.InsertLens.Cons("Email");
        var col_phone = Columns.DeleteLens.Cons("Phone");
        var tbl_people = Tables.IdentityLens.Cons("People", col_id, col_firstName, col_lastName, col_dateOfBirth, col_address, col_email, col_phone);

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
