namespace Bifrons.Base.Tests;

public class EitherTests
{
    [Fact]
    public void CreateEither_WithLeft()
    {
        var either = Either.Left<string, int>("left");
        Assert.True(either.IsLeft);
        Assert.False(either.IsRight);
        Assert.Equal("left", either.Left);
    }

    [Fact]
    public void CreateEither_WithRight()
    {
        var either = Either.Right<string, int>(42);
        Assert.False(either.IsLeft);
        Assert.True(either.IsRight);
        Assert.Equal(42, either.Right);
    }

    [Fact]
    public void LeftEither_MapLeft()
    {
        var either = Either.Left<string, int>("left");
        var mapped = either.MapLeft(s => s.Length);
        Assert.True(mapped.IsLeft);
        Assert.False(mapped.IsRight);
        Assert.Equal(4, mapped.Left);
    }

    [Fact]
    public void RightEither_MapLeft()
    {
        var either = Either.Right<string, int>(42);
        var mapped = either.MapLeft(s => s.Length);
        Assert.False(mapped.IsLeft);
        Assert.True(mapped.IsRight);
        Assert.Equal(42, mapped.Right);
    }

    [Fact]
    public void LeftEither_MapRight()
    {
        var either = Either.Left<string, int>("left");
        var mapped = either.MapRight(i => i * 2);
        Assert.True(mapped.IsLeft);
        Assert.False(mapped.IsRight);
        Assert.Equal("left", mapped.Left);
    }

    [Fact]
    public void RightEither_MapRight()
    {
        var either = Either.Right<string, int>(42);
        var mapped = either.MapRight(i => i * 2);
        Assert.False(mapped.IsLeft);
        Assert.True(mapped.IsRight);
        Assert.Equal(84, mapped.Right);
    }

    [Fact]
    public void LeftEither_Map()
    {
        var either = Either.Left<string, int>("left");
        var mapped = either.Map(
            s => s.Length,
            i => i * 2);
        Assert.True(mapped.IsLeft);
        Assert.False(mapped.IsRight);
        Assert.Equal(4, mapped.Left);
    }

    [Fact]
    public void RightEither_Map()
    {
        var either = Either.Right<string, int>(42);
        var mapped = either.Map(
            s => s.Length,
            i => i * 2);
        Assert.False(mapped.IsLeft);
        Assert.True(mapped.IsRight);
        Assert.Equal(84, mapped.Right);
    }

    [Fact]
    public void LeftEither_BindLeft()
    {
        var either = Either.Left<string, int>("left");
        var mapped = either.BindLeft(s => Either.Left<int, int>(s.Length));
        Assert.True(mapped.IsLeft);
        Assert.False(mapped.IsRight);
        Assert.Equal(4, mapped.Left);
    }

    [Fact]
    public void RightEither_BindLeft()
    {
        var either = Either.Right<string, int>(42);
        var mapped = either.BindLeft(s => Either.Left<int, int>(s.Length));
        Assert.False(mapped.IsLeft);
        Assert.True(mapped.IsRight);
        Assert.Equal(42, mapped.Right);
    }

    [Fact]
    public void LeftEither_BindRight()
    {
        var either = Either.Left<string, int>("left");
        var mapped = either.BindRight(i => Either.Right<string, int>(i * 2));
        Assert.True(mapped.IsLeft);
        Assert.False(mapped.IsRight);
        Assert.Equal("left", mapped.Left);
    }

    [Fact]
    public void RightEither_BindRight()
    {
        var either = Either.Right<string, int>(42);
        var mapped = either.BindRight(i => Either.Right<string, int>(i * 2));
        Assert.False(mapped.IsLeft);
        Assert.True(mapped.IsRight);
        Assert.Equal(84, mapped.Right);
    }

    [Fact]
    public void LeftEither_Bind()
    {
        var either = Either.Left<string, int>("left");
        var mapped = either.Bind(
            s => Either.Left<int, int>(s.Length),
            i => Either.Right<int, int>(i * 2));
        Assert.True(mapped.IsLeft);
        Assert.False(mapped.IsRight);
        Assert.Equal(4, mapped.Left);
    }

    [Fact]
    public void RightEither_Bind()
    {
        var either = Either.Right<string, int>(42);
        var mapped = either.Bind(
            s => Either.Left<int, int>(s.Length),
            i => Either.Right<int, int>(i * 2));
        Assert.False(mapped.IsLeft);
        Assert.True(mapped.IsRight);
        Assert.Equal(84, mapped.Right);
    }

}
