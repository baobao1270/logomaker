

:: this script needs https://www.nuget.org/packages/ilmerge

:: set your target executable name (typically [projectname].exe)
SET HOME_DIR=bin\Release\
SET APP_NAME=LogoMaker.exe
SET OUT_DEST=LogoMaker-Linked.exe

:: set your NuGet ILMerge Version, this is the number from the package manager install, for example:
:: PM> Install-Package ilmerge -Version 3.0.29
:: to confirm it is installed for a given project, see the packages.config file
SET ILMERGE_VERSION=3.0.29

:: the full ILMerge should be found here:
SET ILMERGE_PATH=%USERPROFILE%\.nuget\packages\ilmerge\%ILMERGE_VERSION%\tools\net452
:: dir "%ILMERGE_PATH%"\ILMerge.exe

echo Merging %APP_NAME% ...

:: add project DLL's starting with replacing the FirstLib with this project's DLL
"%ILMERGE_PATH%\ILMerge.exe" ^
  /lib:%HOME_DIR% ^
  /out:%HOME_DIR%%OUT_DEST% ^
  %HOME_DIR%%APP_NAME% ^
  %HOME_DIR%CommandLine.dll ^
  %HOME_DIR%Fizzler.dll ^
  %HOME_DIR%Svg.dll


:Done
dir %APP_NAME%