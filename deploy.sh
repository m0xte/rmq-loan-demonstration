#!/bin/bash
docker stop quoteapi
docker rm quoteapi
docker run --name quoteapi -p 9045:80 --link redis-main:redis -d quoteapi
