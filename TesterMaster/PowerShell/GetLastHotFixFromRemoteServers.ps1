$Global:Output = @()
$Global:EmptyRow = "" | Select Server, Last90days, LastPatch, InstalledOn , Description
$Global:Row = $Global:EmptyRow.PsObject.Copy()
$OutputFile = "servers_output.csv"
$ServerList = Get-Content "servers.txt"

function TryCreateSession([string] $server)
{
    $session = New-PSSession -ComputerName $server -Credential $Global:userCreds

    if(!$session){
        $Global:Row.LastPatch = "Server $server is not available"
        throw "Server $server is not available"
    }

    return $session
}

function ProccessPatchInfo([System.Array] $hotFixes)
{
    $hotFixesBy90Days = $hotFixes |?{$_.InstalledOn -gt ((Get-Date).AddDays(-90))}

    if($hotFixesBy90Days){
        $isUpToDate = "YES"
    }
    else{
        $isUpToDate = "NO"
    }

    $lastHotFix = ($hotFixes | Sort-Object -Descending -Property InstalledOn -ErrorAction SilentlyContinue | Select-Object -First 1)

    $lastPatch = $lastHotFix.HotFixID
    $date = $lastHotFix.InstalledOn
    $description = $lastHotFix.Description

    $Global:Row.Last90days = $isUpToDate
    $Global:Row.LastPatch = $lastPatch
    $Global:Row.InstalledOn = $date
    $Global:Row.Description = $description
}

function ProccessServerPatches([string] $server)
{
    try{
        $Global:Row = $Global:EmptyRow.PsObject.Copy()

        $Global:Row.Server = $server

        $session = TryCreateSession($server)

        $hotFixes =  Invoke-Command -Session $session { Get-HotFix }

        ProccessPatchInfo($hotFixes)

        Remove-PSSession -Session $session
    }
    catch{
        Write-Warning "An error occurred:"
        Write-Warning $_
    }
    finally{
        $Global:Output += $Global:Row
    }
}

try{
    $Global:userCreds = Get-Credential
    $serverCounter = 0;
    $serverListLenght = $ServerList.Count;

    ForEach ($server in $ServerList) 
    {
        $percent =  $serverCounter/$serverListLenght * 100;
        $serverCounter += 1;

        Write-Progress -Activity "Server patches " -Status "$i% Complete:" -PercentComplete $percent
        ProccessServerPatches($server)
    }

    $Global:Output | Export-Csv -Path $OutputFile -NoTypeInformation
}
catch{
    Write-Warning "An error occurred:"
    Write-Warning $_
}
