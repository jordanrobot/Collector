param (
	[Parameter()]
	[string] $Watch,
	[string] $Out,
    [string] $Filter
)

#USER SET PARAMETERS ARE NOT PASSING THROUGH TO THE COLLECTOR SCRIPT YET!
#TODO: FIX

	#Parameter Defaults
#======================================
	#sub-directory to watch...
    $subdirectoryWatchDefault = "src"
    #name of the file to build...
	$outputFilePreset = "build\build.vb"
	#File Search Filter
	$FilterDefault = "*.vb"
#======================================

# Set up parameters if there are any...
#======================================
#Set Directory to Watch...
if (-not $Watch) {
	$directoryToWatch = (Join-Path $PSScriptRoot $subdirectoryWatchDefault)

} else {

	if ([System.IO.Path]::IsPathRooted($Watch)) {
		$directoryToWatch = $Watch
	} else {
		$directoryToWatch = (Join-Path $PSScriptRoot $Watch)
	}
}

if (-not (test-path -Path $directoryToWatch)) {
	Write-Host "The directory $directoryToWatch does not exist.  Exiting with extreme prejudice."
	Write-Host ""
	exit
}

#Set output filename...
if (-not $Out) {
	$outputFile = (Join-Path $PSScriptRoot $outputFilePreset)

} else {

	if ([System.IO.Path]::IsPathRooted($Out)) {
		$outputFile = $Out
	} else {
		$outputFile = (Join-Path $PSScriptRoot $Out)
	}
}

#Set filter...
if (-not $Filter) {
	$Filter = $FilterDefault
}

# Main routine...
#======================================
$FileSystemWatcher = New-Object System.IO.FileSystemWatcher
$FileSystemWatcher.Path  = $directoryToWatch
$FileSystemWatcher.IncludeSubdirectories = $true
$FileSystemWatcher.EnableRaisingEvents = $true
$FileSystemWatcher.Filter = $Filter

# execute the following when a file change event is triggered
$Action = {
    $details = $event.SourceEventArgs
    $FullPath = $details.FullPath
    $ChangeType = $details.ChangeType
    $Timestamp = $event.TimeGenerated
    $text = "{0} detected in {1} at {2}" -f $ChangeType, $FullPath, $Timestamp

    Write-Host ""
    Write-Host $details.
    Write-Host $text -ForegroundColor Green

    & "$PSScriptRoot\collector.ps1"# -Watch "$directoryToWatch" -Filter "$Filter" -Out "$Out"
}

# add event handlers
$handlers = . {
    Register-ObjectEvent -InputObject $FileSystemWatcher -EventName Changed -Action $Action -SourceIdentifier FSChange
    Register-ObjectEvent -InputObject $FileSystemWatcher -EventName Created -Action $Action -SourceIdentifier FSCreate
    Register-ObjectEvent -InputObject $FileSystemWatcher -EventName Deleted -Action $Action -SourceIdentifier FSDelete
    Register-ObjectEvent -InputObject $FileSystemWatcher -EventName Renamed -Action $Action -SourceIdentifier FSRename
}

Write-Host "Watching for changes to $directoryToWatch"

try
{
    do
    {
        Wait-Event -Timeout 1
        Write-Host "." -NoNewline
        
    } while ($true)
}
finally
{
    # CTRL+C from user...
    # remove the event handlers
    Unregister-Event -SourceIdentifier FSChange
    Unregister-Event -SourceIdentifier FSCreate
    Unregister-Event -SourceIdentifier FSDelete
    Unregister-Event -SourceIdentifier FSRename
    # remove background jobs
    $handlers | Remove-Job
    # remove filesystemwatcher
    $FileSystemWatcher.EnableRaisingEvents = $false
    $FileSystemWatcher.Dispose()
    "Event Handler disabled."
}


