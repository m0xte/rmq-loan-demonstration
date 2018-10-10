# Compare the Mongoose 

Because meerkats are lame

![meerkats are lame](images/ctm.png)

A simple RabbitMQ / .Net Core architectural demonstration

## Instructions: 

1. Install erlang OTP from http://erlang.org/download/otp_win64_21.1.exe
2. Install RabbitMQ from https://github.com/rabbitmq/rabbitmq-server/releases/download/v3.7.8/rabbitmq-server-3.7.8.exe
3. (default installs fine for both of the above)
4. Install redis-server in WSL
5. Start redis-server in WSL 
6. Set both projects to run
7. Run as many of the QuoteProvider project instances as you need to scale!

## To use:

To request a quote:

`$ curl -X POST -H "Content-type: application/json" -d "{'Name':'bob'}" http://localhost:1659/api/quote/new`

This will return a correlation id

To get the quote results:

`$ curl http://localhost:1659/api/quote/results/GUIDHERE`

## How it works:

1. End user calls into quote service to request a new quote
2. This sends a message to each provider queue via aggregator RabbitMQ
3. The quote provider processes the quotes. Usually a latent operation.
4. The quote provider calls the quote service back to deliver a result.
5. The end user polls for results (or may notify if you need to - up to the implementor to do)

## Notes

* The messaging here is durable and will retry until successful.
* In real life error handling would be better :)