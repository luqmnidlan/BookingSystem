@echo off
echo Starting IIS deployment for Amm Barbershop Booking System...
echo.
echo This script will:
echo 1. Create an IIS Application Pool
echo 2. Create an IIS Website
echo 3. Set proper permissions
echo.
echo Make sure you are running this as Administrator!
echo.
pause

powershell -ExecutionPolicy Bypass -File "deploy-to-iis.ps1"

echo.
echo Deployment script completed.
echo Check the output above for any errors.
echo.
pause

