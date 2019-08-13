#!/bin/bash

docker stop postgres
docker rm postgres
docker run -d \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_USER=postgres \
  --name postgres \
  -p 5432:5432 \
  --restart=always \
  postgres