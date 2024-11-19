using System.Globalization;

namespace BoykisserBot.Common;

/// <summary>
/// Represents a colour.
/// </summary>
public class Colour
{
    /// <summary>
    /// Red value.
    /// </summary>
    public byte Red { get; set; }

    /// <summary>
    /// Green value.
    /// </summary>
    public byte Green { get; set; }

    /// <summary>
    /// Blue value.
    /// </summary>
    public byte Blue { get; set; }

    /// <summary>
    /// Alpha value (transparency).
    /// </summary>
    public byte Alpha { get; set; }

    /// <summary>
    /// Initialises a new instance of the <see cref="Colour"/> class from a hex string.
    /// </summary>
    /// <param name="hex">Hex code</param>
    /// <exception cref="ArgumentException"></exception>
    public Colour(string hex)
    {
        // Remove the # symbol if it exists
        if (hex[0] == '#') hex = hex[1..];

        // Check if the hex code is valid (# symbol is optional)
        if (hex.Length != 6 && hex.Length != 8) throw new ArgumentException("Invalid hex code.", nameof(hex));

        Red = byte.Parse(hex[0..2], NumberStyles.HexNumber);
        Green = byte.Parse(hex[2..4], NumberStyles.HexNumber);
        Blue = byte.Parse(hex[4..6], NumberStyles.HexNumber);
        Alpha = hex.Length == 8 // Check if the hex code has an alpha value
            ? byte.Parse(hex[6..8], NumberStyles.HexNumber)
            : byte.MaxValue;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="Colour"/> class from RGB values.
    /// </summary>
    /// <param name="red">Red value.</param>
    /// <param name="green">Green value.</param>
    /// <param name="blue">Blue value.</param>
    /// <param name="alpha">Alpha value.</param>
    public Colour(byte red, byte green, byte blue, byte alpha = byte.MaxValue)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    public override string ToString()
    {
        return $"#{Red:X2}{Green:X2}{Blue:X2}{Alpha:X2}";
    }
}