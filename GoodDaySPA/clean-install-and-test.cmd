@echo off
setlocal

rd /q/s node_modules

if "%1" == "yarn" (
    call yarn install
    call yarn test
) else (
    call npm install
    @REM call npm test
)

endlocal