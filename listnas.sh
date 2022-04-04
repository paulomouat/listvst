#!/bin/zsh
pushd ListVst/bin/Debug/net6.0
#dotnet ./ListVst.dll /Volumes/projects/music | tee ../../../../output.txt
dotnet ./ListVst.dll $*
popd