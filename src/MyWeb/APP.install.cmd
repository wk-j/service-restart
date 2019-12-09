rem SET QUSER=BCircle.MyWeb.exe
SET QNAME=BCircle.MyWeb.exe
SET QPASSWORD=
SET QAPP=MyWeb.exe
SET QPATH=%cd%
SET QHOST=%COMPUTERNAME%

rem net user %QUSER% %QPASSWORD% /add
rem icacls "%QPATH%" /grant %QUSER%:(OI)(CI)WRX /t
sc create %QNAME% binPath= "%QPATH%\%QAPP% --urls=http://*:9000" obj= ".\LocalSystem" password= "%QPASSWORD%"
sc start %QNAME%