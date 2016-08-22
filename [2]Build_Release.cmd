@echo off
pushd "%~dp0"
NAnt -buildfile:NAnt.build Easemob.Restfull4Net_BuildOut_Release
pause