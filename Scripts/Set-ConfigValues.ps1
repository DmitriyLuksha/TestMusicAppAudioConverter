<#
.DESCRIPTION
    Set config values
#>

[CmdletBinding()]
Param (
    [Parameter(Mandatory=$True)]
    [string]$ServiceBusSettingsConnectionString,

    [Parameter(Mandatory=$True)]
    [string]$ServiceBusSettingsAudioConversionQueueName,

    [Parameter(Mandatory=$True)]
    [string]$ServiceBusSettingsYoutubeConversionQueueName,

    [Parameter(Mandatory=$True)]
    [string]$ServiceBusSettingsAudioUploadingResultQueueName,

    [Parameter(Mandatory=$True)]
    [string]$StorageSettingsConnectionString,

    [Parameter(Mandatory=$True)]
    [string]$StorageSettingsAudioFilesContainerName,

    [Parameter(Mandatory=$True)]
    [string]$StorageSettingsUnprocessedAudioFilesContainerName,

    [Parameter(Mandatory=$False)]
    [string]$AudioConverterInstrumentationKey,

    [Parameter(Mandatory=$False)]
    [string]$YoutubeConverterInstrumentationKey
)

Function Set-PlaceholderValue($Content, $Placeholder, $Value) {
    return $Content.Replace("*$Placeholder*", $Value);
}

Function Get-ConfigTemplatePath($FileName) {
    return "..\TestMusicAppServices\ApplicationParameters\$FileName.Template.xml";
}

Function Get-ConfigPath($FileName) {
    return "..\TestMusicAppServices\ApplicationParameters\$FileName.xml";
}

Function New-ConfigFromTemplate($FileName) {
    $ConfigTemplatePath = Get-ConfigTemplatePath($FileName);
    $ConfigPath = Get-ConfigPath($FileName);

    $FileContent = Get-Content -Path $ConfigTemplatePath;

    $FileContent = Set-PlaceholderValue -Content $FileContent -Placeholder "TestMusicApp.ServiceBusSettings.ConnectionString" -Value $ServiceBusSettingsConnectionString;
    $FileContent = Set-PlaceholderValue -Content $FileContent -Placeholder "TestMusicApp.ServiceBusSettings.AudioConversionQueueName" -Value $ServiceBusSettingsAudioConversionQueueName;
    $FileContent = Set-PlaceholderValue -Content $FileContent -Placeholder "TestMusicApp.ServiceBusSettings.YoutubeConversionQueueName" -Value $ServiceBusSettingsYoutubeConversionQueueName;
    $FileContent = Set-PlaceholderValue -Content $FileContent -Placeholder "TestMusicApp.ServiceBusSettings.AudioUploadingResultQueueName" -Value $ServiceBusSettingsAudioUploadingResultQueueName;
    $FileContent = Set-PlaceholderValue -Content $FileContent -Placeholder "TestMusicApp.StorageSettings.ConnectionString" -Value $StorageSettingsConnectionString;
    $FileContent = Set-PlaceholderValue -Content $FileContent -Placeholder "TestMusicApp.StorageSettings.AudioFilesContainerName" -Value $StorageSettingsAudioFilesContainerName;
    $FileContent = Set-PlaceholderValue -Content $FileContent -Placeholder "TestMusicApp.StorageSettings.UnprocessedAudioFilesContainerName" -Value $StorageSettingsUnprocessedAudioFilesContainerName;
    $FileContent = Set-PlaceholderValue -Content $FileContent -Placeholder "TestMusicApp.AudioConverter.InstrumentationKey" -Value $AudioConverterInstrumentationKey;
    $FileContent = Set-PlaceholderValue -Content $FileContent -Placeholder "TestMusicApp.YoutubeConverter.InstrumentationKey" -Value $YoutubeConverterInstrumentationKey;
    
    If (!(Test-Path -Path $ConfigPath)) {
        New-Item -Path $ConfigPath | Out-Null;
        Write-Host "Create file $ConfigPath";
    }

    Set-Content -Path $ConfigPath -Value $FileContent | Out-Null;
    Write-Host "Update content of $ConfigPath";
}

New-ConfigFromTemplate("Local.1Node");
New-ConfigFromTemplate("Local.5Node");