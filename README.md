# Consequences
## Overview
Today we'll be playing a game called Consequences ([Wikipedia](https://en.wikipedia.org/wiki/Consequences_(game))). This is an old parlour game traditionally played with pen and paper, where players are asked a series of questions and have to respond by writing their answer on a piece of paper, folding their answer over, and then passing their piece of paper on to the person sitting next to them, cumulating in a funny/interesting story at the end, when all of the questions have been asked.

For our game of consequences we have a preset list of questions we're going to follow, in this format:
- "Please enter an adjective, followed by a person's name"
- "Please enter another adjective, followed by a different person's name"
- "Please enter a place where people could meet"
- "Please enter why they were there"
- "Please enter something the first person did"
- "Please enter something the second person did"
- "Please enter the consequence of their actions"

## The Task
The aim of the challenge is to create a websocket client to connect to a pre-established server that allows the user to play the game.

## Inputs/Outputs
For the server to work successfully there are a few steps you need to follow with the information you send to the server.

The **inputs** the server will take are as follows:
- "Name" - the name in which other people will identify you.
- "Room" - an identifier used to play with specific people in a session. You can also be in a room by yourself.
- "Answer" - your response to the question returned.

The **outputs** the server will send back to you are:
- "Message" - an information message, generally to advise on what to do next. If there's an error this message will begin with "ERROR:"
- "Players" - a list of players sent with non-error messages for you to see who you are playing with
- "Question" - a question from the list above, which denotes the server is expecting an answer from you
- "Results" - a list of results from all of the players

These need to be be JSON formatted, i.e. they will look like the following:
> {"Message": "Hello"}

The flow for the game will look something like:
1. **Client:** initial message, can be any message at all, JSON or otherwise
2. **Server:** Will respond with a welcome "Message" and prompt for some information
3. **Client:** Needs to send back "Name" and "Room"
4. **Server:** Will respond with a series of "Message" containing instructions, and "Question" for the user to respond to, along with a list of "Players"
5. **Client:** Needs to respond going forwards with a series of "Answer"
6. **Server:** Will finally give you a "Results" list

## Example Result
In the end you will end up with a whole result per person playing the game, each result looking something like:
> Magnificent Margaret met Hungry Henry at the post office to buy shoes.
Margaret ate an apple, whilst Henry did their maths homework.
The consequence of their actions was aliens invaded the planet.

## TDD Approach
There are lots of different methods you could use to TDD this kata:
- Mocking/stubbing
- Both inside-out and outside-in
- With integration tests in a “Room” by yourself
- Using two clients in your tests

## Server Connection
To connect to the websocket server you need to use: **ws://51.141.52.52:1234**

## Notes
- The extra punctuation and words to structure the final results are coded into the server, so you just need to answer the question normally.
- If you run multiple tests too quickly one after the other they might run before your room has been cleared up, you can prevent this by using a different room in each of your tests.
