namespace GreatCircle.Tests;

public class MyMathTests
{
    [Fact]
    public void IsCloseTo_NearTarget()
    {
        Assert.True(MyMathUtils.MyMath.IsCloseTo(1 - 1e-6, 1));
    }

    [Fact]
    public void IsCloseTo_FarFromTarget()
    {
        Assert.False(MyMathUtils.MyMath.IsCloseTo(1 - 2.1e-6, 1));
    }

    [Fact]
    public void IsCloseTo_NearZero()
    {
        Assert.True(MyMathUtils.MyMath.IsCloseTo(1e-6, 0));
    }

    [Fact]
    public void IsCloseTo_FarFromZero()
    {
        Assert.False(MyMathUtils.MyMath.IsCloseTo(1.1e-6, 0));
    }

    [Fact]
    public void AreClose_NearEachOther()
    {
        Assert.True(MyMathUtils.MyMath.AreClose(1 - 1e-6, 1 + 1e-6));
    }

    [Fact]
    public void AreClose_FarFromEachOther()
    {
        Assert.False(MyMathUtils.MyMath.AreClose(1 - 1e-6, 1 + 1.1e-6));
    }

    [Fact]
    public void AreClose_NearZero_True()
    {
        Assert.True(MyMathUtils.MyMath.AreClose(5e-7, -5e-7));
    }

    [Fact]
    public void AreClose_NearZero_False()
    {
        Assert.False(MyMathUtils.MyMath.AreClose(5.1e-6, -5e-7));
    }
}