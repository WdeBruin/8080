namespace cpu;

public class SpaceInvader
{
    public void Run()
    {
        var state = new State8080
        {
            memory = File.ReadAllBytes("invaders")
        };

        var cpuEmu = new OpCode();

        while (true)
        {
            cpuEmu.Emulate8080Op(ref state);
        }
    }
}
