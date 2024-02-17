$exePath = "c:\Users\Roman\OneDrive\Source\MSAccessHttp\src\WorkerService\bin\Debug\net8.0\WorkerService.exe"
$exeFolderPath = "c:\Users\Roman\OneDrive\Source\MSAccessHttp\src\WorkerService\bin\Debug\net8.0"
$userName = "$env:computername\MSAccessService"

$acl = Get-Acl $exeFolderPath
$aclRuleArgs = $userName, "Read,Write,ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($aclRuleArgs)
$acl.SetAccessRule($accessRule)
$acl | Set-Acl $exeFolderPath

New-Service -Name "MSAccess-Http" -BinaryPathName "$exePath --contentRoot $exeFolderPath" -Credential $userName -Description "Https Service Connected to MS Access" -DisplayName "MSAccess-Http" -StartupType Automatic

Start-Service -Name "MSAccess-Http"
