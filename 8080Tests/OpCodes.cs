using cpu;

public class OpCodeTests
{
    [Fact]
    public void Nop_should_change_nothing()
    {
        // Arrange
        State8080 state = new();
        OpCode sut = new();
        state.pc = 0x00;
        state.memory = new byte[] {0x00};

        // Act
        sut.Emulate8080Op(state);

        // Assert
        Assert.Equal(new State8080(), state);
    }
}
