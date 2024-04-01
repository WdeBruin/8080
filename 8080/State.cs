namespace cpu;

public struct ConditionCodes
{
    public bool z;
    public bool s;
    public bool p;
    public bool cy;
    public bool ac;
    public bool pad;
}

public struct State8080 
{
    public byte a;
    public byte b;
    public byte c;
    public byte d;
    public byte e;
    public byte h;
    public byte l;
    public ushort sp;
    public ushort pc;
    public byte[] memory;
    public ConditionCodes cc;
    public byte int_enable;
}
