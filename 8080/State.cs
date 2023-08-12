namespace cpu;

public struct ConditionCodes
{
    public byte z;
    public byte s;
    public byte p;
    public byte cy;
    public byte ac;
    public byte pad;
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
    public UInt16 sp;
    public UInt16 pc;
    public byte[] memory;
    public ConditionCodes cc;
    public byte int_enable; 
}
