using Bifrons.Lenses.RelationalData.Columns;

namespace Bifrons.Lenses.Strings.Tests;

public class Experiments
{
    [Fact]
    public void CreateRight_WithConcatLens()
    {
        var lens = IdentityLens.Cons(@"\w+") &
            IdentityLens.Cons(@"\s+") &
            IdentityLens.Cons(@"(?!\w+\s+)\w+") &
            DeleteLens.Cons(@"(?!\w+\s+):\s+") &
            InsertLens.Cons(@",");

        var source = "Jane Doe: ";
        var expectedTarget = "Jane Doe,";

        var target = lens.CreateRight(source);

        Assert.Equal(expectedTarget, target);

    }

    [Fact]
    public void PutRight_OnConcatLens_WithNoOriginalSource()
    {
        var lens = IdentityLens.Cons(@"\w+") &
            IdentityLens.Cons(@"\s+") &
            IdentityLens.Cons(@"(?!\w+\s+)\w+") &
            DeleteLens.Cons(@"(?!\w+\s+):\s+") &
            InsertLens.Cons(@",");

        var source = "Jane Doe: ";
        var expectedTarget = "Jane Doe,";

        var target = lens.PutRight(source, Option.None<string>());

        Assert.Equal(expectedTarget, target);

    }

    [Fact]
    public void RoundTrip_OnEnumerateLens()
    {
        var source = "John;Paul;Alice;George;Dicky;Stuart;Pete";
        var target = new[] { "John", "Paul", "Alice", "George", "Dicky", "Stuart", "Pete" };
        var updatedTarget = new[] { "John", "George", "Alice", "Dicky", "Pete", "Gregory" };
        var updatedSource = "John;George;Alice;Dicky;Pete;Gregory";

        var lens = Combinators.Enumerate(";", IdentityLens.Cons(@"\w+"));

        var result = lens.CreateRight(source)
            .Bind(right => lens.PutLeft(updatedTarget, Option.Some(source)));

        Assert.True(result);
        Assert.Equal(updatedSource, result.Data);
    }

    [Fact]
    public void RoundTripCSV_OnEnumerateLens()
    {
        string sourceData =
            "John;Doe;1985;Engineering;50000\n" +
            "Jane;Smith;1990;Marketing;60000\n" +
            "Mike;Johnson;1982;Finance;70000\n" +
            "Emily;Williams;1995;Human Resources;55000";

        var targetData = new[]
        {
            "John Doe 1985 Engineering 50000",
            "Jane Smith 1990 Marketing 60000",
            "Mike Johnson 1982 Finance 70000",
            "Emily Williams 1995 Human Resources 55000"

        };

        var lens = Lenses.Combinators.Compose(
            EnumerateLens.Cons(
                "\n",
                IdentityLens.Cons(@"[^;]+")
                & DeleteLens.Cons(";")
                & InsertLens.Cons(@" ")
                & IdentityLens.Cons(@"[^;]+")
                & DeleteLens.Cons(";")
                & InsertLens.Cons(@" ")
                & IdentityLens.Cons(@"\d+")
                & DeleteLens.Cons(";")
                & InsertLens.Cons(@" ")
                & IdentityLens.Cons(@"[^;]+")
                & DeleteLens.Cons(";")
                & InsertLens.Cons(@" ")
                & IdentityLens.Cons(@"[^\n]+")),
            JoinLens.Cons("\n", IdentityLens.Cons(@"[^\n]+"))
        );

        var result = lens.CreateRight(sourceData);

        Assert.True(result);
        Assert.Equal(string.Join("\n", targetData), result.Data);
    }

    [Fact]
    public void RoundTrip_OnOrLens_WithImplicitMerge()
    {
        var sourceLeft = "1234";
        var sourceRight = "abcd";

        var targetLeft = "1234";
        var targetRight = "abcd";

        SymmetricStringLens lens = IdentityLens.Cons(@"\d+") | IdentityLens.Cons(@"[a-zA-Z]+"); // Implicit merge

        var resultLeft = lens.CreateRight(sourceLeft);
        var resultRight = lens.CreateRight(sourceRight);

        Assert.True(resultLeft);
        Assert.True(resultRight);
        Assert.Equal(targetLeft, resultLeft.Data);
        Assert.Equal(targetRight, resultRight.Data);
    }

    [Fact]
    public void ManagementAndHR_DataSync()
    {
        var managementData = new[] {
            "Jane Doe: 38000",
            "John Public: 37500",
            "Mike Johnson: 40000",
            "Emily Williams: 35000",
            "Gregory Smith: 45000"
        };

        var HRData = new[] {
            "Jane Doe,Healthcare Inc.",
            "John Public,Insurance Co.",
            "Mike Johnson,Healthcare Inc.",
            "Emily Williams,Based Co.",
            "Gregory Smith,Insurance Co."
        };


        var nameRegex = @"[a-zA-Z]+";
        var salaryRegex = @"\d+";
        var companyRegex = @"[a-zA-Z \.]+";

        var nameLens = IdentityLens.Cons(nameRegex) & IdentityLens.Cons(" ") & IdentityLens.Cons(nameRegex) & DeleteLens.Cons(": ") & InsertLens.Cons(",");
        var employeeLens = nameLens & DisconnectLens.Cons(salaryRegex, "", "unk", "") & DisconnectLens.Cons("", companyRegex, "", "UNK");

        foreach (var (managementRow, hrRow) in Enumerable.Zip(managementData, HRData))
        {
            var result = employeeLens.PutRight(managementRow, hrRow);
            Assert.True(result);
            Assert.Equal(hrRow, result.Data);
        }

    }

