namespace cpu;

public class SpaceInvader
{
    public void Run()
    {
        var state = new State8080
        {
            memory = new byte[0x4000] //16k (0000 - 1FFF ROM; 2000 - 23FF RAM; 2400 - 3FFF VRAM)
        };

        var rom = File.ReadAllBytes("invaders");
        rom.CopyTo(state.memory, 0); // load the rom

        var cpuEmu = new OpCode();

        int step = 0;
        while (true && step <= 50000)
        {
            Console.WriteLine($"#{step} -- PC {AsHex(state.pc)} -- SP {AsHex(state.sp)} -- Flags {(state.cc.z ? "Z":".")}{(state.cc.ac ? "AC":".")}{(state.cc.pad ? "PAD":".")}{(state.cc.cy ? "CY":".")}{(state.cc.p ? "P":".")}{(state.cc.s ? "S":".")}");
            Console.WriteLine($"A {AsHex(state.a)} -- BC {AsHex(state.b)}{AsHex(state.c)} -- DE {AsHex(state.d)}{AsHex(state.e)} -- HL {AsHex(state.h)}{AsHex(state.l)}");

            cpuEmu.Emulate8080Op(ref state);
            Console.WriteLine();
            step++;
        }
    }

    private string AsHex(int i, int l = 2)
    {
        return i.ToString($"X{l}");
    }
}
