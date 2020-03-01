$ErrorActionPreference = "Stop";

$OrchestratorPath = "$PSScriptRoot\..\..\TestMusicAppLocalDeploymentOrchestrator";
$MSBuild = & "$OrchestratorPath\Utility\Find-MSBuildPath.ps1";
$SolutionPath = [System.IO.Path]::GetFullPath("$PSScriptRoot\..\TestMusicAppServices.sln");
$ServicesProjectPath = [System.IO.Path]::GetFullPath("$PSScriptRoot\..\TestMusicAppServices\TestMusicAppServices.sfproj");
$PackagePath = [System.IO.Path]::GetFullPath("$PSScriptRoot\..\TestMusicAppServices\pkg\Debug");

$ClusterUrl = "localhost:19000";

Write-Host "Connecting to the local Service Fabric cluster at '$ClusterUrl'";
Connect-ServiceFabricCluster -ConnectionEndpoint $ClusterUrl | Out-Null;

$Node = Get-ServiceFabricNode;
$IsMultipleNodes = $Node -is [array];

If ($IsMultipleNodes) {
    $NodesCount = 5;
}
Else {
    $NodesCount = 1;
}

Write-Host "Decided to use publish profile for $NodesCount node(s)";

$PublishProfileFile = [System.IO.Path]::GetFullPath("$PSScriptRoot\..\TestMusicAppServices\PublishProfiles\Local.${NodesCount}Node.xml");

Write-Host "Building solution";
dotnet build $SolutionPath;

Write-Host "Packaging services";
& $MSBuild $ServicesProjectPath /t:Package /v:Minimal

Write-Host "Running deploy script";

$Global:ClusterConnection = $ClusterConnection;

Try {
    $DeployFabricApplicationParams = @{
        'ApplicationPackagePath' = $PackagePath;
        'PublishProfileFile' = $PublishProfileFile;
        'OverwriteBehavior' = 'Always';
        'UseExistingClusterConnection' = $True;
    };

    . "$PSScriptRoot\..\TestMusicAppServices\Scripts\Deploy-FabricApplication.ps1" @DeployFabricApplicationParams;
}
Finally {
    Remove-Variable ClusterConnection -Scope Global;
} 