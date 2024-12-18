using System.Text.RegularExpressions;

namespace AoC24.Solutions.Day17.Models;

public partial class Patterns
{
    [GeneratedRegex(@"Register\sA:\s*(?<a>\d+)[\s\r\n]*Register\sB\:\s*(?<b>\d+)[\s\r\n]*Register\sC\:\s*(?<c>\d+)[\s\r\n]*Program\:\s*(?<program>[\d\,]+)")]
    public static partial Regex InitializationInfo();
}
