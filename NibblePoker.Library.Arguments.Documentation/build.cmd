@echo off

:: Going into the script's directory
pushd %~dp0

:: Making the documentation
rmdir html /s /q
doxygen

:: Going back to the original directory
popd
