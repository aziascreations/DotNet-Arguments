# Building

## Setting up .NET

### Using Microsoft's installers
Navigate to ['dotnet.microsoft.com/en-us/download/dotnet'](https://dotnet.microsoft.com/en-us/download/dotnet) and select .NET 6.0 or a newer version.

Download and install the SDK that is compatible with your OS and CPU on the left side of the page.

### Using Visual Studio Installer
Modify your installation and install the individual component named `.NET SDK`.

### Using a packet manager

#### Ubuntu
Use the following commands:
```shell
sudo apt-get update
sudo apt-get install -y dotnet6
```

## Downloading the sources
Before compiling, you need to download the source code with git like so:
```shell
git clone https://github.com/aziascreations/DotNet-Arguments
```

Finally, once the repository is cloned, you need to enter the project's directory with:
```shell
cd DotNet-Arguments
````

## Preparing the project
In order to prepare the project for any future task, you need to use the following command:
```shell
dotnet restore
```

## Testing
In order to run the tests, you can use the following command:
```shell
dotnet test
```

## Compiling
In order to build the project, you should use the following command:
```shell
dotnet build
```

## Packaging
The nuget package is automatically built when [compiling](#compiling).

See the '[NibblePoker.Library.Arguments/bin/Release](NibblePoker.Library.Arguments/bin/Release)' folder.

## Building the documentation

### Installing DocFX
Run the following command to install DocFX:
```shell
dotnet tool install -g docfx
dotnet tool update -g docfx
```
**This tool may take a couple of minutes to install, just go grab a coffee in the meantime.**

### Running DocFX
Run the following command to build the documentation locally via the included script:
```shell
cd NibblePoker.Library.Arguments.Documentation
.\serve.cmd
```
