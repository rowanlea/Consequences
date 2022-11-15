import json
import queue
from websocket import WebSocketApp, create_connection

from Messages import InboundMessage

try:
    import thread
except ImportError:
    import _thread as thread


class SocketClient:
    def __init__(self, uri):
        self.uri = uri
        self.messages = queue.Queue()
        self.ws = create_connection(self.uri)

    def receive(self) -> InboundMessage:
        json_message = self.ws.recv()
        json_obj = json.loads(json_message)
        message_obj = InboundMessage(message=json_obj["Message"])
        return message_obj

    def send(self, message):
        json_message = json.dumps(message)
        self.ws.send(json_message)
