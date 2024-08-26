@REM Delete target folder
rd /q /s GoodDayRelease

@REM Build .NET BACKEND

@cd GoodDayWebApp
@rd /q/s bin
@rd /q/s obj
@dotnet publish GoodDayWebApp.csproj  -c Release -f net8.0
@SET BACKEND_SOURCE=GoodDayWebApp\bin\Release\net8.0\publish
@cd ..

@REM Build Angular FRONTEND

@cd GoodDaySPA
@rd /q/s dist
@cmd /c ng build --configuration production
@SET FRONTEND_SOURCE=GoodDaySPA\dist\good-day-spa
@cd ..

@REM BUILD FINAL RELEASE

@rd /q /s GoodDayRelease
@mkdir GoodDayRelease
@xcopy %BACKEND_SOURCE%\ GoodDayRelease /s /e /y

@REM Remove unnecessary localization files
@cd GoodDayRelease
@rd /s /q zh-Hans
@rd /s /q zh-Hant
@rd /s /q tr
@rd /s /q ru
@rd /s /q ko
@rd /s /q pl
@rd /s /q pt-BR
@rd /s /q de
@rd /s /q es
@rd /s /q fr
@rd /s /q it
@rd /s /q ja
@rd /s /q cs
@cd ..

@REM copy frontend files to wwwroot folder of backend
@xcopy %FRONTEND_SOURCE%\browser GoodDayRelease\wwwroot /s /e /y
@del GoodDayRelease\wwwroot\assets\config.json

@REM Add optional scripts for easier instalations
@xcopy scripts GoodDayRelease\scripts /s /e /y /i


