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

echo =====================================
echo Ejecutando todos los tests con cobertura
echo =====================================

call dotnet test "tests\Domain.UnitTests" --collect:"XPlat Code Coverage"
if %errorlevel% neq 0 (
    echo ❌ Error al ejecutar Domain.UnitTests
    exit /b 1
)

call dotnet test "tests\Application.UnitTests" --collect:"XPlat Code Coverage"
if %errorlevel% neq 0 (
    echo ❌ Error al ejecutar Application.UnitTests
    exit /b 1
)

call dotnet test "tests\Infrastructure.UnitTests" --collect:"XPlat Code Coverage"
if %errorlevel% neq 0 (
    echo ❌ Error al ejecutar Infrastructure.UnitTests
    exit /b 1
)

call dotnet test "tests\Application.Functional.UnitTests" --collect:"XPlat Code Coverage"
if %errorlevel% neq 0 (
    echo ❌ Error al ejecutar Application.Functional.UnitTests
    exit /b 1
)

echo =====================================
echo Buscando archivos coverage.cobertura.xml
echo =====================================

set "coverageFiles="

for /r %%f in (*coverage.cobertura.xml) do (
    echo Encontrado: %%f
    if defined coverageFiles (
        set "coverageFiles=!coverageFiles!;%%f"
    ) else (
        set "coverageFiles=%%f"
    )
)

if "!coverageFiles!"=="" (
    echo ❌ No se encontraron archivos coverage.cobertura.xml
    exit /b 1
)

echo Archivos de cobertura encontrados:
echo !coverageFiles!

echo =====================================
echo Generando reporte HTML combinado
echo =====================================

reportgenerator ^
    -reports:!coverageFiles! ^
    -targetdir:coverageReport ^
    -reporttypes:HtmlSummary;Html

if %errorlevel% neq 0 (
    echo ❌ Error al generar reporte HTML
    exit /b 1
)

echo =====================================
echo Reporte HTML generado correctamente
echo Abriendo coverageReport\index.html
echo =====================================

start "" "coverageReport\index.html"

exit /b 0
