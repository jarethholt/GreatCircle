namespace GreatCircle.Tests;

public class UtilitiesTests
{
    [Fact]
    public void IsCloseTo_NearTarget()
    {
        Assert.True(Utilities.IsCloseTo(1 - 1e-6, 1));
    }

    [Fact]
    public void IsCloseTo_FarFromTarget()
    {
        Assert.False(Utilities.IsCloseTo(1 - 2.1e-6, 1));
    }

    [Fact]
    public void IsCloseTo_NearZero()
    {
        Assert.True(Utilities.IsCloseTo(1e-6, 0));
    }

    [Fact]
    public void IsCloseTo_FarFromZero()
    {
        Assert.False(Utilities.IsCloseTo(1.1e-6, 0));
    }

    [Fact]
    public void AreClose_NearEachOther()
    {
        Assert.True(Utilities.AreClose(1 - 1e-6, 1 + 1e-6));
    }

    [Fact]
    public void AreClose_FarFromEachOther()
    {
        Assert.False(Utilities.AreClose(1 - 1e-6, 1 + 1.1e-6));
    }

    [Fact]
    public void AreClose_NearZero_True()
    {
        Assert.True(Utilities.AreClose(5e-7, -5e-7));
    }

    [Fact]
    public void AreClose_NearZero_False()
    {
        Assert.False(Utilities.AreClose(5.1e-6, -5e-7));
    }
}