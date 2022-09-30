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
```bash
sudo apt-get update
sudo apt-get install -y dotnet6
```

## Downloading the sources
Before compiling, you need to download the source code with git like so:
```bash
git clone https://github.com/aziascreations/DotNet-Arguments
```

Finally, once the repository is cloned, you need to enter the project's directory with:
```bash
cd DotNet-Arguments
````

## Preparing the project
In order to prepare the project for any future task, you need to use the following command:
```batch
dotnet restore
```

## Testing
In order to run the tests, you can use the following command:
```bash
dotnet test
```

## Compiling
In order to build the project, you should use the following command:
```bash
dotnet build
```

## Packaging
The nuget package is automatically built when [compiling](#compiling).

See the '[NibblePoker.Library.Arguments/bin/Release](NibblePoker.Library.Arguments/bin/Release)' folder.
