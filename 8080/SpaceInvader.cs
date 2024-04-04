using System.Diagnostics;

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

        Stopwatch s = new Stopwatch();
        s.Start();
        long lastInterrupt = 0;

        bool done = false;
        while (done != true)
        {
            Console.WriteLine($"#{step} -- PC {AsHex(state.pc)} -- SP {AsHex(state.sp)} -- Flags {(state.cc.z ? "Z" : ".")}{(state.cc.ac ? "AC" : ".")}{(state.cc.pad ? "PAD" : ".")}{(state.cc.cy ? "CY" : ".")}{(state.cc.p ? "P" : ".")}{(state.cc.s ? "S" : ".")}");
            Console.WriteLine($"A {AsHex(state.a)} -- BC {AsHex(state.b)}{AsHex(state.c)} -- DE {AsHex(state.d)}{AsHex(state.e)} -- HL {AsHex(state.h)}{AsHex(state.l)} -- {(state.int_enable ? "INT" : ".")}");
            Console.WriteLine();

            cpuEmu.Emulate8080Op(ref state);

            if (s.ElapsedMilliseconds - lastInterrupt > 1.0 / 60.0)
            {
                if (state.int_enable)
                {
                    GenerateInterrupt(ref state, 2);
                    lastInterrupt = s.ElapsedMilliseconds;

                    // Now draw the screen
                    DrawScreen(state.memory[0x2400..0x3fff]);
                }
            }

            step++;
        }
    }

    private void GenerateInterrupt(ref State8080 state, int interruptNum)
    {
        // Push pc to stack
        state.memory[state.sp - 1] = (byte)(state.pc & 0xff00);
        state.memory[state.sp - 2] = (byte)(state.pc & 0xff);
        state.sp -= 2;

        //set pc to low mem vector (0x10 for interruptNum 2 at end of screen, don't need middle screen interruptnum 1 at 0x08)
        state.pc = (ushort)(8 * interruptNum);
    }

    private void DrawScreen(byte[] vram)
    {

    }

    private string AsHex(int i, int l = 2)
    {
        return i.ToString($"X{l}");
    }
}
