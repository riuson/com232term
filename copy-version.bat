@echo off
echo copy version info
if not exist %1 goto File1NotFound

xcopy %1 %2 /F /Y & goto END

:File1NotFound
echo %1 not found.
echo generating...
echo "empty" > %2
goto END

:END
echo Done.