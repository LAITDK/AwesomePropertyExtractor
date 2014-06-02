param($rootPath, $toolsPath, $package, $project)

if ($project) {	

#save the project file first - this commits the changes made by nuget before this     script runs.
$project.Save()

#Load the csproj file into an xml object
$xml = [XML] (gc $project.FullName)

#save the changes.
$xml.Save($project.FullName)
}