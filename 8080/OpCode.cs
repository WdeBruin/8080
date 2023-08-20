using System.Runtime.Intrinsics.X86;

namespace cpu;

public class OpCode
{
    void UnimplementedInstruction(State8080 state)
    {
        Console.WriteLine($"Error: Unimplemented instruction");
    }

    public void Emulate8080Op(ref State8080 state)
    {
        byte opCode = state.memory[state.pc];
        ushort opBytes = 1;

        switch (opCode)
        {
            case 0x00: break; //NOP
            case 0x01: // LXI B
                opBytes = 3;
                state.b = state.memory[state.pc + 2];
                state.c = state.memory[state.pc + 1];
                break;
            case 0x02: // STAX B 
                {
                    ushort addr = (ushort)(state.b << 8 | state.c);
                    state.memory[addr] = state.a;
                }
                break;
            case 0x03: // INX B
                {
                    ushort res = (ushort)((state.b << 8 | state.c) + 1);
                    state.b = (byte)(res >> 8);
                    state.c = (byte)res;
                }
                break;
            case 0x04: // INR B
                {
                    byte res = (byte)(state.b + 1);
                    state.b = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x05: // DCR B
                {
                    byte res = (byte)(state.b - 1);
                    state.b = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x06: // MVI B
                opBytes = 2;
                state.b = state.memory[state.pc + 1];
                break;
            case 0x07: // RLC
                {
                    bool hbit = (byte)((state.a >> 7) & 0xff) == 1;
                    state.a = (byte)((state.a << 1) | (hbit ? 1 : 0));

                    state.cc.cy = hbit;
                }
                break;
            case 0x09: // DAD B
                {
                    ushort toAdd = (ushort)(state.b << 8 | state.c);
                    ushort hl = (ushort)(state.h << 8 | state.l);
                    int res = toAdd + hl;
                    state.cc.cy = (res & 0x1_0000) > 0; // set carry if bit 17 is set (overflow)
                    state.h = (byte)(res >> 8);
                    state.l = (byte)res;
                }
                break;
            case 0x0A: // LDAX B
                {
                    ushort addr = (ushort)(state.b << 8 | state.c);
                    state.a = state.memory[addr];
                }
                break;
            case 0x0B: // DCX B
                {
                    ushort res = (ushort)((state.b << 8 | state.c) - 1);
                    state.b = (byte)(res >> 8);
                    state.c = (byte)res;
                }
                break;
            case 0x0C: // INR C
                {
                    byte res = (byte)(state.c + 1);
                    state.c = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x0D: // DCR C
                {
                    byte res = (byte)(state.c - 1);
                    state.c = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x0E: // MVI C
                opBytes = 2;
                state.c = state.memory[state.pc + 1];
                break;
            case 0x0F: UnimplementedInstruction(state); break;
            case 0x11: UnimplementedInstruction(state); break;
            case 0x12: UnimplementedInstruction(state); break;
            case 0x13: UnimplementedInstruction(state); break;
            case 0x14: // INR D
                {
                    byte res = (byte)(state.d + 1);
                    state.d = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x15: // DCR D
                {
                    byte res = (byte)(state.d - 1);
                    state.d = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x16: // MVI D
                opBytes = 2;
                state.d = state.memory[state.pc + 1];
                break;
            case 0x17: UnimplementedInstruction(state); break;
            case 0x19: UnimplementedInstruction(state); break;
            case 0x1A: UnimplementedInstruction(state); break;
            case 0x1B: UnimplementedInstruction(state); break;
            case 0x1C: // INR E
                {
                    byte res = (byte)(state.e + 1);
                    state.e = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x1D: // DCR E
                {
                    byte res = (byte)(state.e - 1);
                    state.e = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x1E: // MVI E
                opBytes = 2;
                state.e = state.memory[state.pc + 1];
                break;
            case 0x1F: UnimplementedInstruction(state); break;
            case 0x21: UnimplementedInstruction(state); break;
            case 0x22: UnimplementedInstruction(state); break;
            case 0x23: UnimplementedInstruction(state); break;
            case 0x24: // INR H
                {
                    byte res = (byte)(state.h + 1);
                    state.h = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x25: // DCR H
                {
                    byte res = (byte)(state.h - 1);
                    state.h = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x26: // MVI H
                opBytes = 2;
                state.h = state.memory[state.pc + 1];
                break;
            case 0x27: UnimplementedInstruction(state); break;
            case 0x29: UnimplementedInstruction(state); break;
            case 0x2A: UnimplementedInstruction(state); break;
            case 0x2B: UnimplementedInstruction(state); break;
            case 0x2C: // INR L
                {
                    byte res = (byte)(state.l + 1);
                    state.l = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x2D: // DCR L
                {
                    byte res = (byte)(state.l - 1);
                    state.l = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x2E: // MVI L
                opBytes = 2;
                state.l = state.memory[state.pc + 1];
                break;
            case 0x2F: UnimplementedInstruction(state); break;
            case 0x31: UnimplementedInstruction(state); break;
            case 0x32: UnimplementedInstruction(state); break;
            case 0x33: UnimplementedInstruction(state); break;
            case 0x34: UnimplementedInstruction(state); break;
            case 0x35: UnimplementedInstruction(state); break;
            case 0x36: UnimplementedInstruction(state); break;
            case 0x37: UnimplementedInstruction(state); break;
            case 0x39: UnimplementedInstruction(state); break;
            case 0x3A: UnimplementedInstruction(state); break;
            case 0x3B: UnimplementedInstruction(state); break;
            case 0x3C: // INR A
                {
                    byte res = (byte)(state.a + 1);
                    state.a = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x3D: // DCR A
                {
                    byte res = (byte)(state.a - 1);
                    state.a = res;

                    state.cc.z = res == 0;
                    state.cc.s = (res & 0x80) != 0;
                    state.cc.p = (res % 2) == 0;
                }
                break;
            case 0x3E: // MVI A
                opBytes = 2;
                state.a = state.memory[state.pc + 1];
                break;
            case 0x3F: UnimplementedInstruction(state); break;
            case 0x40: UnimplementedInstruction(state); break;
            case 0x41: UnimplementedInstruction(state); break;
            case 0x42: UnimplementedInstruction(state); break;
            case 0x43: UnimplementedInstruction(state); break;
            case 0x44: UnimplementedInstruction(state); break;
            case 0x45: UnimplementedInstruction(state); break;
            case 0x46: UnimplementedInstruction(state); break;
            case 0x47: UnimplementedInstruction(state); break;
            case 0x48: UnimplementedInstruction(state); break;
            case 0x49: UnimplementedInstruction(state); break;
            case 0x4A: UnimplementedInstruction(state); break;
            case 0x4B: UnimplementedInstruction(state); break;
            case 0x4C: UnimplementedInstruction(state); break;
            case 0x4D: UnimplementedInstruction(state); break;
            case 0x4E: UnimplementedInstruction(state); break;
            case 0x4F: UnimplementedInstruction(state); break;
            case 0x50: UnimplementedInstruction(state); break;
            case 0x51: UnimplementedInstruction(state); break;
            case 0x52: UnimplementedInstruction(state); break;
            case 0x53: UnimplementedInstruction(state); break;
            case 0x54: UnimplementedInstruction(state); break;
            case 0x55: UnimplementedInstruction(state); break;
            case 0x56: UnimplementedInstruction(state); break;
            case 0x57: UnimplementedInstruction(state); break;
            case 0x58: UnimplementedInstruction(state); break;
            case 0x59: UnimplementedInstruction(state); break;
            case 0x5A: UnimplementedInstruction(state); break;
            case 0x5B: UnimplementedInstruction(state); break;
            case 0x5C: UnimplementedInstruction(state); break;
            case 0x5D: UnimplementedInstruction(state); break;
            case 0x5E: UnimplementedInstruction(state); break;
            case 0x5F: UnimplementedInstruction(state); break;
            case 0x60: UnimplementedInstruction(state); break;
            case 0x61: UnimplementedInstruction(state); break;
            case 0x62: UnimplementedInstruction(state); break;
            case 0x63: UnimplementedInstruction(state); break;
            case 0x64: UnimplementedInstruction(state); break;
            case 0x65: UnimplementedInstruction(state); break;
            case 0x66: UnimplementedInstruction(state); break;
            case 0x67: UnimplementedInstruction(state); break;
            case 0x68: UnimplementedInstruction(state); break;
            case 0x69: UnimplementedInstruction(state); break;
            case 0x6A: UnimplementedInstruction(state); break;
            case 0x6B: UnimplementedInstruction(state); break;
            case 0x6C: UnimplementedInstruction(state); break;
            case 0x6D: UnimplementedInstruction(state); break;
            case 0x6E: UnimplementedInstruction(state); break;
            case 0x6F: UnimplementedInstruction(state); break;
            case 0x70: UnimplementedInstruction(state); break;
            case 0x71: UnimplementedInstruction(state); break;
            case 0x72: UnimplementedInstruction(state); break;
            case 0x73: UnimplementedInstruction(state); break;
            case 0x74: UnimplementedInstruction(state); break;
            case 0x75: UnimplementedInstruction(state); break;
            case 0x76: UnimplementedInstruction(state); break;
            case 0x77: UnimplementedInstruction(state); break;
            case 0x78: UnimplementedInstruction(state); break;
            case 0x79: UnimplementedInstruction(state); break;
            case 0x7A: UnimplementedInstruction(state); break;
            case 0x7B: UnimplementedInstruction(state); break;
            case 0x7C: UnimplementedInstruction(state); break;
            case 0x7D: UnimplementedInstruction(state); break;
            case 0x7E: UnimplementedInstruction(state); break;
            case 0x7F: UnimplementedInstruction(state); break;
            case 0x80: UnimplementedInstruction(state); break;
            case 0x81: UnimplementedInstruction(state); break;
            case 0x82: UnimplementedInstruction(state); break;
            case 0x83: UnimplementedInstruction(state); break;
            case 0x84: UnimplementedInstruction(state); break;
            case 0x85: UnimplementedInstruction(state); break;
            case 0x86: UnimplementedInstruction(state); break;
            case 0x87: UnimplementedInstruction(state); break;
            case 0x88: UnimplementedInstruction(state); break;
            case 0x89: UnimplementedInstruction(state); break;
            case 0x8A: UnimplementedInstruction(state); break;
            case 0x8B: UnimplementedInstruction(state); break;
            case 0x8C: UnimplementedInstruction(state); break;
            case 0x8D: UnimplementedInstruction(state); break;
            case 0x8E: UnimplementedInstruction(state); break;
            case 0x8F: UnimplementedInstruction(state); break;
            case 0x90: UnimplementedInstruction(state); break;
            case 0x91: UnimplementedInstruction(state); break;
            case 0x92: UnimplementedInstruction(state); break;
            case 0x93: UnimplementedInstruction(state); break;
            case 0x94: UnimplementedInstruction(state); break;
            case 0x95: UnimplementedInstruction(state); break;
            case 0x96: UnimplementedInstruction(state); break;
            case 0x97: UnimplementedInstruction(state); break;
            case 0x98: UnimplementedInstruction(state); break;
            case 0x99: UnimplementedInstruction(state); break;
            case 0x9A: UnimplementedInstruction(state); break;
            case 0x9B: UnimplementedInstruction(state); break;
            case 0x9C: UnimplementedInstruction(state); break;
            case 0x9D: UnimplementedInstruction(state); break;
            case 0x9E: UnimplementedInstruction(state); break;
            case 0x9F: UnimplementedInstruction(state); break;
            case 0xA0: UnimplementedInstruction(state); break;
            case 0xA1: UnimplementedInstruction(state); break;
            case 0xA2: UnimplementedInstruction(state); break;
            case 0xA3: UnimplementedInstruction(state); break;
            case 0xA4: UnimplementedInstruction(state); break;
            case 0xA5: UnimplementedInstruction(state); break;
            case 0xA6: UnimplementedInstruction(state); break;
            case 0xA7: UnimplementedInstruction(state); break;
            case 0xA8: UnimplementedInstruction(state); break;
            case 0xA9: UnimplementedInstruction(state); break;
            case 0xAA: UnimplementedInstruction(state); break;
            case 0xAB: UnimplementedInstruction(state); break;
            case 0xAC: UnimplementedInstruction(state); break;
            case 0xAD: UnimplementedInstruction(state); break;
            case 0xAE: UnimplementedInstruction(state); break;
            case 0xAF: UnimplementedInstruction(state); break;
            case 0xB0: UnimplementedInstruction(state); break;
            case 0xB1: UnimplementedInstruction(state); break;
            case 0xB2: UnimplementedInstruction(state); break;
            case 0xB3: UnimplementedInstruction(state); break;
            case 0xB4: UnimplementedInstruction(state); break;
            case 0xB5: UnimplementedInstruction(state); break;
            case 0xB6: UnimplementedInstruction(state); break;
            case 0xB7: UnimplementedInstruction(state); break;
            case 0xB8: UnimplementedInstruction(state); break;
            case 0xB9: UnimplementedInstruction(state); break;
            case 0xBA: UnimplementedInstruction(state); break;
            case 0xBB: UnimplementedInstruction(state); break;
            case 0xBC: UnimplementedInstruction(state); break;
            case 0xBD: UnimplementedInstruction(state); break;
            case 0xBE: UnimplementedInstruction(state); break;
            case 0xBF: UnimplementedInstruction(state); break;
            case 0xC0: UnimplementedInstruction(state); break;
            case 0xC1: UnimplementedInstruction(state); break;
            case 0xC2: UnimplementedInstruction(state); break;
            case 0xC3: UnimplementedInstruction(state); break;
            case 0xC4: UnimplementedInstruction(state); break;
            case 0xC5: UnimplementedInstruction(state); break;
            case 0xC6: UnimplementedInstruction(state); break;
            case 0xC7: UnimplementedInstruction(state); break;
            case 0xC8: UnimplementedInstruction(state); break;
            case 0xC9: UnimplementedInstruction(state); break;
            case 0xCA: UnimplementedInstruction(state); break;
            case 0xCC: UnimplementedInstruction(state); break;
            case 0xCD: UnimplementedInstruction(state); break;
            case 0xCE: UnimplementedInstruction(state); break;
            case 0xCF: UnimplementedInstruction(state); break;
            case 0xD0: UnimplementedInstruction(state); break;
            case 0xD1: UnimplementedInstruction(state); break;
            case 0xD2: UnimplementedInstruction(state); break;
            case 0xD3: UnimplementedInstruction(state); break;
            case 0xD4: UnimplementedInstruction(state); break;
            case 0xD5: UnimplementedInstruction(state); break;
            case 0xD6: UnimplementedInstruction(state); break;
            case 0xD7: UnimplementedInstruction(state); break;
            case 0xD8: UnimplementedInstruction(state); break;
            case 0xDA: UnimplementedInstruction(state); break;
            case 0xDB: UnimplementedInstruction(state); break;
            case 0xDC: UnimplementedInstruction(state); break;
            case 0xDE: UnimplementedInstruction(state); break;
            case 0xDF: UnimplementedInstruction(state); break;
            case 0xE0: UnimplementedInstruction(state); break;
            case 0xE1: UnimplementedInstruction(state); break;
            case 0xE2: UnimplementedInstruction(state); break;
            case 0xE3: UnimplementedInstruction(state); break;
            case 0xE4: UnimplementedInstruction(state); break;
            case 0xE5: UnimplementedInstruction(state); break;
            case 0xE6: UnimplementedInstruction(state); break;
            case 0xE7: UnimplementedInstruction(state); break;
            case 0xE8: UnimplementedInstruction(state); break;
            case 0xE9: UnimplementedInstruction(state); break;
            case 0xEA: UnimplementedInstruction(state); break;
            case 0xEB: UnimplementedInstruction(state); break;
            case 0xEC: UnimplementedInstruction(state); break;
            case 0xEE: UnimplementedInstruction(state); break;
            case 0xEF: UnimplementedInstruction(state); break;
            case 0xF0: UnimplementedInstruction(state); break;
            case 0xF1: UnimplementedInstruction(state); break;
            case 0xF2: UnimplementedInstruction(state); break;
            case 0xF3: UnimplementedInstruction(state); break;
            case 0xF4: UnimplementedInstruction(state); break;
            case 0xF5: UnimplementedInstruction(state); break;
            case 0xF6: UnimplementedInstruction(state); break;
            case 0xF7: UnimplementedInstruction(state); break;
            case 0xF8: UnimplementedInstruction(state); break;
            case 0xF9: UnimplementedInstruction(state); break;
            case 0xFA: UnimplementedInstruction(state); break;
            case 0xFB: UnimplementedInstruction(state); break;
            case 0xFC: UnimplementedInstruction(state); break;
            case 0xFE: UnimplementedInstruction(state); break;
            case 0xFF: UnimplementedInstruction(state); break;
        }
        state.pc += opBytes;
    }
}