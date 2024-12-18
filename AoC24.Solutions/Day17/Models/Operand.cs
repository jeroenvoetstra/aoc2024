namespace AoC24.Solutions.Day17.Models;

public record Operand(OperandType Type, byte Value)
{
    public static OperandType GetOperandType(OpCode opCode) => opCode switch
    {
        OpCode.bxl or OpCode.jnz => OperandType.Literal,
        _ => OperandType.Combo,
    };
}
