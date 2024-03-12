namespace GreatCircle.Tests;

public class UtilitiesTests
{
    /// <summary>
    /// Check that IsCloseTo returns True for a value near the target.
    /// </summary>
    [Fact]
    public void IsCloseTo_NearTarget()
    {
        Assert.True(Utilities.IsCloseTo(1 - 1e-6, 1));
    }

    /// <summary>
    /// Check that IsCloseTo returns False for a value far from the target.
    /// </summary>
    [Fact]
    public void IsCloseTo_FarFromTarget()
    {
        Assert.False(Utilities.IsCloseTo(1 - 2.1e-6, 1));
    }

    /// <summary>
    /// Check that IsCloseTo returns True for a value near a target of 0.
    /// </summary>
    [Fact]
    public void IsCloseTo_NearZero()
    {
        Assert.True(Utilities.IsCloseTo(1e-6, 0));
    }

    /// <summary>
    /// Check that IsCloseTo returns False for a value far from a target of 0.
    /// </summary>
    [Fact]
    public void IsCloseTo_FarFromZero()
    {
        Assert.False(Utilities.IsCloseTo(1.1e-6, 0));
    }

    /// <summary>
    /// Check that AreClose returns True for two values close to each other.
    /// </summary>
    [Fact]
    public void AreClose_NearEachOther()
    {
        Assert.True(Utilities.AreClose(1 - 1e-6, 1 + 1e-6));
    }

    /// <summary>
    /// Check that AreClose returns False for two values far from each other.
    /// </summary>
    [Fact]
    public void AreClose_FarFromEachOther()
    {
        Assert.False(Utilities.AreClose(1 - 1e-6, 1 + 1.1e-6));
    }

    /// <summary>
    /// Check that AreClose returns True for two values near zero close to each other.
    /// </summary>
    [Fact]
    public void AreClose_NearZero_True()
    {
        Assert.True(Utilities.AreClose(5e-7, -5e-7));
    }

    /// <summary>
    /// Check that AreClose returns False for two values near zero far from each other.
    /// </summary>
    [Fact]
    public void AreClose_NearZero_False()
    {
        Assert.False(Utilities.AreClose(5.1e-6, -5e-7));
    }
}