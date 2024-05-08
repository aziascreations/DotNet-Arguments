# Building

## Setting up .NET

### Using Microsoft's installers
Go to 'https://dotnet.microsoft.com/en-us/download/dotnet' and select the appropriate .NET 6.0 SDK, or a newer version.

Once downloaded, simply install the SDK.

### Using Visual Studio Installer
Modify your installation and add the individual components named `.NET SDK` and `.NET 6.0 Runtime`.

### Using a packet manager
Please refer to your distro's packet manager and repository for more information


## Downloading the sources
Before compiling, you need to download the source code with git like so:
```shell
git clone --recurse-submodules https://github.com/aziascreations/DotNet-Arguments
```

Finally, once the repository is cloned, you need to enter the project's directory with:
```shell
cd DotNet-Arguments
```

The `--recurse-submodules` is only required if you intend to build the documentation with the optional
[doxygen-awesome-css](https://github.com/jothepro/doxygen-awesome-css) theme.

If you forgot to clone the submodules, you can use the following command to pull them:
```shell
git submodule update --init
```

## Preparing the project
In order to prepare the project for any future task, you need to use the following command:
```shell
dotnet restore
```

## Testing
In order to run the NUnit tests, you can use the following command:
```shell
dotnet test
```

## Compiling
In order to build the project, you should use the following command:
```shell
dotnet build
```

## Packaging
In order to build the NuGet package, you should use the following command:
```shell
dotnet pack
```
Afterward, check the '[NibblePoker.Library.Arguments/bin/Release](NibblePoker.Library.Arguments/bin/Release)' folder.


## Building the documentation

### Installing Doxygen
In order to install doxygen, you should follow the instructions given on their
[website](https://www.doxygen.nl/index.html).

If you already have it installed, make sure you have the `1.10.0` version, or a newer one by using the
following command:
```shell
doxygen -v
```

### Building manually
Run the following commands to build the documentation locally without using any script:

#### Windows
```shell
cd NibblePoker.Library.Arguments.Documentation
rmdir html /s /q
doxygen
```

#### Linux
```shell
cd NibblePoker.Library.Arguments.Documentation
rm -rf html
doxygen
```


### Building automatically
Run the following command to build the documentation locally with the help of a small script:

#### Windows
```shell
cd NibblePoker.Library.Arguments.Documentation
.\build.cmd
```

#### Linux
*No script is provided for linux yet.*


### Checking the documentation
Once compiled, the documentation can be read by opening the
'[NibblePoker.Library.Arguments.Documentation/html/index.html](NibblePoker.Library.Arguments.Documentation/html/index.html])'
file.
