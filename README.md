# Compare the Mongoose 

Because meerkats are lame

![meerkats are lame](images/ctm.png)

A simple Redis / .Net Core architectural demonstration

## Requirements:

* Computer
* .Net Core 3

## Instructions

Instructions are currently for windows only. Redis is available for OSX and Linux so 
there should be no portability issues with this example.

1. Install redis-server in WSL or natively
2. Start redis-server in WSL 
3. Set all projects to run on startup if using VS or run using dotnet run (both quote providers and the quote API server)

## To use:

To request a quote:

`$ curl -X POST -H "Content-type: application/json" -d "{'Name':'bob'}" http://localhost:1659/api/quote/new`

This will return a correlation id which can be used to retrieve quotes later.

To get the quote results:

`$ curl http://localhost:1659/api/quote/results/correlationId`

Replace correlationId with the result from the initial new quote request

## How it works:

1. End user calls into quote service to request a new quote
2. This sends a message to each provider via redis queue
3. The quote provider processes the quotes. Usually a latent operation.
4. The quote provider publishes a response message to a response queue
5. The end user polls for results (or may notify if you need to - up to the implementor to do)

## Notes

* In real life error handling would be better :)