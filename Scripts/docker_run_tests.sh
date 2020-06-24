#!/bin/bash

echo "----------------------build------------------------------"
docker build --target build . -t build
echo "----------------------test-------------------------------"
docker build --target test . -t test --cache-from build
echo "----------------------run test---------------------------"
docker run --rm -i -v C:/luizfelipe/myexpenses-backend/coverage:/app/coverage test
echo "----------------------final------------------------------"
docker build --target final . -t final --cache-from build
echo "----------------------run------------------------------"
# docker run --rm -i final