import json
import queue
from websocket import WebSocketApp
from Messages import InboundMessage, OutboundMessage

try:
    import thread
except ImportError:
    import _thread as thread


class SocketClient:
    def __init__(self, uri):
        self.uri = uri
        self.messages = queue.Queue()
        self.ws = WebSocketApp(self.uri,
                               on_message=self._on_message,
                               on_error=self._on_error,
                               on_close=self._on_close)
        self.ws.on_open = self._on_open

    def connect(self):
        self.ws.run_forever()

    def receive(self) -> InboundMessage:
        json_message = self.messages.get(block=True)
        json_obj = json.loads(json_message)
        message_obj = InboundMessage(message=json_obj["Message"])
        return message_obj

    def send(self, message: OutboundMessage):
        json_message = json.dumps(message)
        self.ws.send(json_message)

    def _on_message(self, ws, message):
        self.messages.put(message)

    @staticmethod
    def _on_error(ws, error):
        print(error)

    @staticmethod
    def _on_close(ws):
        print("### closed ###")

    @staticmethod
    def _on_open(ws):
        print("Opened connection")
