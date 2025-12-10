@echo off
setlocal enabledelayedexpansion

echo =====================================
echo Limpiando archivos de cobertura antiguos y TestResults
echo =====================================

REM Eliminar todos los coverage.cobertura.xml existentes
for /r %%f in (*coverage.cobertura.xml) do (
    echo Eliminando: %%f
    del /q "%%f"
)

REM Eliminar carpetas TestResults viejas
for /d /r %%d in (TestResults) do (
    echo Eliminando carpeta: %%d
    rmdir /s /q "%%d"
)

REM Eliminar reporte HTML previo
if exist coverageReport (
    echo Eliminando carpeta coverageReport
    rmdir /s /q coverageReport
)


exit /b 0
