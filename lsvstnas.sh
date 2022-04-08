#!/bin/zsh
DIR=${PWD}
pushd ListVst/bin/Debug/net6.0
./ListVst save $* --source-path /Volumes/projects/music/projects
popd
