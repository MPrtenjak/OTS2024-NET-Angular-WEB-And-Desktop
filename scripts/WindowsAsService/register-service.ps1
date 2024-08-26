$currentFolder = Get-Location

$scriptFolder = Split-Path -Path $currentFolder -Parent
$appFolder = Split-Path -Path $scriptFolder -Parent

New-Service -Name GoodDayService `
            -BinaryPathName "$appFolder\GoodDayWebApp.exe --contentRoot $appFolder\GoodDayWebApp.exe" `
            -Description "GoodDayService DESCRIPTION" `
            -DisplayName "GoodDayService"
