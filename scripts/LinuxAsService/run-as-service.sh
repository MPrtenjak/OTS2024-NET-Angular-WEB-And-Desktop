#!/bin/bash

source ./stop-service.sh

PARENT_DIR=$(dirname "$(pwd)")
PARENT_DIR=$(dirname $PARENT_DIR)
FILE_NAME="GoodDayWebApp.service"

if [ -f "$FILE_NAME" ]; then
    sed -i "s|^WorkingDirectory=.*$|WorkingDirectory=$PARENT_DIR|" "$FILE_NAME"
fi

cp GoodDayWebApp.service /etc/systemd/system
systemctl daemon-reload
systemctl start GoodDayWebApp