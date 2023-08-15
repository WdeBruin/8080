using cpu;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

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

    [Fact]
    public void LxiB_should_load_correctly()
    {
        // Arrange
        State8080 state = new State8080
        {
            memory = new byte[] { 0x01, 0x08, 0xF8 },
            pc = 0x00
        };
        OpCode sut = new();

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x03, state.pc);
        Assert.Equal(0xF8, state.b);
        Assert.Equal(0x08, state.c);
    }

    [Fact]
    public void StaxB_should_move_accumulator_to_bc_indirect_0xff01()
    {
        // Arrange
        byte[] mem = new byte[0xffff];
        mem[0] = 0x02;
        OpCode sut = new();

        State8080 state = new State8080
        {
            memory = mem,
            pc = 0x00,
            a = 0x08,
            b = 0xff,
            c = 0x01
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0x08, state.memory[0xff01]);
    }

    [Fact]
    public void InxB_should_increment_pair_bc()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x03 },
            pc = 0x00,
            b = 0x07,
            c = 0x07
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0x0708, state.b << 8 | state.c);
    }

    [Fact]
    public void InrB_should_increment_b_flags()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x04 },
            pc = 0x00,
            b = 0x07,
            cc = new ConditionCodes { s = true, z = true, cy = true }
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0x08, state.b);
        Assert.False(state.cc.z);
        Assert.False(state.cc.s);
        Assert.True(state.cc.p);
        Assert.True(state.cc.cy);
    }

    [Fact]
    public void InrB_should_increment_b_flags_overflow()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x04 },
            pc = 0x00,
            b = 0xff,
            cc = new ConditionCodes { s = true }
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0x00, state.b);
        Assert.True(state.cc.z);
        Assert.False(state.cc.s);
        Assert.True(state.cc.p);
        Assert.False(state.cc.cy);
    }

    [Fact]
    public void InrB_should_increment_b_flags2()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x04 },
            pc = 0x00,
            b = 0xf0,
            cc = new ConditionCodes { z = true, p = true, cy = true }
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0xf1, state.b);
        Assert.False(state.cc.z);
        Assert.True(state.cc.s);
        Assert.False(state.cc.p);
        Assert.True(state.cc.cy);
    }

    [Fact]
    public void DcrB_should_decr_b_flags()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x05 },
            pc = 0x00,
            b = 0x07,
            cc = new ConditionCodes { s = true, z = true, cy = true }
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0x06, state.b);
        Assert.False(state.cc.z);
        Assert.False(state.cc.s);
        Assert.True(state.cc.p);
        Assert.True(state.cc.cy);
    }

    [Fact]
    public void DcrB_should_decr_b_flags_overflow()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x05 },
            pc = 0x00,
            b = 0x00,
            cc = new ConditionCodes { z = true, p = true }
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x01, state.pc);
        Assert.Equal(0xff, state.b);
        Assert.False(state.cc.z);
        Assert.True(state.cc.s);
        Assert.False(state.cc.p);
        Assert.False(state.cc.cy);
    }

    [Fact]
    public void MviB_should_move_to_b()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x06, 0x88 },
            pc = 0x00,
            b = 0x00
        };

        // Act
        sut.Emulate8080Op(ref state);

        // Assert
        Assert.Equal(0x02, state.pc);
        Assert.Equal(0x88, state.b);
    }

    [Fact]
    public void Rlc_should_rot_acc_carry()
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
    public void Rlc_should_rot_acc_carry_zero()
    {
        // Arrange
        OpCode sut = new();
        State8080 state = new State8080
        {
            memory = new byte[] { 0x07 },
            pc = 0x00,
            a = 0b00000001 // 1, if rotate left it is 0b10 (2)
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
}
