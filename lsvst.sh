#!/bin/zsh
pushd ListVst
dotnet run save $*
popd
