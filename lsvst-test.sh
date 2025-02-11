#!/bin/zsh
DIR=${PWD}
pushd ListVst/bin/Debug/net9.0
./ListVst save --format txt --file $DIR/o.txt --format html --file $DIR/o.html --source-path ~/Documents/projects/music
popd
