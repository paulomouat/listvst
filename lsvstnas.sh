#!/bin/zsh
pushd ListVst
dotnet run save $* --source-path /Volumes/projects/music
popd
