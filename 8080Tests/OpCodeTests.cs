using System.Collections;
using cpu;

namespace Tests;

public class OpCodeTests
{
    [Fact]
    public void Nop_should_change_nothing()
    {
        // Arrange
        var mem = new byte[] { 0x00 };
        State8080 state = new();
        OpCode sut = new();
        state.pc = 0x00;
        state.memory = mem;

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(new State8080 { memory = mem, pc = 0x01 }, state);
    }

    [Theory] // b, d, h, sp
    [InlineData("b"), InlineData("d"), InlineData("h"), InlineData("sp")]
    public void Lxi_should_load_correctly(string reg)
    {
        // Arrange
        State8080 state = new State8080
        {
            memory = new byte[] { 0x01, 0x08, 0xF8 },
            pc = 0x00
        };

        switch (reg)
        {
            case "b":
                {
                    state.memory[0] = 0x01;
                    break;
                }
            case "d":
                {
                    state.memory[0] = 0x11;
                    break;
                }
            case "h":
                {
                    state.memory[0] = 0x21;
                    break;
                }
            case "sp":
                {
                    state.memory[0] = 0x31;
                    break;
                }
        }

        OpCode sut = new();

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        switch (reg)
        {
            case "b":
                Assert.Equal(0xF8, state.b);
                Assert.Equal(0x08, state.c);
                break;
            case "d":
                Assert.Equal(0xF8, state.d);
                Assert.Equal(0x08, state.e);
                break;
            case "h":
                Assert.Equal(0xF8, state.h);
                Assert.Equal(0x08, state.l);
                break;
            case "sp":
                Assert.Equal(0xF8, state.sp >> 8);
                Assert.Equal(0x08, (byte)state.sp);
                break;
        }
        Assert.Equal(0x03, state.pc);
    }

    [Theory]
    [InlineData("b"), InlineData("d")]
    public void Stax_should_move_accumulator_to_pair_indirect(string reg)
    {
        // Arrange
        byte[] mem = new byte[0xffff];

        if (reg == "b")
            mem[0] = 0x02;
        if (reg == "d")
            mem[0] = 0x12;
            
        OpCode sut = new();

        State8080 state = new State8080
        {
            memory = mem,
            pc = 0x00,
            a = 0x08,
            b = 0xff,
            c = 0x01,
            d = 0xee,
            e = 0x02
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);

        if (reg == "b")
            Assert.Equal(0x08, state.memory[0xff01]);

        if (reg == "d")
            Assert.Equal(0x08, state.memory[0xee02]);
    }

