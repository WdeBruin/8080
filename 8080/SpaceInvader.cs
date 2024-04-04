using System.Collections;
using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;

namespace cpu;

public class SpaceInvader
{
    RenderWindow window;

    public SpaceInvader()
    {
        window = new RenderWindow(new VideoMode(256, 224), "Space Invaders");
    }

    public async Task Run()
    {
        // todo add input
        // window.KeyPressed += 

        int step = 0;
        Stopwatch s = new Stopwatch();
        s.Start();
        long lastInterrupt = 0;

        var state = new State8080
        {
            memory = new byte[0x4000] //16k (0000 - 1FFF ROM; 2000 - 23FF RAM; 2400 - 3FFF VRAM)
        };

        var rom = File.ReadAllBytes("invaders");
        rom.CopyTo(state.memory, 0); // load the rom

        var cpuEmu = new OpCode();

        while (window.IsOpen)
        {
            // process events
            window.DispatchEvents();

            if (s.ElapsedMilliseconds - lastInterrupt > 1.0 / 60.0)
            {
                if (state.int_enable)
                {
                    GenerateInterrupt(ref state, 2);
                    lastInterrupt = s.ElapsedMilliseconds;

                    // Now draw the screen
                    await DrawScreen(state.memory[0x2400..0x3fff]);

                    // Disable interrupt
                    DisableInterrupt(ref state, 2);
                }
            }

            Console.WriteLine($"#{step} -- PC {AsHex(state.pc)} -- SP {AsHex(state.sp)} -- Flags {(state.cc.z ? "Z" : ".")}{(state.cc.ac ? "AC" : ".")}{(state.cc.pad ? "PAD" : ".")}{(state.cc.cy ? "CY" : ".")}{(state.cc.p ? "P" : ".")}{(state.cc.s ? "S" : ".")}");
            Console.WriteLine($"A {AsHex(state.a)} -- BC {AsHex(state.b)}{AsHex(state.c)} -- DE {AsHex(state.d)}{AsHex(state.e)} -- HL {AsHex(state.h)}{AsHex(state.l)} -- {(state.int_enable ? "INT" : ".")}");
            Console.WriteLine();

            cpuEmu.Emulate8080Op(ref state);
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

    private void DisableInterrupt(ref State8080 state, int interruptNum)
    {
        // Pop pc from stack -> goes wrong, missing value. Got 243 but not 2560 at sp+1
        state.pc = (ushort)(state.memory[state.sp] + state.memory[state.sp+1]);
        state.sp += 2;
    }

    private async Task DrawScreen(byte[] vram)
    {
        window.Clear(Color.Black);

        BitArray pixels = new BitArray(vram);

        //byte 0-31 is line 1. each byte has 8 bits for pixels
        int x = 0;
        int y = 0;
        for (int i = 0; i < pixels.Length; i++)
        {
            if (i == 32)
            {
                y += 1;
                x = 0;
            }

            var pixShape = new RectangleShape(new SFML.System.Vector2f(1, 1))
            {
                Position = new SFML.System.Vector2f(x, y),
                FillColor = pixels[i] ? Color.White : Color.Black
            };

            window.Draw(pixShape);
            x++;
        }

        window.Display();
    }

    private string AsHex(int i, int l = 2)
    {
        return i.ToString($"X{l}");
    }
}
