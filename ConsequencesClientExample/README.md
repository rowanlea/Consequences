# Consequences Client Example
## Overview
This is my approach at the consequences kata designed for the LSCC.

It was done using:
- C#
- NUnit
- NSubstitute
- Marfusios Websocket client - https://github.com/Marfusios/websocket-client

## TDD Approach
When doing this kata I decided to run two series of acceptance tests, one for the Websocket client implementation, and one for the game loop.

This allowed me to integration test the Websocket side to make sure I could successfully connect to the server, and verify both the technology and connection worked properly.
This then allowed me to use mocking to TDD the logic of the game loop completely separately, so I could focus on just the logic of the gameplay.

I developed this using a combination of inside-out and outside-in TDD, creating a series of small incremental acceptance tests to progress the functionality of the client.
You can see this progression by reading the WebsocketAcceptanceTests and the GameplayAcceptanceTests in order, or by looking through my commit history.

## End Result
The end result of my TDD approach was:
- the whole solution was easy to create
- it was easy to refactor along the way
- it was very easy to implement the concrete game at the end (required basically no work)