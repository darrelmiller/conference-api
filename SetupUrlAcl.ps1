set-alias netsh c:\Windows\System32\netsh.exe
$PORT = 1001
$domain = $Env:userdomain
$name = $Env:username
$ErrorActionPreference = "Continue";

netsh http delete urlacl url=http://localhost:$PORT/
netsh http add urlacl url=http://localhost:$PORT/ user=$domain\$name