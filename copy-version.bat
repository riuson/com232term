@echo off
echo copy version info
set source=%1version-included.txt
set destination=%2version-included.txt

if not exist %source% goto FileNotFound
xcopy %source% %2 /D /Y & goto END

:FileNotFound
echo %source% not found.
echo generating...
echo "empty" > %destination%
goto END

:END
echo Done.