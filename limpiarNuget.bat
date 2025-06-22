@echo off
echo Limpiando bin y obj...
for /d /r %%i in (bin,obj) do if exist "%%i" rd /s /q "%%i"

echo Borrando caché local de NuGet...
dotnet nuget locals all --clear

echo Restaurando paquetes...
dotnet restore

echo Limpieza y restauración completadas.
pause
