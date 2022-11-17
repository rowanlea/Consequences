import com.google.gson.Gson;

import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.WebSocket;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.CompletionStage;
import java.util.concurrent.LinkedBlockingDeque;

public class SocketClient {
    WebSocket ws;
    WebSocketClient wsc;
    Gson gson = new Gson();

    public SocketClient(String uri){
        wsc = new WebSocketClient();
        ws = HttpClient
                .newHttpClient()
                .newWebSocketBuilder()
                .buildAsync(URI.create(uri), wsc)
                .join();
    }

    public void Send(OutboundMessage message)
    {
        String jsonMessage = gson.toJson(message);
        ws.sendText(jsonMessage, true);
    }

    public InboundMessage Receive()
    {
        String jsonMessage = wsc.GetMessageFromQueue();
        return gson.fromJson(jsonMessage, InboundMessage.class);
    }

    private static class WebSocketClient implements WebSocket.Listener {
        private final BlockingQueue<String> messageList;

        public WebSocketClient()
        {
            messageList = new LinkedBlockingDeque<String>();
        }

        public String GetMessageFromQueue()
        {
            try {
                return messageList.take();
            }
            catch (InterruptedException e)
            {
                return "Failed to take message from queue";
            }
        }

        @Override
        public void onOpen(WebSocket webSocket) {
            System.out.println("Websocket open");
            WebSocket.Listener.super.onOpen(webSocket);
        }

        @Override
        public CompletionStage<?> onText(WebSocket webSocket, CharSequence data, boolean last) {
            messageList.add(data.toString());
            return WebSocket.Listener.super.onText(webSocket, data, last);
        }

        @Override
        public void onError(WebSocket webSocket, Throwable error) {
            System.out.println("Error: " + error.getMessage());
            WebSocket.Listener.super.onError(webSocket, error);
        }
    }
}
