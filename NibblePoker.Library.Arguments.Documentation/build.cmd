@echo off

:: Going into the script's directory
pushd %~dp0

:: ...
cd docs

:: Making the documentation
rmdir html /s /q
doxygen

popd
