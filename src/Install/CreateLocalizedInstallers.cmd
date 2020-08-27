@ECHO Off
SET Path=%WINDIR%\Microsoft.NET\Framework\v3.5;%PATH%

IF "%1" neq "" SET config=%1
IF "%config%"=="" SET /p config=Enter the configuration name: 

msbuild /property:Configuration=%config%;Culture=en-AU;Codepage=1252 /target:Clean;Build
msbuild /property:Configuration=%config%;Culture=en-GB;Codepage=1252 /target:Clean;Build
msbuild /property:Configuration=%config%;Culture=de-DE;Codepage=1252 /target:Clean;Build
msbuild /property:Configuration=%config%;Culture=en-US;Codepage=1252 /target:Clean;Build
msbuild /property:Configuration=%config%;Culture=es-ES;Codepage=1252 /target:Clean;Build
msbuild /property:Configuration=%config%;Culture=fr-FR;Codepage=1252 /target:Clean;Build
msbuild /property:Configuration=%config%;Culture=it-IT;Codepage=1252 /target:Clean;Build
msbuild /property:Configuration=%config%;Culture=ja-JP;Codepage=932 /target:Clean;Build
msbuild /property:Configuration=%config%;Culture=ko-KR;Codepage=1252 /target:Clean;Build
msbuild /property:Configuration=%config%;Culture=nl-NL;Codepage=1252 /target:Clean;Build

ECHO on