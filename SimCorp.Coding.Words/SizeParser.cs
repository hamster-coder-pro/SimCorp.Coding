using System.Globalization;

namespace SimCorp.Coding.Words;

public static class SizeParser
{
    // Define multipliers for each unit
    // ReSharper disable InconsistentNaming
    private const long KB = 1L << 10; // 2^10
    private const long MB = 1L << 20; // 2^20
    private const long GB = 1L << 30; // 2^30
    private const long TB = 1L << 40; // 2^40
    // ReSharper restore InconsistentNaming

    public static long ParseSizeString(string size)
    {
        if (string.IsNullOrWhiteSpace(size))
            throw new ArgumentException("Size string cannot be null or empty.");

        // Normalize input (trim and make lowercase for case-insensitivity)
        size = size.Trim().ToLowerInvariant();

        // Extract the numeric part and the unit part
        int index = 0;
        while (index < size.Length && (char.IsDigit(size[index]) || size[index] == '.'))
            index++;

        if (index == 0)
            throw new FormatException("No numeric value found in size string.");

        string numberPart = size.Substring(0, index);
        string unitPart   = size.Substring(index).Trim();

        // Parse the numeric value
        if (!double.TryParse(numberPart, NumberStyles.Float, CultureInfo.InvariantCulture, out double number))
            throw new FormatException("Invalid numeric value in size string.");


        // Match the unit part to a multiplier
        return unitPart switch
               {
                   "kb" => (long)(number * KB),
                   "mb" => (long)(number * MB),
                   "gb" => (long)(number * GB),
                   "tb" => (long)(number * TB),
                   ""   => (long)number, // No unit means bytes
                   _    => throw new FormatException($"Unknown unit: '{unitPart}'.")
               };
    }

    public static string ToSizeString(long bytes)
    {
        if (bytes > 1000 * GB)
        {
            var value = (double)bytes / TB;
            return $"{value:N2}TB";
        }

        if (bytes > 1000 * MB)
        {
            var value = (double)bytes / GB;
            return $"{value:N2}GB";
        }

        if (bytes > 1000 * KB)
        {
            var value = (double)bytes / MB;
            return $"{value:N2}MB";
        }

        if (bytes > 1000)
        {
            var value = (double)bytes / KB;
            return $"{value:N2}KB";
        }

        return $"{bytes:N0}";
    }
}