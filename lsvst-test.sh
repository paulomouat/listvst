#!/bin/zsh
DIR=${PWD}
pushd ListVst/bin/Debug/net6.0
./ListVst save --format txt --file $DIR/o.txt --format txt --file $DIR/o2.txt --source-path ~/Documents/projects/music
popd
