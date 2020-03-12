#!/bin/bash

dotnet dev-certs https --clean
dotnet dev-certs https
sudo security set-key-partition-list -D localhost -S unsigned:,teamid:UBF8T346G9