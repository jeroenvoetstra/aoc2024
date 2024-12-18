namespace AoC24.Solutions.Day17.Models;

public class ChronospatialComputer
{
    private int _instructionPointer;
    private List<Instruction> _instructions;
    private bool _running;

    public int RegisterA { get; private set; }
    public int RegisterB { get; private set; }
    public int RegisterC { get; private set; }

    public ChronospatialComputer(int registerA, int registerB, int registerC)
    {
        _instructionPointer = 0;
        _instructions = new List<Instruction>();

        RegisterA = registerA;
        RegisterB = registerB;
        RegisterC = registerC;
    }

    public void LoadProgram(byte[] input)
    {
        _instructions.Clear();
        _instructions.AddRange(
            Enumerable.Range(0, input.Length / 2).Select((index) =>
            {
                var opCode = (OpCode)input[index * 2];
                return new Instruction(
                    opCode,
                    new Operand(Operand.GetOperandType(opCode), input[(index * 2) + 1])
                    );

            }
            ));
    }

    public IEnumerable<int> GetOutput()
    {
        _running = true;
        while (_running)
        {
            if (_instructionPointer < 0 || _instructionPointer >= _instructions.Count)
            {
                _running = false;
                yield break;
            }

            var currentInstruction = _instructions[_instructionPointer];
            var operand = currentInstruction.Operand;
            switch (currentInstruction.OpCode)
            {
                case OpCode.adv:
                    RegisterA = (int)(RegisterA / Math.Pow(2, GetOperandValue(operand)));
                    _instructionPointer++;
                    break;
                case OpCode.bdv:
                    RegisterB = (int)(RegisterA / Math.Pow(2, GetOperandValue(operand)));
                    _instructionPointer++;
                    break;
                case OpCode.cdv:
                    RegisterC = (int)(RegisterA / Math.Pow(2, GetOperandValue(operand)));
                    _instructionPointer++;
                    break;
                case OpCode.bxl:
                    RegisterB ^= GetOperandValue(operand);
                    _instructionPointer++;
                    break;
                case OpCode.bxc:
                    RegisterB ^= RegisterC;
                    _instructionPointer++;
                    break;
                case OpCode.jnz:
                    if (RegisterA != 0)
                        _instructionPointer = GetOperandValue(operand);
                    else
                        _instructionPointer++;
                    break;
                case OpCode.bst:
                    RegisterB = GetOperandValue(operand) % 8;
                    _instructionPointer++;
                    break;
                case OpCode.@out:
                    yield return GetOperandValue(operand) % 8;
                    _instructionPointer++;
                    break;
            }
        }
    }

    private int GetOperandValue(Operand operand) => operand.Type switch
    {
        OperandType.Literal => operand.Value,
        OperandType.Combo when operand.Value <= 3 => operand.Value,
        OperandType.Combo when operand.Value == 4 => RegisterA,
        OperandType.Combo when operand.Value == 5 => RegisterB,
        OperandType.Combo when operand.Value == 6 => RegisterC,
        _ => throw new InvalidProgramException(),
    };
}
