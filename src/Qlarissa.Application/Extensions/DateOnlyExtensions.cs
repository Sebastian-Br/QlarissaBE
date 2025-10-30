namespace Qlarissa.Application.Extensions;

public static class DateOnlyExtensions
{
    public static double ToDouble(this DateOnly date)
    {
        int year = date.Year;
        int daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
        return year + date.DayOfYear / (double) daysInYear;
    }

    public static DateOnly ToDateOnly(this double date)
    {
        int year = (int)Math.Floor(date);
        double fractionOfYear = date - year;
        int daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
        int dayOfYear = (int)Math.Round(fractionOfYear * daysInYear);
        return new DateOnly(year, 1, 1).AddDays(dayOfYear - 1);
    }
}