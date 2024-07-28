#!/bin/zsh
DIR=${PWD}
pushd ListVst/bin/Debug/net8.0
./ListVst save $*
popd
