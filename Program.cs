byte[] rom = File.ReadAllBytes("invaders.h");
int pc = 0;
while (pc < rom.Length)
{
    int n = Disassemble8080Op(rom, pc);
    pc += n;
}

 /*    
    *codebuffer is a valid pointer to 8080 assembly code    
    pc is the current offset into the code    

    returns the number of bytes of the op    
   */    
int Disassemble8080Op(byte[] codeBuffer, int pc)
{
    int opBytes = 1;
    
    byte code = codeBuffer[pc];
    Console.Write($"0x{AsHex(pc)}\t0x{AsHex(code)}\t");

    switch(code)
    {
        case 0x00: Console.Write("NOP"); break;
        case 0x01: Console.Write($"LXI   B,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0x02: Console.Write($"STAX  B"); break;
        case 0x03: Console.Write($"INX   B"); break;
        case 0x04: Console.Write($"INR   B"); break;
        case 0x05: Console.Write($"DCR   B"); break;
        case 0x06: Console.Write($"MVI   B,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0x07: Console.Write("RLC"); break;
        
        case 0x09: Console.Write($"DAD   B"); break;
        case 0x0A: Console.Write($"LDAX  B"); break;
        case 0x0B: Console.Write($"DCX   B"); break;
        case 0x0C: Console.Write($"INR   C"); break;
        case 0x0D: Console.Write($"DCR   C"); break;
        case 0x0E: Console.Write($"MVI   C,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0x0F: Console.Write($"RRC"); break;

        case 0x11: Console.Write($"LXI   D,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0x12: Console.Write($"STAX  D"); break;
        case 0x13: Console.Write($"INX   D"); break;
        case 0x14: Console.Write($"INR   D"); break;
        case 0x15: Console.Write($"DCR   D"); break;
        case 0x16: Console.Write($"MVI   D,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0x17: Console.Write("RAL"); break;

        case 0x19: Console.Write($"DAD   D"); break;
        case 0x1A: Console.Write($"LDAX  D"); break;
        case 0x1B: Console.Write($"DCX   D"); break;
        case 0x1C: Console.Write($"INR   E"); break;
        case 0x1D: Console.Write($"DCR   E"); break;
        case 0x1E: Console.Write($"MVI   E,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0x1F: Console.Write($"RAR"); break;
        
        case 0x21: Console.Write($"LXI   H,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0x22: Console.Write($"SHLD adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0x23: Console.Write($"INX   H"); break;
        case 0x24: Console.Write($"INR   H"); break;
        case 0x25: Console.Write($"DCR   H"); break;
        case 0x26: Console.Write($"MVI   H,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0x27: Console.Write("DAA"); break;

        case 0x29: Console.Write($"DAD   H"); break;
        case 0x2A: Console.Write($"LHLD adr{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0x2B: Console.Write($"DCX   H"); break;
        case 0x2C: Console.Write($"INR   L"); break;
        case 0x2D: Console.Write($"DCR   L"); break;
        case 0x2E: Console.Write($"MVI   L,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0x2F: Console.Write($"CMA"); break;

        case 0x31: Console.Write($"LXI  sp,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0x32: Console.Write($"STA adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0x33: Console.Write($"INX  SP"); break;
        case 0x34: Console.Write($"INR   M"); break;
        case 0x35: Console.Write($"DCR   M"); break;
        case 0x36: Console.Write($"MVI   M,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0x37: Console.Write("STC"); break;

        case 0x39: Console.Write($"DAD  SP"); break;
        case 0x3A: Console.Write($"LDA adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0x3B: Console.Write($"DCX  SP"); break;
        case 0x3C: Console.Write($"INR   A"); break;
        case 0x3D: Console.Write($"DCR   A"); break;
        case 0x3E: Console.Write($"MVI   A,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0x3F: Console.Write($"CMC"); break;
        case 0x40: Console.Write($"MOV B,B"); break;
        case 0x41: Console.Write($"MOV B,C"); break;
        case 0x42: Console.Write($"MOV B,D"); break;
        case 0x43: Console.Write($"MOV B,E"); break;
        case 0x44: Console.Write($"MOV B,H"); break;
        case 0x45: Console.Write($"MOV B,L"); break;
        case 0x46: Console.Write($"MOV B,M"); break;
        case 0x47: Console.Write($"MOV B,A"); break;
        case 0x48: Console.Write($"MOV C,B"); break;
        case 0x49: Console.Write($"MOV C,C"); break;
        case 0x4A: Console.Write($"MOV C,D"); break;
        case 0x4B: Console.Write($"MOV C,E"); break;
        case 0x4C: Console.Write($"MOV C,H"); break;
        case 0x4D: Console.Write($"MOV C,L"); break;
        case 0x4E: Console.Write($"MOV C,M"); break;
        case 0x4F: Console.Write($"MOV C,A"); break;
        case 0x50: Console.Write($"MOV D,B"); break;
        case 0x51: Console.Write($"MOV D,C"); break;
        case 0x52: Console.Write($"MOV D,D"); break;
        case 0x53: Console.Write($"MOV D,E"); break;
        case 0x54: Console.Write($"MOV D,H"); break;
        case 0x55: Console.Write($"MOV D,L"); break;
        case 0x56: Console.Write($"MOV D,M"); break;
        case 0x57: Console.Write($"MOV D,A"); break;
        case 0x58: Console.Write($"MOV E,B"); break;
        case 0x59: Console.Write($"MOV E,C"); break;
        case 0x5A: Console.Write($"MOV E,D"); break;
        case 0x5B: Console.Write($"MOV E,E"); break;
        case 0x5C: Console.Write($"MOV E,H"); break;
        case 0x5D: Console.Write($"MOV E,L"); break;
        case 0x5E: Console.Write($"MOV E,M"); break;
        case 0x5F: Console.Write($"MOV E,A"); break;
        case 0x60: Console.Write($"MOV H,B"); break;
        case 0x61: Console.Write($"MOV H,C"); break;
        case 0x62: Console.Write($"MOV H,D"); break;
        case 0x63: Console.Write($"MOV H,E"); break;
        case 0x64: Console.Write($"MOV H,H"); break;
        case 0x65: Console.Write($"MOV H,L"); break;
        case 0x66: Console.Write($"MOV H,M"); break;
        case 0x67: Console.Write($"MOV H,A"); break;
        case 0x68: Console.Write($"MOV L,B"); break;
        case 0x69: Console.Write($"MOV L,C"); break;
        case 0x6A: Console.Write($"MOV L,D"); break;
        case 0x6B: Console.Write($"MOV L,E"); break;
        case 0x6C: Console.Write($"MOV L,H"); break;
        case 0x6D: Console.Write($"MOV L,L"); break;
        case 0x6E: Console.Write($"MOV L,M"); break;
        case 0x6F: Console.Write($"MOV L,A"); break;
        case 0x70: Console.Write($"MOV M,B"); break;
        case 0x71: Console.Write($"MOV M,C"); break;
        case 0x72: Console.Write($"MOV M,D"); break;
        case 0x73: Console.Write($"MOV M,E"); break;
        case 0x74: Console.Write($"MOV M,H"); break;
        case 0x75: Console.Write($"MOV M,L"); break;
        case 0x76: Console.Write($"HLT"); break;
        case 0x77: Console.Write($"MOV M,A"); break;
        case 0x78: Console.Write($"MOV A,B"); break;
        case 0x79: Console.Write($"MOV A,C"); break;
        case 0x7A: Console.Write($"MOV A,D"); break;
        case 0x7B: Console.Write($"MOV A,E"); break;
        case 0x7C: Console.Write($"MOV A,H"); break;
        case 0x7D: Console.Write($"MOV A,L"); break;
        case 0x7E: Console.Write($"MOV A,M"); break;
        case 0x7F: Console.Write($"MOV A,A"); break;
        case 0x80: Console.Write($"ADD   B"); break;
        case 0x81: Console.Write($"ADD   C"); break;
        case 0x82: Console.Write($"ADD   D"); break;
        case 0x83: Console.Write($"ADD   E"); break;
        case 0x84: Console.Write($"ADD   H"); break;
        case 0x85: Console.Write($"ADD   L"); break;
        case 0x86: Console.Write($"ADD   M"); break;
        case 0x87: Console.Write($"ADD   A"); break;
        case 0x88: Console.Write($"ADC   B"); break;
        case 0x89: Console.Write($"ADC   C"); break;
        case 0x8A: Console.Write($"ADC   D"); break;
        case 0x8B: Console.Write($"ADC   E"); break;
        case 0x8C: Console.Write($"ADC   H"); break;
        case 0x8D: Console.Write($"ADC   L"); break;
        case 0x8E: Console.Write($"ADC   M"); break;
        case 0x8F: Console.Write($"ADC   A"); break;
        case 0x90: Console.Write($"SUB   B"); break;
        case 0x91: Console.Write($"SUB   C"); break;
        case 0x92: Console.Write($"SUB   D"); break;
        case 0x93: Console.Write($"SUB   E"); break;
        case 0x94: Console.Write($"SUB   H"); break;
        case 0x95: Console.Write($"SUB   L"); break;
        case 0x96: Console.Write($"SUB   M"); break;
        case 0x97: Console.Write($"SUB   A"); break;
        case 0x98: Console.Write($"SBB   B"); break;
        case 0x99: Console.Write($"SBB   C"); break;
        case 0x9A: Console.Write($"SBB   D"); break;
        case 0x9B: Console.Write($"SBB   E"); break;
        case 0x9C: Console.Write($"SBB   H"); break;
        case 0x9D: Console.Write($"SBB   L"); break;
        case 0x9E: Console.Write($"SBB   M"); break;
        case 0x9F: Console.Write($"SBB   A"); break;
        case 0xA0: Console.Write($"ANA   B"); break;
        case 0xA1: Console.Write($"ANA   C"); break;
        case 0xA2: Console.Write($"ANA   D"); break;
        case 0xA3: Console.Write($"ANA   E"); break;
        case 0xA4: Console.Write($"ANA   H"); break;
        case 0xA5: Console.Write($"ANA   L"); break;
        case 0xA6: Console.Write($"ANA   M"); break;
        case 0xA7: Console.Write($"ANA   A"); break;
        case 0xA8: Console.Write($"XRA   B"); break;
        case 0xA9: Console.Write($"XRA   C"); break;
        case 0xAA: Console.Write($"XRA   D"); break;
        case 0xAB: Console.Write($"XRA   E"); break;
        case 0xAC: Console.Write($"XRA   H"); break;
        case 0xAD: Console.Write($"XRA   L"); break;
        case 0xAE: Console.Write($"XRA   M"); break;
        case 0xAF: Console.Write($"XRA   A"); break;
        case 0xB0: Console.Write($"ORA   B"); break;
        case 0xB1: Console.Write($"ORA   C"); break;
        case 0xB2: Console.Write($"ORA   D"); break;
        case 0xB3: Console.Write($"ORA   E"); break;
        case 0xB4: Console.Write($"ORA   H"); break;
        case 0xB5: Console.Write($"ORA   L"); break;
        case 0xB6: Console.Write($"ORA   M"); break;
        case 0xB7: Console.Write($"ORA   A"); break;
        case 0xB8: Console.Write($"CMP   B"); break;
        case 0xB9: Console.Write($"CMP   C"); break;
        case 0xBA: Console.Write($"CMP   D"); break;
        case 0xBB: Console.Write($"CMP   E"); break;
        case 0xBC: Console.Write($"CMP   H"); break;
        case 0xBD: Console.Write($"CMP   L"); break;
        case 0xBE: Console.Write($"CMP   M"); break;
        case 0xBF: Console.Write($"CMP   A"); break;
        case 0xC0: Console.Write($"RNZ"); break;
        case 0xC1: Console.Write($"POP   B"); break;
        case 0xC2: Console.Write($"JNZ adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xC3: Console.Write($"JMP adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xC4: Console.Write($"CNZ adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xC5: Console.Write($"PUSH  B"); break;
        case 0xC6: Console.Write($"ADI,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0xC7: Console.Write($"RST   0"); break;
        case 0xC8: Console.Write($"RZ"); break;
        case 0xC9: Console.Write($"RET"); break;
        case 0xCA: Console.Write($"JZ adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        
        case 0xCC: Console.Write($"CZ adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xCD: Console.Write($"CALL adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xCE: Console.Write($"ACI,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0xCF: Console.Write($"RST   1"); break;
        case 0xD0: Console.Write($"RNC"); break;
        case 0xD1: Console.Write($"POP   D"); break;
        case 0xD2: Console.Write($"JNC adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xD3: Console.Write($"OUT,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0xD4: Console.Write($"CNC adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xD5: Console.Write($"PUSH  D"); break;
        case 0xD6: Console.Write($"SUI,{AsHex(codeBuffer[pc+1])}"); opBytes= 2; break;
        case 0xD7: Console.Write($"RST   2"); break;
        case 0xD8: Console.Write($"RC"); break;

        case 0xDA: Console.Write($"JC adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xDB: Console.Write($"IN,{AsHex(codeBuffer[pc+1])}"); opBytes = 2; break;
        case 0xDC: Console.Write($"CC adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;

        case 0xDE: Console.Write($"SBI,{AsHex(codeBuffer[pc+1])}"); opBytes= 2; break;
        case 0xDF: Console.Write($"RST   3"); break;
        case 0xE0: Console.Write($"RPO"); break;
        case 0xE1: Console.Write($"POP   H"); break;
        case 0xE2: Console.Write($"JPO adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xE3: Console.Write($"XTHL"); break;
        case 0xE4: Console.Write($"CPO adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xE5: Console.Write($"PUSH  H"); break;
        case 0xE6: Console.Write($"ANI,{AsHex(codeBuffer[pc+1])}"); opBytes= 2; break;
        case 0xE7: Console.Write($"RST   4"); break;
        case 0xE8: Console.Write($"RPE"); break;
        case 0xE9: Console.Write($"PCHL"); break;
        case 0xEA: Console.Write($"JPE adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xEB: Console.Write($"XCHG"); break;
        case 0xEC: Console.Write($"CPE adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;

        case 0xEE: Console.Write($"XRI,{AsHex(codeBuffer[pc+1])}"); opBytes= 2; break;
        case 0xEF: Console.Write($"RST   5"); break;
        case 0xF0: Console.Write($"RP"); break;
        case 0xF1: Console.Write($"POP PSW"); break;
        case 0xF2: Console.Write($"JP adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xF3: Console.Write($"DI"); break;
        case 0xF4: Console.Write($"CP adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xF5: Console.Write($"PUSH PSW"); break;
        case 0xF6: Console.Write($"ORI,{AsHex(codeBuffer[pc+1])}"); opBytes= 2; break;
        case 0xF7: Console.Write($"RST   6"); break;
        case 0xF8: Console.Write($"RM"); break;
        case 0xF9: Console.Write($"SPHL"); break;
        case 0xFA: Console.Write($"JM adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;
        case 0xFB: Console.Write($"EI"); break;
        case 0xFC: Console.Write($"CM adr,{AsHex(codeBuffer[pc+2])} {AsHex(codeBuffer[pc+1])}"); opBytes = 3; break;

        case 0xFE: Console.Write($"CPI,{AsHex(codeBuffer[pc+1])}"); opBytes= 2; break;
        case 0xFF: Console.Write($"RST   7"); break;
    }

    Console.Write("\n");
    return opBytes;
}

string AsHex(int i)
{
    return Convert.ToString(i, 16).ToUpper();
}