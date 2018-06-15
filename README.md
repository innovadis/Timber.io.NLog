# Timber.io.NLog
[![NuGet](https://img.shields.io/nuget/v/Timber.io.NLog.svg)](https://www.nuget.org/packages/Timber.io.NLog/)

Timber.io.NLog is a NLog target to push your logs to [Timber](https://timber.io).

# Usage
## Generate API key
To get an API key, just create a new app. Under *Language Type* and *Platform Type*, select _Other_. You will now get a page that contains your API key.

## Install package
Run the following command from the Package Manager Console: `PM> Install-Package Timber.io.NLog`

## Configure a target in NLog.config
```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" 
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

    <extensions>
        <!-- Add the Timber.io.NLog assembly to your configuration -->
        <add assembly="Timber.io.NLog" />
    </extensions>

    <targets>
        <!-- Define a new target -->
        <target name="timber" type="Timber.io" token="[your API token]" />
    </targets>
    <rules>
        <!-- Define the logging rules for your Timber.io.NLog-target -->
        <logger name="*" writeTo="timber" minlevel="Info" />
    </rules>
</nlog>
```
