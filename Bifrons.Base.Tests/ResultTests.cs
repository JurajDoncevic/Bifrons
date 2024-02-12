namespace Bifrons.Base.Tests;

public class ResultTests
{
    [Fact]
    public void CreateSuccess_WithObject()
    {
        var result = Result.Success("Hello");
        Assert.True(result.IsSuccess);
        Assert.Equal("Hello", result.Data);
    }

    [Fact]
    public void CreateSuccess_WithValue()
    {
        int x = 42;
        var result = Result.Success(x);
        Assert.True(result.IsSuccess);
        Assert.Equal(x, result.Data);
    }


    [Fact]
    public void CreateFailure_WithValue()
    {
        var failureMessage = "Failure";
        var result = Result.Failure<int>(failureMessage);
        Assert.False(result.IsSuccess);
        Assert.Equal(failureMessage, result.Message);
    }

    [Fact]
    public void CreateFailure_WithObject()
    {
        var failureMessage = "Failure";
        var result = Result.Failure<object>(failureMessage);
        Assert.False(result.IsSuccess);
        Assert.Equal(failureMessage, result.Message);
    }

    [Fact]
    public void CreateException_WithValue()
    {
        var exception = new Exception("Failure");
        var result = Result.Exception<int>(exception);
        Assert.False(result.IsSuccess);
        Assert.Equal(exception, result.Exception);
    }

    [Fact]
    public void CreateException_WithObject()
    {
        var exception = new Exception("Failure");
        var result = Result.Exception<object>(exception);
        Assert.False(result.IsSuccess);
        Assert.Equal(exception, result.Exception);
    }

    [Fact]
    public void CreateSuccess_WithNull_AsFailure()
    {
        var result = Result.Success<string>(null!);
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
    }

    [Fact]
    public void MapSuccess_WithObject()
    {
        var x = "Hello";
        var result = Result.Success(x);
        var mapped = result.Map(s => s.Length);
        Assert.True(mapped.IsSuccess);
        Assert.Equal(x.Length, mapped.Data);
    }

    [Fact]
    public void MapSuccess_ToFailure_WithNullObject()
    {
        var x = "Hello";
        var result = Result.Success(x);
        var mapped = result
            .Map(s => s + "\n")
            .Map(i => (string)null!);
        Assert.False(mapped.IsSuccess);
    }

    [Fact]
    public void BindSuccess_WithObject()
    {
        var x = "Hello";
        var result = Result.Success(x);
        var binded = result
            .Bind(s => Result.Success(s + " "))
            .Bind(s => Result.Success(s + "World"));

        Assert.True(binded.IsSuccess);
        Assert.Equal("Hello World", binded.Data);
    }

    [Fact]
    public void BindSuccess_WithFailure()
    {
        var x = "Hello";
        var result = Result.Success(x);
        var binded = result
            .Bind(s => Result.Failure<string>("Failure"))
            .Bind(s => Result.Success(s + "World"));

        Assert.False(binded.IsSuccess);
    }

    [Fact]
    public void CreateSuccess_WithOperation()
    {
        var result = Result.AsResult<string>(() => "Hello");
        Assert.True(result.IsSuccess);
        Assert.Equal("Hello", result.Data);
    }

    [Fact]
    public void CreateFailure_WithOperation()
    {
        var result = Result.AsResult<string>(
            () =>
            {
                return Result.Failure<string>("Failure");
                #pragma warning disable CS0162
                return "Hello";
                #pragma warning restore CS0162
            });
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Failure", result.Message);
    }

        [Fact]
    public void CreateException_WithOperation()
    {
        var exception = new Exception("Exceptional failure");
        var result = Result.AsResult<string>(
            () =>
            {
                throw exception;
                #pragma warning disable CS0162
                return "Hello";
                #pragma warning restore CS0162
            });
        Assert.False(result.IsSuccess);
        Assert.True(result.IsException);
        Assert.Equal(exception, result.Exception);
    }

}
