import unittest

from SocketClient import SocketClient


class SocketClientTests(unittest.TestCase):
    def test_connect_to_server(self):
        # arrange
        socket_client = SocketClient("ws://51.141.52.52:1234")

        # act
        socket_client.send({"Hello": "Hello"})
        response = socket_client.receive()

        # assert
        self.assertEqual("Welcome to Consequences, to get started send your name and room code.", response.message)
