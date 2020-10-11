@echo off
call :echoInLine "START.." a
echo.
echo.
echo.

::Set directories
Set dirXML=CodeCoverageXMLReport
Set dirHTML=CodeCoverageHTMLReport

::Prepare directories

::Remove old test xml results for nonconfuse reportgenerator
If Exist %dirXML% (
	RMDIR %dirXML% /S /Q
	mkdir %dirXML% 
)

If NOT Exist %dirHTML% ( mkdir %dirHTML% )




::START Processing
dotnet restore
dotnet build

::run tests
dotnet test --collect "XPlat Code Coverage" --results-directory %dirXML%

:: generate/update a web reports
reportgenerator "-reports:%dirXML%\**\coverage.cobertura.xml" ^
	"-targetdir:%dirHTML%" ^
	-reporttypes:HTML

::END Processing





::Information formating for display
echo.
echo.
echo.
echo.
call :echoInLine "Results in a folder " b
call :echoInLine %dirHTML% e
echo.
call :echoInLine "(" 6
call :echoInLine %~dp0%dirHTML% 6
call :echoInLine ")" 6
echo.
echo.
echo.

SET launchFile=%dirHTML%\index.htm
If Exist %launchFile% (
	start %launchFile%
	call :echoInLine "Web review launched." a
	echo.
) else ( 
	call :echoInLine "Launch file for web review not found!" c
	echo.
)

pause






:: FUNCTIONS
 
:echoInLine
for /f %%i in ('"prompt $h& for %%i in (.) do rem"') do (set Z=%%i)
pushd "%TEMP%" && (
    <nul>"%~1^" set /p="%Z%%Z%  %Z%%Z%"
    findstr /a:%2 . "%~1^*"
    del "%~1^"
    popd
    )
exit /b


