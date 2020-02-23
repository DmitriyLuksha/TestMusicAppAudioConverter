[CmdletBinding()]
Param (
    [Parameter(Mandatory=$True)]
    [string]$ServiceBusConnectionString,

    [Parameter(Mandatory=$True)]
    [string]$AudioConversionQueueName,

    [Parameter(Mandatory=$True)]
    [string]$YoutubeConversionQueueName,

    [Parameter(Mandatory=$True)]
    [string]$AudioUploadingResultQueueName,

    [Parameter(Mandatory=$True)]
    [string]$StorageConnectionString,

    [Parameter(Mandatory=$True)]
    [string]$AudioFilesContainerName,

    [Parameter(Mandatory=$True)]
    [string]$UnprocessedAudioFilesContainerName,

    [Parameter(Mandatory=$False)]
    [string]$AudioConverterInstrumentationKey,

    [Parameter(Mandatory=$False)]
    [string]$YoutubeConverterInstrumentationKey
)

Function Set-PlaceholderValue($Content, $Placeholder, $Value) {
    return $Content.Replace("*$Placeholder*", $Value);
}

Function Get-ConfigTemplatePath($FileName) {
    return [System.IO.Path]::GetFullPath("$PSScriptRoot\..\TestMusicAppServices\ApplicationParameters\$FileName.Template.xml");
}

Function Get-ConfigPath($FileName) {
    return [System.IO.Path]::GetFullPath("$PSScriptRoot\..\TestMusicAppServices\ApplicationParameters\$FileName.xml");
}

Function New-ConfigFromTemplate($FileName) {
    $ConfigTemplatePath = Get-ConfigTemplatePath($FileName);
    $ConfigPath = Get-ConfigPath($FileName);

    $ConfigContent = Get-Content -Path $ConfigTemplatePath;

    $ConfigContent = Set-PlaceholderValue -Content $ConfigContent -Placeholder "TestMusicApp.ServiceBusSettings.ConnectionString" -Value $ServiceBusConnectionString;
    $ConfigContent = Set-PlaceholderValue -Content $ConfigContent -Placeholder "TestMusicApp.ServiceBusSettings.AudioConversionQueueName" -Value $AudioConversionQueueName;
    $ConfigContent = Set-PlaceholderValue -Content $ConfigContent -Placeholder "TestMusicApp.ServiceBusSettings.YoutubeConversionQueueName" -Value $YoutubeConversionQueueName;
    $ConfigContent = Set-PlaceholderValue -Content $ConfigContent -Placeholder "TestMusicApp.ServiceBusSettings.AudioUploadingResultQueueName" -Value $AudioUploadingResultQueueName;
    $ConfigContent = Set-PlaceholderValue -Content $ConfigContent -Placeholder "TestMusicApp.StorageSettings.ConnectionString" -Value $StorageConnectionString;
    $ConfigContent = Set-PlaceholderValue -Content $ConfigContent -Placeholder "TestMusicApp.StorageSettings.AudioFilesContainerName" -Value $AudioFilesContainerName;
    $ConfigContent = Set-PlaceholderValue -Content $ConfigContent -Placeholder "TestMusicApp.StorageSettings.UnprocessedAudioFilesContainerName" -Value $UnprocessedAudioFilesContainerName;
    $ConfigContent = Set-PlaceholderValue -Content $ConfigContent -Placeholder "TestMusicApp.AudioConverter.InstrumentationKey" -Value $AudioConverterInstrumentationKey;
    $ConfigContent = Set-PlaceholderValue -Content $ConfigContent -Placeholder "TestMusicApp.YoutubeConverter.InstrumentationKey" -Value $YoutubeConverterInstrumentationKey;
    
    If (!(Test-Path -Path $ConfigPath)) {
        Write-Host "Creating file $ConfigPath";
        New-Item -Path $ConfigPath | Out-Null;
    }

    Write-Host "Updating content of $ConfigPath";
    Set-Content -Path $ConfigPath -Value $ConfigContent | Out-Null;
}

New-ConfigFromTemplate("Local.1Node");
New-ConfigFromTemplate("Local.5Node");