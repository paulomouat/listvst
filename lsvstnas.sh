#!/bin/zsh
DIR=${PWD}
pushd ListVst/bin/Debug/net9.0
./ListVst save $* --source-path /Volumes/projects/music/projects
popd