    [Theory]
    [InlineData("b"), InlineData("d"), InlineData("h"), InlineData("sp")]
    public void Inx_should_increment_pair(string reg)
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[1],
            pc = 0x00,
            b = 0x07,
            c = 0x07,
            d = 0x08,
            e = 0x08,
            h = 0x09,
            l = 0x09,
            sp = 0x0a0a
        };

        switch (reg)
        {
            case "b":
                state.memory[0] = 0x03;
                break;
                case "d":
                state.memory[0] = 0x13;
                break;
                case "h":
                state.memory[0] = 0x23;
                break;
                case "sp":
                state.memory[0] = 0x33;
                break;
        }

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);

        if (reg == "b")
            Assert.Equal(0x0708, state.b << 8 | state.c);

        if (reg == "d")
            Assert.Equal(0x0809, state.d << 8 | state.e);

        if (reg == "h")
            Assert.Equal(0x090a, state.h << 8 | state.l);

        if (reg == "sp")
            Assert.Equal(0x0a0b, state.sp);
    }

    [Theory] // "b", "c", "d", "e", "h", "l", "a"
    [InlineData("b"), InlineData("c"), InlineData("d"), InlineData("e"), InlineData("h"), InlineData("l"), InlineData("a")]
    public void Inr_should_increment_flags(string reg)
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[1] { 0x04 },
            pc = 0x00,
            cc = new ConditionCodes { s = true, z = true, cy = true }
        };

        switch (reg)
        {
            case "b":
                {
                    state.memory[0] = 0x04;
                    state.b = 0x07;
                }
                break;
            case "c":
                {
                    state.memory[0] = 0x0c;
                    state.c = 0x07;
                }
                break;
            case "d":
                {
                    state.memory[0] = 0x14;
                    state.d = 0x07;
                }
                break;
            case "e":
                {
                    state.memory[0] = 0x1c;
                    state.e = 0x07;
                }
                break;
            case "h":
                {
                    state.memory[0] = 0x24;
                    state.h = 0x07;
                }
                break;
            case "l":
                {
                    state.memory[0] = 0x2c;
                    state.l = 0x07;
                }
                break;
            case "a":
                {
                    state.memory[0] = 0x3c;
                    state.a = 0x07;
                }
                break;
            default:
                break;
        }

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);

        switch (reg)
        {
            case "b":
                Assert.Equal(0x08, state.b);
                break;
            case "c":
                Assert.Equal(0x08, state.c);
                break;
            case "d":
                Assert.Equal(0x08, state.d);
                break;
            case "e":
                Assert.Equal(0x08, state.e);
                break;
            case "h":
                Assert.Equal(0x08, state.h);
                break;
            case "l":
                Assert.Equal(0x08, state.l);
                break;
            case "a":
                Assert.Equal(0x08, state.a);
                break;
            default:
                break;
        }
        Assert.False(state.cc.z);
        Assert.False(state.cc.s);
        Assert.True(state.cc.p);
        Assert.True(state.cc.cy);
    }

    [Theory]
    // "b", "c", "d", "e", "h", "l", "a"
    [InlineData("b"), InlineData("c"), InlineData("d"), InlineData("e"), InlineData("h"), InlineData("l"), InlineData("a")]
    public void Inr_should_increment_flags_overflow(string reg)
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[1],
            pc = 0x00,
            b = 0xff,
            cc = new ConditionCodes { s = true }
        };

        switch (reg)
        {
            case "b":
                {
                    state.memory[0] = 0x04;
                    state.b = 0xff;
                }
                break;
            case "c":
                {
                    state.memory[0] = 0x0c;
                    state.c = 0xff;
                }
                break;
            case "d":
                {
                    state.memory[0] = 0x14;
                    state.d = 0xff;
                }
                break;
            case "e":
                {
                    state.memory[0] = 0x1c;
                    state.e = 0xff;
                }
                break;
            case "h":
                {
                    state.memory[0] = 0x24;
                    state.h = 0xff;
                }
                break;
            case "l":
                {
                    state.memory[0] = 0x2c;
                    state.l = 0xff;
                }
                break;
            case "a":
                {
                    state.memory[0] = 0x3c;
                    state.a = 0xff;
                }
                break;
            default:
                break;
        }

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);

        switch (reg)
        {
            case "b":
                Assert.Equal(0x00, state.b);
                break;
            case "c":
                Assert.Equal(0x00, state.c);
                break;
            case "d":
                Assert.Equal(0x00, state.d);
                break;
            case "e":
                Assert.Equal(0x00, state.e);
                break;
            case "h":
                Assert.Equal(0x00, state.h);
                break;
            case "l":
                Assert.Equal(0x00, state.l);
                break;
            case "a":
                Assert.Equal(0x00, state.a);
                break;
            default:
                break;
        }

        Assert.True(state.cc.z);
        Assert.False(state.cc.s);
        Assert.True(state.cc.p);
        Assert.False(state.cc.cy);
    }

    [Theory] // "b", "c", "d", "e", "h", "l", "a"
    [InlineData("b"), InlineData("c"), InlineData("d"), InlineData("e"), InlineData("h"), InlineData("l"), InlineData("a")]
    public void Inr_should_increment_flags2(string reg)
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[1],
            pc = 0x00,
            cc = new ConditionCodes { z = true, p = true, cy = true }
        };

        switch (reg)
        {
            case "b":
                {
                    state.memory[0] = 0x04;
                    state.b = 0xf0;
                }
                break;
            case "c":
                {
                    state.memory[0] = 0x0c;
                    state.c = 0xf0;
                }
                break;
            case "d":
                {
                    state.memory[0] = 0x14;
                    state.d = 0xf0;
                }
                break;
            case "e":
                {
                    state.memory[0] = 0x1c;
                    state.e = 0xf0;
                }
                break;
            case "h":
                {
                    state.memory[0] = 0x24;
                    state.h = 0xf0;
                }
                break;
            case "l":
                {
                    state.memory[0] = 0x2c;
                    state.l = 0xf0;
                }
                break;
            case "a":
                {
                    state.memory[0] = 0x3c;
                    state.a = 0xf0;
                }
                break;
            default:
                break;
        }

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);

        switch (reg)
        {
            case "b":
                Assert.Equal(0xf1, state.b);
                break;
            case "c":
                Assert.Equal(0xf1, state.c);
                break;
            case "d":
                Assert.Equal(0xf1, state.d);
                break;
            case "e":
                Assert.Equal(0xf1, state.e);
                break;
            case "h":
                Assert.Equal(0xf1, state.h);
                break;
            case "l":
                Assert.Equal(0xf1, state.l);
                break;
            case "a":
                Assert.Equal(0xf1, state.a);
                break;
            default:
                break;
        }

        Assert.False(state.cc.z);
        Assert.True(state.cc.s);
        Assert.False(state.cc.p);
        Assert.True(state.cc.cy);
    }

    [Theory] // "b", "c", "d", "e", "h", "l", "a"
    [InlineData("b"), InlineData("c"), InlineData("d"), InlineData("e"), InlineData("h"), InlineData("l"), InlineData("a")]
    public void Dcr_should_decr_flags(string reg)
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[1],
            pc = 0x00,
            cc = new ConditionCodes { s = true, z = true, cy = true }
        };

        switch (reg)
        {
            case "b":
                {
                    state.memory[0] = 0x05;
                    state.b = 0x07;
                }
                break;
            case "c":
                {
                    state.memory[0] = 0x0d;
                    state.c = 0x07;
                }
                break;
            case "d":
                {
                    state.memory[0] = 0x15;
                    state.d = 0x07;
                }
                break;
            case "e":
                {
                    state.memory[0] = 0x1d;
                    state.e = 0x07;
                }
                break;
            case "h":
                {
                    state.memory[0] = 0x25;
                    state.h = 0x07;
                }
                break;
            case "l":
                {
                    state.memory[0] = 0x2d;
                    state.l = 0x07;
                }
                break;
            case "a":
                {
                    state.memory[0] = 0x3d;
                    state.a = 0x07;
                }
                break;
            default:
                break;
        }

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);

        switch (reg)
        {
            case "b":
                Assert.Equal(0x06, state.b);
                break;
            case "c":
                Assert.Equal(0x06, state.c);
                break;
            case "d":
                Assert.Equal(0x06, state.d);
                break;
            case "e":
                Assert.Equal(0x06, state.e);
                break;
            case "h":
                Assert.Equal(0x06, state.h);
                break;
            case "l":
                Assert.Equal(0x06, state.l);
                break;
            case "a":
                Assert.Equal(0x06, state.a);
                break;
            default:
                break;
        }

        Assert.False(state.cc.z);
        Assert.False(state.cc.s);
        Assert.True(state.cc.p);
        Assert.True(state.cc.cy);
    }

    [Theory] // "b", "c", "d", "e", "h", "l", "a"
    [InlineData("b"), InlineData("c"), InlineData("d"), InlineData("e"), InlineData("h"), InlineData("l"), InlineData("a")]
    public void DcrB_should_decr_b_flags_overflow(string reg)
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[1],
            pc = 0x00,
            cc = new ConditionCodes { z = true, p = true }
        };

        switch (reg)
        {
            case "b":
                {
                    state.memory[0] = 0x05;
                    state.b = 0x00;
                }
                break;
            case "c":
                {
                    state.memory[0] = 0x0d;
                    state.c = 0x00;
                }
                break;
            case "d":
                {
                    state.memory[0] = 0x15;
                    state.d = 0x00;
                }
                break;
            case "e":
                {
                    state.memory[0] = 0x1d;
                    state.e = 0x00;
                }
                break;
            case "h":
                {
                    state.memory[0] = 0x25;
                    state.h = 0x00;
                }
                break;
            case "l":
                {
                    state.memory[0] = 0x2d;
                    state.l = 0x00;
                }
                break;
            case "a":
                {
                    state.memory[0] = 0x3d;
                    state.a = 0x00;
                }
                break;
            default:
                break;
        }

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);

        switch (reg)
        {
            case "b":
                Assert.Equal(0xff, state.b);
                break;
            case "c":
                Assert.Equal(0xff, state.c);
                break;
            case "d":
                Assert.Equal(0xff, state.d);
                break;
            case "e":
                Assert.Equal(0xff, state.e);
                break;
            case "h":
                Assert.Equal(0xff, state.h);
                break;
            case "l":
                Assert.Equal(0xff, state.l);
                break;
            case "a":
                Assert.Equal(0xff, state.a);
                break;
            default:
                break;
        }

        Assert.False(state.cc.z);
        Assert.True(state.cc.s);
        Assert.False(state.cc.p);
        Assert.False(state.cc.cy);
    }

    [Theory] // "b", "c", "d", "e", "h", "l", "a"
    [InlineData("b"), InlineData("c"), InlineData("d"), InlineData("e"), InlineData("h"), InlineData("l"), InlineData("a")]
    public void MviR_should_move_to_R(string reg)
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0, 0x88 },
            pc = 0x00,
        };

        switch (reg)
        {
            case "b":
                {
                    state.memory[0] = 0x06;
                }
                break;
            case "c":
                {
                    state.memory[0] = 0x0e;
                }
                break;
            case "d":
                {
                    state.memory[0] = 0x16;
                }
                break;
            case "e":
                {
                    state.memory[0] = 0x1e;
                }
                break;
            case "h":
                {
                    state.memory[0] = 0x26;
                }
                break;
            case "l":
                {
                    state.memory[0] = 0x2e;
                }
                break;
            case "a":
                {
                    state.memory[0] = 0x3e;
                }
                break;
            default:
                break;
        }

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x02, state.pc);

        switch (reg)
        {
            case "b":
                Assert.Equal(0x88, state.b);
                break;
            case "c":
                Assert.Equal(0x88, state.c);
                break;
            case "d":
                Assert.Equal(0x88, state.d);
                break;
            case "e":
                Assert.Equal(0x88, state.e);
                break;
            case "h":
                Assert.Equal(0x88, state.h);
                break;
            case "l":
                Assert.Equal(0x88, state.l);
                break;
            case "a":
                Assert.Equal(0x88, state.a);
                break;
            default:
                break;
        }
    }

    [Fact]
    public void Rlc_should_rotleft_acc_carry()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x07 },
            pc = 0x00,
            a = 0b10000001 // 129, if rotate left it is 0b11 (3)
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0b11, state.a);
        Assert.True(state.cc.cy);
    }

    [Fact]
    public void Rlc_should_rotleft_acc_carry_zero()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x07 },
            pc = 0x00,
            a = 0b00000001, // 1, if rotate left it is 0b10 (2)
            cc = new ConditionCodes { cy = true }
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0b10, state.a);
        Assert.False(state.cc.cy);
    }

    [Fact]
    public void DadB_should_add_bc_hl()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x09 },
            pc = 0x00,
            b = 0x05,
            c = 0x02,
            h = 0x64,
            l = 0x64,
            cc = new ConditionCodes { cy = true }
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0x69, state.h);
        Assert.Equal(0x66, state.l);
        Assert.False(state.cc.cy);
    }

    [Fact]
    public void DadB_should_add_bc_hl_carry()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x09 },
            pc = 0x00,
            b = 0xff,
            c = 0xff,
            h = 0x00,
            l = 0x01,
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0x00, state.h);
        Assert.Equal(0x00, state.l);
        Assert.True(state.cc.cy);
    }

    [Fact]
    public void LdaxB_should_move_bc_to_accumulator_indirect()
    {
        // Arrange
        byte[] mem = new byte[0xffff];
        mem[0] = 0x0a;
        mem[0xff08] = 0x88;
        OpCode sut = new();

        State8080 state = new State8080
        {
            memory = mem,
            pc = 0x00,
            a = 0x00,
            b = 0xff,
            c = 0x08
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0x88, state.a);
    }

    [Fact]
    public void DcxB_should_decr_pair_bc()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x0b },
            pc = 0x00,
            b = 0x07,
            c = 0x07
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0x0706, state.b << 8 | state.c);
    }

    [Fact]
    public void Rrc_should_rotright_acc_carry()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x0f },
            pc = 0x00,
            a = 0b00000011 // rotate right and it is 0b10000001
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0b10000001, state.a);
        Assert.True(state.cc.cy);
    }

    [Fact]
    public void Rrc_should_rotright_acc_carry_zero()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x0f },
            pc = 0x00,
            a = 0b10000000, // rotate right and itll be 0b01
            cc = new ConditionCodes { cy = true }
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0x40, state.a);
        Assert.False(state.cc.cy);
    }

    [Fact]
    public void Ral_should_rotleft_acc_through_carry()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x17 },
            pc = 0x00,
            a = 0b10000001 // 129, if rotate left it is 0b10 (carry is 0) and cy is true
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0b10, state.a);
        Assert.True(state.cc.cy);
    }

    [Fact]
    public void Rar_should_rotright_acc_through_carry()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x27 },
            pc = 0x00,
            a = 0b00000011 // rotate right and it is 0b00000001 (carry 0) and carry to 1
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0b1, state.a);
        Assert.True(state.cc.cy);
    }
}
