#!/bin/bash

PROJECT_KEY='AIzaSyDMDE7eTQbjwkQglMJf5KnFtMr48-pAoVM'
EMAIL='user@test.com'
PASSWORD='testtest'

curl 'https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key='$PROJECT_KEY \
-H 'Content-Type: application/json' \
--data-binary '{"email":"'$EMAIL'","password":"'$PASSWORD'","returnSecureToken":true}'