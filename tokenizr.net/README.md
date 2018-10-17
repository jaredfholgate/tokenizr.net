As well as being the basic client, this project also acts as the Nuget package. It contains meta data for Nuget in the project file. There are some rules to follow for this to work and for it to include dependencies.

1. The project references need to include 'PrivateAssets="all"'. This will ensure the assemblies are included in the nupkg file.
2. Nuget packages referenced in dependent projects need to be included as references for this project too. This will ensure they are dependencies in the nupkg file.