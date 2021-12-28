Write-Warning "Define var: Start"

$EnvMapping = @{}

function Add-Env-Mapping {
    param( [String]$StackName, [String]$NewRelicName, [String]$WebConfigName, [string]$Environment)
    $envObj = New-Object PSObject
    $envObj | Add-Member -MemberType NoteProperty -Name Name -Value $StackName
    $envObj | Add-Member -MemberType NoteProperty -Name NewRelicName -Value $NewRelicName
    $envObj | Add-Member -MemberType NoteProperty -Name WebConfig -Value $WebConfigName
    $envObj | Add-Member -MemberType NoteProperty -Name Environment -Value $Environment
    $EnvMapping.Add($StackName,$envObj)
}

#Add-Env-Mapping 
#Channel: US
#Distributors
Add-Env-Mapping "BR1APPDQM" "" "Web.config.QA-Master" "DEV"
Add-Env-Mapping "BR1APPDQM2" "" "Web.config.QA-Master" "QA"
Add-Env-Mapping "oneapp-dis-psfcore-dev" "" "Web.config.QA-Master-psf-core" "UAT"
Add-Env-Mapping "BR1APPDQR" "" "Web.config.QA-Oneapp" "PROD"

#Stack data Lookup 
$StackObj = $EnvMapping["oneapp-dis-psfcore-dev"]

if ($StackObj -eq $null) {
  $str = $StackObj | Out-String
  Write-Host $str -ForegroundColor red
  Write-Warning "updateWebConfig: StackObj is null"
  Exit
}
$NewRelicName = $StackObj.NewRelicName
$NewWebConfig = $StackObj.WebConfig

Set-Variable Environment $StackObj.Environment -Scope global

Write-Warning "Finish"