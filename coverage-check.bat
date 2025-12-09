@echo off
setlocal enabledelayedexpansion

set threshold=80

echo ============================
echo Ejecutando tests con cobertura
echo ============================

dotnet test --collect:"XPlat Code Coverage"
if %errorlevel% neq 0 (
    echo ❌ Error al ejecutar tests
    exit /b 1
)

echo ============================
echo Buscando archivos generados...
echo ============================

set coverageFiles=

for /r %%f in (coverage.cobertura.xml) do (
    echo Encontrado: %%f
    set coverageFiles=!coverageFiles! "%%f"
)

if "%coverageFiles%"=="" (
    echo ❌ No se encontraron archivos coverage.cobertura.xml
    exit /b 1
)

echo Archivos detectados:
echo %coverageFiles%

echo ============================
echo Generando reporte combinado...
echo ============================

reportgenerator ^
    -reports:%coverageFiles% ^
    -targetdir:coverageReport ^
    -reporttypes:HtmlSummary;Html ^
    -assemblyfilters:+*

if %errorlevel% neq 0 (
    echo ❌ Error al generar reporte HTML
    exit /b 1
)

echo ============================
echo Leyendo cobertura total...
echo ============================

set summaryFile=coverageReport\Summary.html

if not exist "%summaryFile%" (
    echo ❌ No se pudo generar Summary.html
    exit /b 1
)

REM Extraer línea del total:
for /f "tokens=2 delims=<" %%A in ('findstr /i "<td>Total</td>" "%summaryFile%"') do (
    set line=%%A
)

REM Extraer porcentaje del HTML:
for /f "tokens=1 delims=>%" %%B in ("%line%") do (
    set percent=%%B
)

echo Cobertura total: %percent%%%

echo ============================
if %percent% GEQ %threshold% (
    echo ✔ OK — Cumple con el mínimo (%threshold%%)
) else (
    echo ❌ FAIL — Solo %percent%% (mínimo %threshold%%)
)

echo ============================
echo Abriendo reporte HTML...
echo ============================
start "" "coverageReport\index.html"

exit /b 0
