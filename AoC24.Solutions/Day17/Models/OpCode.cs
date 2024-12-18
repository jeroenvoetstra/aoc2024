namespace AoC24.Solutions.Day17.Models;

public enum OpCode
{
    /// <summary>
    /// Division, [A] / 2^operand (combo) => [A]
    /// </summary>
    /// <remarks>Truncate value to int</remarks>
    adv = 0,

    /// <summary>
    /// Bitwise XOR, [B] XOR operand (literal) => [B]
    /// </summary>
    bxl = 1,

    /// <summary>
    /// Modulo 8, operand (combo) % 8 => [B]
    /// </summary>
    bst = 2,

    /// <summary>
    /// Jump, if [A] != 0, {instruction pointer} = operand (literal)
    /// </summary>
    /// <remarks>Do not advance instruction pointer</remarks>
    jnz = 3,

    /// <summary>
    /// Bitwise XOR, [B] XOR [C] => [B]
    /// </summary>
    /// <remarks>Ignores operand</remarks>
    bxc = 4,

    /// <summary>
    /// Modulo 8, operand (combo) % 8 => OUTPUT
    /// </summary>
    @out = 5,

    /// <summary>
    /// Division, [A] / 2^operand (combo) => [B]
    /// </summary>
    /// <remarks>Truncate value to int</remarks>
    bdv = 6,

    /// <summary>
    /// Division, [A] / 2^operand (combo) => [C]
    /// </summary>
    /// <remarks>Truncate value to int</remarks>
    cdv = 7,
}
