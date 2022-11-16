package tests;
import main;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertSame;

class SocketClientTests {

    @Test
    void demoTestMethod() {
        // Arrange
        SocketClient socketClient = new SocketClient("ws://51.141.52.52:1234");
        socketClient.Connect();

        // Act
        OutboundMessage message = new OutboundMessage();
        message.Hello = "Hello";
        socketClient.Send(message);
        var response = socketClient.Receive();

        // Assert
        assertSame("Welcome to Consequences, to get started send your name and room code.", response.Message);
    }
}
