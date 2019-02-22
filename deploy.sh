#!/bin/bash
docker stop quoteapi
docker wait quoteapi
docker rm quoteapi
docker wait quoteapi
docker run --name quoteapi -p 9045:80 --link redis-main:redis -d quoteapi
