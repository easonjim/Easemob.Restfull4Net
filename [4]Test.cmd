@echo off
pushd "%~dp0"
NAnt -buildfile:NAnt.build Easemob.Restfull4Net.Test_Test
pause