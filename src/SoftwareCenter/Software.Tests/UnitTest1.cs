namespace Software.Tests;

public class UnitTest1
{
    [Fact]
    public void WeCanAddTenAndTwenty()
    {
        // Given - Arrange
        int a = 10, b = 20, answer;

        // When - Act
        answer = a + b;

        // Then - Assert
        Assert.Equal(30, answer);
    }

    [Theory]
    [InlineData(10,20,30)]
    [InlineData(2,2,4)]
    [InlineData(3,3,6)]
    public void WeCanAddAnyTwoIntegers(int a, int b, int expected)
    {
        var answer = a + b;
        Assert.Equal(expected, answer);
    }
}
