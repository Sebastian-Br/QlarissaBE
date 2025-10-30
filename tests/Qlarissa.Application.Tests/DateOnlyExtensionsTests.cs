using Qlarissa.Application.Extensions;

namespace Qlarissa.Application.Tests;

public class DateOnlyExtensionsTests
{
    [Fact]
    public void DateOnly_ToDouble_Equals_ToDateOnly()
    {
        DateOnly currentDate = new(2020, 1, 1); // includes a leap year and a non leap year
        while (currentDate.Year < 2022)
        {
            Assert.Equal(currentDate, currentDate.ToDouble().ToDateOnly());
            currentDate = currentDate.AddDays(1);
        }
    }
}