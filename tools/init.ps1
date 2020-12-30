param($installPath, $toolsPath, $package)

# param($installPath, $toolsPath, $package, $project)
$logFile = "D:\temp\testlog.txt"
Add-Content -Path $logFile -Value "Installing restclientapp to $installPath"
Add-Content -Path $logFile -Value "Tools path is $toolsPath"
$currentDir = Join-Path $installPath "current"
Add-Content -Path $logFile -Value "Current dir path will be '$currentDir'"
# Remove-Item $currentDir -Force -Recurse
New-Item -ItemType SymbolicLink -Path $currentDir -Value $installPath -Name "current"
Add-Content -Path $logFile -Value "Created symlink"