    [Fact]
    public void ManagementAndHR_RoundTrip_WithRightUpdate()
    {
        var managementData = new[] {
            "Jane Doe: 38000",
            "John Public: 37500",
            "Mike Johnson: 40000",
            "Emily Williams: 35000",
            "Gregory Smith: 45000"
        };

        var HRData = new[] {
            "Jane Doe,Healthcare Inc.",
            "John Public,Insurance Co.",
            "Mike Johnson,Healthcare Inc.",
            "Emily Williams,Based Co.",
            "Gregory Smith,Insurance Co."
        };

        var updatedHRData = new[] {
            "Janine Doe,Healthcare Inc.",
            "John Private,Insurance Co.",
            "Michael Johnson,HealthUncare Inc.",
            "Emily Bills,Based Co.",
            "Gregor Smithe,Insurance Co."
        };

        var updatedManagementData = new[] {
            "Janine Doe: 38000",
            "John Private: 37500",
            "Michael Johnson: 40000",
            "Emily Bills: 35000",
            "Gregor Smithe: 45000"
        };

        var nameRegex = @"[a-zA-Z]+";
        var salaryRegex = @"\d+";
        var companyRegex = @"[a-zA-Z \.]+";

        var nameLens =
            IdentityLens.Cons(nameRegex) &
            IdentityLens.Cons(" ") &
            IdentityLens.Cons(nameRegex) &
            DeleteLens.Cons(": ") &
            InsertLens.Cons(",");

        var employeeLens =
            nameLens &
            DisconnectLens.Cons(salaryRegex, "", "unk", "") &
            DisconnectLens.Cons("", companyRegex, "", "UNK");

        var fullLens = Lenses.Combinators.Iterate(employeeLens);

        var result = fullLens.PutLeft(updatedHRData, managementData);

        Assert.True(result);
        Assert.Equal(updatedManagementData, result.Data);

    }

    [Fact]
    public void ManagementAndHR_RoundTrip_WithLeftUpdate()
    {
        var managementData = new[] {
            "Jane Doe: 38000",
            "John Public: 37500",
            "Mike Johnson: 40000",
            "Emily Williams: 35000",
            "Gregory Smith: 45000"
        };

        var HRData = new[] {
            "Jane Doe,Healthcare Inc.",
            "John Public,Insurance Co.",
            "Mike Johnson,Healthcare Inc.",
            "Emily Williams,Based Co.",
            "Gregory Smith,Insurance Co."
        };

        var updatedHRData = new[] {
            "Janine Doe,Healthcare Inc.",
            "John Private,Insurance Co.",
            "Michael Johnson,Healthcare Inc.",
            "Emily Bills,Based Co.",
            "Gregor Smithe,Insurance Co."
        };

        var updatedManagementData = new[] {
            "Janine Doe: 38000",
            "John Private: 37500",
            "Michael Johnson: 40000",
            "Emily Bills: 35000",
            "Gregor Smithe: 45000"
        };

        var nameRegex = @"[a-zA-Z]+";
        var salaryRegex = @"\d+";
        var companyRegex = @"[a-zA-Z \.]+";

        var nameLens = IdentityLens.Cons(nameRegex) & IdentityLens.Cons(" ") & IdentityLens.Cons(nameRegex) & DeleteLens.Cons(": ") & InsertLens.Cons(",");
        var employeeLens = nameLens & DisconnectLens.Cons(salaryRegex, "", "unk", "") & DisconnectLens.Cons("", companyRegex, "", "UNK");

        var fullLens = Lenses.Combinators.Iterate(employeeLens);

        var result = fullLens.PutRight(updatedManagementData, HRData);

        Assert.True(result);
        Assert.Equal(updatedHRData, result.Data);
    }

    [Fact]
    public void Inverse_OnIterateLens()
    {
        //var source = "John;Paul;Alice;George;Dicky;Stuart;Pete";
        var target = new[] { "John", "Paul", "Alice", "George", "Dicky", "Stuart", "Pete" };
        var updatedTarget = new[] { "John", "George", "Alice", "Dicky", "Pete", "Gregory" };
        var updatedSource = "John;George;Alice;Dicky;Pete;Gregory";

        var lens = Lenses.Combinators.Invert(Combinators.Enumerate(";", IdentityLens.Cons(@"\w+")));

        var result = lens.CreateRight(target)
            .Bind(right => lens.PutLeft(updatedSource, Option.Some(Enumerable.AsEnumerable(target))));

        Assert.True(result);
        Assert.Equal(updatedTarget, result.Data);
    }

    [Fact]
    public void Paper_BeautifyExample()
    {
        var csvLine = "John;Doe;35;New York";
        var exp_csvLine = "John;Doe;35;New York";
        var exp_beautified = "Name: John Doe, Age: 35, City: New York";

        var l_semi = DeleteLens.Cons(";");
        var l_space = InsertLens.Cons(" ");
        var l_comma = InsertLens.Cons(", ");
        var l_name = IdentityLens.Cons(@"[a-zA-Z]+");
        var l_nameT = InsertLens.Cons("Name: ");
        var l_age = IdentityLens.Cons(@"\d+");
        var l_ageT = InsertLens.Cons("Age: ");
        var l_city = IdentityLens.Cons(@"[a-zA-Z ]+");
        var l_cityT = InsertLens.Cons("City: ");

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
}
