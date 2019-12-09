rem SET QUSER=BCircle.MyWeb.exe
SET QNAME=BCircle.MyWeb.exe

sc stop   %QNAME%
sc delete %QNAME%
rem net user  %QUSER% /delete