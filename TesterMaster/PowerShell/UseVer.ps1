$SecretManagerEnvMapping = @{}

function Add-Secrets-Manager-Env-Mapping {
    param( [String]$key, [String]$value)
    $SecretManagerEnvMapping.Add($key,$value)
}

Add-Secrets-Manager-Env-Mapping "DEV" "dev"
Add-Secrets-Manager-Env-Mapping "QA" "qa"
Add-Secrets-Manager-Env-Mapping "UAT" "uat"
Add-Secrets-Manager-Env-Mapping "PROD" "prod"

$secretEnv = $SecretManagerEnvMapping["$Environment"]
Remove-Variable Environment -Scope global

Write-Warning $secretEnv
