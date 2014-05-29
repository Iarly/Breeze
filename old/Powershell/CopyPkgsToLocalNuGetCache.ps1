<<<<<<< HEAD:Nuget.builds/CopyPkgsToLocalNuGetCache.ps1
# srcDir is the location of this script file
$srcDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent

copy-item $srcDir\Breeze.WebApi\Breeze.WebApi.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.WebApi.Sample\Breeze.WebApi.Sample.*.nupkg $env:LOCALAPPDATA\Nuget\Cache

copy-item $srcDir\Breeze.Client\Breeze.Client.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.Server.WebApi.Core\Breeze.Server.WebApi.Core.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.Server.WebApi.EF\Breeze.Server.WebApi.EF.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.Server.WebApi.NH\Breeze.Server.WebApi.NH.*.nupkg $env:LOCALAPPDATA\Nuget\Cache

copy-item $srcDir\Breeze.Server.ContextProvider\Breeze.Server.ContextProvider.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.Server.ContextProvider.EF6\Breeze.Server.ContextProvider.EF6.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.Server.WebApi2\Breeze.Server.WebApi2.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.WebApi2.EF6\Breeze.WebApi2.EF6.*.nupkg $env:LOCALAPPDATA\Nuget\Cache

=======
# srcDir is the location of this script file
$srcDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent

copy-item $srcDir\Breeze.WebApi\Breeze.WebApi.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.WebApi.Sample\Breeze.WebApi.Sample.*.nupkg $env:LOCALAPPDATA\Nuget\Cache

copy-item $srcDir\Breeze.Client\Breeze.Client.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.Server.WebApi.Core\Breeze.Server.WebApi.Core.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.Server.WebApi.EF\Breeze.Server.WebApi.EF.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.Server.WebApi.NH\Breeze.Server.WebApi.NH.*.nupkg $env:LOCALAPPDATA\Nuget\Cache

copy-item $srcDir\Breeze.Server.ContextProvider\Breeze.Server.ContextProvider.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.Server.ContextProvider.EF6\Breeze.Server.ContextProvider.EF6.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.Server.WebApi2\Breeze.Server.WebApi2.*.nupkg $env:LOCALAPPDATA\Nuget\Cache
copy-item $srcDir\Breeze.WebApi2.EF6\Breeze.WebApi2.EF6.*.nupkg $env:LOCALAPPDATA\Nuget\Cache

# Breeze Labs. Build by hand.
copy-item $srcDir\Breeze.Angular.Directives\Breeze.Angular.Directives.*.nupkg $env:LOCALAPPDATA\Nuget\Cache

>>>>>>> 9fe7264a8ea74100b2ba57719bc9e8f615dc7393:old/Powershell/CopyPkgsToLocalNuGetCache.ps1
