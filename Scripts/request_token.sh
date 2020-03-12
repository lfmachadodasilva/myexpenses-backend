#!/bin/bash

PROJECT_KEY='AIzaSyAVfjm2M_d7mmVbD4kOwVWxp57CQZV8jqQ'
EMAIL='user@test.com'
PASSWORD='testtest'

curl 'https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key='$PROJECT_KEY \
-H 'Content-Type: application/json' \
--data-binary '{"email":"'$EMAIL'","password":"'$PASSWORD'","returnSecureToken":true}'