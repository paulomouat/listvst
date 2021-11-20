#!/bin/zsh
pushd ListVst/bin/Debug/net5.0
dotnet ./ListVst.dll /Volumes/projects/music | tee ../../../../output.txt
popd