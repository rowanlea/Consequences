import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;

class SocketClientTests {

    @Test
    void demoTestMethod() {
        // Arrange
        SocketClient socketClient = new SocketClient("ws://51.141.52.52:1234");

        // Act
        OutboundMessage message = new OutboundMessage();
        message.Hello = "Hello";
        socketClient.Send(message);
        var response = socketClient.Receive();

        // Assert
        assertEquals("Welcome to Consequences, to get started send your name and room code.", response.Message);
    }
}
