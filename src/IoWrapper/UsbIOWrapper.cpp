#pragma once
//-------------------------------------------------------------------------------------------------
// <copyright file="UsbIOWrapper.cpp" company="Microsoft">
//     Copyright (C) 2009 Microsoft Corporation.
//     This program is free software; you can redistribute it and/or modify 
//     it under the terms of the GNU General Public License version 2 as 
//     published by the Free Software Foundation.
// 
//     This program is distributed in the hope that it will be useful, but 
//     WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
//     or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License 
//     for more details.
// 
//     You should have received a copy of the GNU General Public License along 
//     with this program; if not, write to the Free Software Foundation, Inc., 
//     51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// </copyright>
// 
// <summary>
//    Utility for interacting with USB device.
// </summary>
//-------------------------------------------------------------------------------------------------

#include "precomp.h"
#include "UsbIOWrapper.h"

static BOOL m_fRunning;

extern "C" __declspec(dllexport) HRESULT SetActivePartition(LPWSTR selectedDrive) {
    HRESULT hr = S_OK;
    BOOL bSuccess = FALSE;
    DWORD dwBytesReturned = 0;

    HANDLE hDisk = OpenDiskHandle(selectedDrive);
    if (NULL == hDisk || INVALID_HANDLE_VALUE == hDisk) {
        hr = GetLastError();
        ExitOnFailure(hr, "Unable to get handle to disk.");
    }

    // Get the partition information
    BYTE Buffer[sizeof(DRIVE_LAYOUT_INFORMATION_EX) + 3 * sizeof(PARTITION_INFORMATION_EX)];
    bSuccess = DeviceIoControl (hDisk,
        IOCTL_DISK_GET_DRIVE_LAYOUT_EX,
        NULL,
        0,
        &Buffer,
        sizeof(Buffer),
        &dwBytesReturned,
        NULL);

    if (!bSuccess || dwBytesReturned != sizeof(Buffer)) {
        hr = GetLastError();
        ExitOnFailure(hr, "Unable to read partition information.");
    }
    
    // Set bootable flag and rewrite the partition if needed.
    DRIVE_LAYOUT_INFORMATION_EX*  pDriveLayout = (DRIVE_LAYOUT_INFORMATION_EX*)Buffer;
    if(!pDriveLayout->PartitionEntry[0].Mbr.BootIndicator) {
        pDriveLayout->PartitionEntry[0].Mbr.BootIndicator = TRUE;
        pDriveLayout->PartitionEntry[0].RewritePartition = TRUE;

        bSuccess = DeviceIoControl (hDisk,
            IOCTL_DISK_SET_DRIVE_LAYOUT_EX,
            pDriveLayout, 
            sizeof(DRIVE_LAYOUT_INFORMATION_EX) + 3 * sizeof(PARTITION_INFORMATION_EX),
            NULL,
            0,
            &dwBytesReturned,
            NULL);

        if (!bSuccess) {
            hr = GetLastError();
            ExitOnFailure(hr, "Unable to write partition information.");
        }
    }

LExit:
    if (NULL != hDisk && INVALID_HANDLE_VALUE != hDisk) 
    {
        CloseHandle(hDisk);
    }

    return hr;
}

extern "C" __declspec(dllexport) HRESULT FormatDrive(LPWSTR selectedDrive) {
    HRESULT hr = S_OK;

    int iInvokeCommand;
    UINT uExitCode;
    DWORD dwDurationFormat;
    DWORD dwDurationConvert;
    LPWSTR cmdPath = NULL;
    LPWSTR formatCmd = NULL;
    LPWSTR convertCmd = NULL;
    OSVERSIONINFO osvi;
    BOOL bUseConvert;

    dwDurationFormat = 300;
    dwDurationConvert = 300;

    // Detect OS version
    ZeroMemory(&osvi, sizeof(OSVERSIONINFO));
    osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);

    GetVersionEx(&osvi);

    bUseConvert = osvi.dwMajorVersion < 6;

    // Build path to cmd
    hr = StrAlloc(&cmdPath, MAX_PATH + 1);
    ExitOnFailure(hr, "Unable to allocate cmd path string.");
	GetEnvironmentVariable(L"WINDIR", cmdPath, MAX_PATH);

    hr = StrAllocConcat(&cmdPath, L"\\system32\\cmd.exe", 17);
    ExitOnFailure(hr, "Unable to concatenate cmd path string.");

    // Build format command
    hr = StrAllocConcat(&formatCmd, L"/C format ", 10);
    ExitOnFailure(hr, "Unable to allocate format command string.");
    
    hr = StrAllocConcat(&formatCmd, selectedDrive, 2);
    ExitOnFailure(hr, "Unable to append drive letter to format command string.");
   
    if (bUseConvert)
    {
        hr = StrAllocConcat(&formatCmd, L" /FS:FAT32 /V:\"\" /Q /X", 22);
        ExitOnFailure(hr, "Unable to append paramters to format command string.");

        // Build convert command
        hr = StrAllocConcat(&convertCmd, L"/C echo N | convert ", 20);
        ExitOnFailure(hr, "Unable to allocate convert command string.");
        
        hr = StrAllocConcat(&convertCmd, selectedDrive, 2);
        ExitOnFailure(hr, "Unable to append drive letter to convert command string.");
       
        hr = StrAllocConcat(&convertCmd, L" /FS:NTFS /X", 12);
        ExitOnFailure(hr, "Unable to append paramters to convert command string.");
    }
    else 
    {
        hr = StrAllocConcat(&formatCmd, L" /FS:NTFS /V:\"\" /Q /X", 21);
        ExitOnFailure(hr, "Unable to append paramters to format command string.");
    }

    iInvokeCommand = InvokeCommand(cmdPath, formatCmd, "Y\nN\n", 5, dwDurationFormat*1000L, 99, &uExitCode);
    if (iInvokeCommand != 0) 
    {
        printf("\n\nFORMAT  - Exit Code returned (%d)  Invoke Command returned (%d)  Duration was (%d)\n\n", uExitCode, iInvokeCommand, dwDurationFormat);
        return iInvokeCommand;
    }

    if (bUseConvert)
    {
        iInvokeCommand = InvokeCommand(cmdPath, convertCmd, NULL, 0, dwDurationConvert*1000L, 99, &uExitCode);
        if (iInvokeCommand != 0) 
        {
            printf("\n\nCONVERT - Exit Code returned (%d)  Invoke Command returned (%d)  Duration was (%d)\n\n", uExitCode, iInvokeCommand, dwDurationConvert);
            return iInvokeCommand;
        }
    }

LExit:
    ReleaseNullStr(cmdPath);

    return hr;
}

HANDLE OpenDiskHandle(LPWSTR rootPath) {
    HRESULT hr;
    HANDLE hDisk = INVALID_HANDLE_VALUE;
    LPWSTR drivePath = NULL;

    // CreateFile requires a file name in the format '\\.\X:' instead of 'X:\'
    hr = StrAllocString(&drivePath, L"\\\\.\\", 6);
    ExitOnFailure(hr, "Unable to allocate drive path string.");
    
    hr = StrAllocConcat(&drivePath, rootPath, 2);
    ExitOnFailure(hr, "Unable to concatenate drive path string.");

    // Open handle to disk
    hDisk =  CreateFile(drivePath, 
                        GENERIC_EXECUTE | GENERIC_READ | GENERIC_WRITE,
                        FILE_SHARE_READ | FILE_SHARE_WRITE,
                        NULL,
                        OPEN_EXISTING,
                        FILE_ATTRIBUTE_NORMAL,
                        NULL);

LExit:
    ReleaseNullStr(drivePath);

    if (hr != S_OK) {
        SetLastError(hr);
    }

    return hDisk;
}

int InvokeCommand(LPWSTR pwszCmdExe, LPWSTR pwszCommand, LPSTR pszAnswers, int cbAnswers, DWORD dwTimeout, UINT uDefaultExitCode, PUINT puExitCode)
{
    DWORD error = ERROR_INTERNAL_ERROR;
    DWORD uExitCode = uDefaultExitCode;
    SECURITY_ATTRIBUTES sa; 
    PROCESS_INFORMATION pi;
    STARTUPINFO si;
    HANDLE hStdInProcess, hStdOutProcess, hStdErrProcess;
    HANDLE hStdIn = INVALID_HANDLE_VALUE;
    HANDLE hStdOut = INVALID_HANDLE_VALUE;
    HANDLE hStdErr = INVALID_HANDLE_VALUE;

    sa.nLength = sizeof(sa);
    sa.bInheritHandle = TRUE;
    sa.lpSecurityDescriptor = NULL;

    ZeroMemory(&pi,sizeof(pi));
    ZeroMemory(&si,sizeof(si));
    si.cb = sizeof(si);

    if (!CreatePipe(&hStdInProcess, &hStdIn, &sa, 0))
    {
        error = GetLastError();
        printf("DuplicateHandle stdin failed (%d)\n", error);
        goto done;
    }

    if (!CreatePipe(&hStdOut, &hStdOutProcess, &sa, 0))
    {
        error = GetLastError();
        printf("DuplicateHandle stdout failed (%d)\n", error);
        goto done;
    }

    if (!CreatePipe(&hStdErr, &hStdErrProcess, &sa, 0))
    {
        error = GetLastError();
        printf("DuplicateHandle stderr failed (%d)\n", error);
        goto done;
    }

    si.dwFlags |= STARTF_USESTDHANDLES | STARTF_USESHOWWINDOW;
    si.hStdInput = hStdInProcess;
    si.hStdOutput = hStdOutProcess; 
    si.hStdError = hStdErrProcess; 
    si.wShowWindow = SW_HIDE;
    
    if(!CreateProcess(pwszCmdExe,pwszCommand,NULL,NULL,TRUE,0,NULL,NULL,&si,&pi)) 
    {
        error = GetLastError();
        printf("CreateProcess failed (%d)\n", error);
        goto done;
    }

    // Wait for a second for command to complete by itself before trying to answer any prompts.
    if (WAIT_TIMEOUT==WaitForSingleObject(pi.hProcess, 1000L) && cbAnswers>0)
    {
        DWORD dwWritten;
        if (cbAnswers>0)
        {
            printf("Entering '%s'.", pszAnswers);
            if (!WriteFile(hStdIn, pszAnswers, cbAnswers, &dwWritten, NULL))
            {
                DWORD error = GetLastError();
                printf("WriteFile failed (%d)\n", error);
                goto done;
            }
        }
    }

    // Wait for specified duration for command to complete.
    if (WAIT_TIMEOUT==WaitForSingleObject(pi.hProcess, dwTimeout))
    {
        if (!TerminateProcess(pi.hProcess, uDefaultExitCode))
        {
            DWORD error = GetLastError();
            printf("TerminateProcess failed (%d)\n", error);
            goto done;
        }
    }
    else
    {
        if (!GetExitCodeProcess(pi.hProcess, &uExitCode))
        {
            DWORD error = GetLastError();
            printf("GetExitCodeProcess failed (%d)\n", error);
            goto done;
        }
        else 
        {	
            error=0;
        }
        printf("Process exited with value (%d)\n", uExitCode);
    }

done:
    if (puExitCode)
    {
        *puExitCode=uExitCode;
    }

    if (INVALID_HANDLE_VALUE!=pi.hProcess && 0!=pi.hProcess)
    {
        CloseHandle(pi.hProcess);
    }

    if (INVALID_HANDLE_VALUE!=pi.hThread && 0!=pi.hThread)
    {
        CloseHandle(pi.hThread);
    }

    if (NULL != hStdIn && INVALID_HANDLE_VALUE != hStdIn) 
    {
        CloseHandle(hStdIn);
    }

    if (NULL != hStdInProcess && INVALID_HANDLE_VALUE != hStdInProcess) 
    {
        CloseHandle(hStdInProcess);
    }

    if (NULL != hStdOut && INVALID_HANDLE_VALUE != hStdOut) 
    {
        CloseHandle(hStdOut);
    }

    if (NULL != hStdOutProcess && INVALID_HANDLE_VALUE != hStdOutProcess) 
    {
        CloseHandle(hStdOutProcess);
    }

    if (NULL != hStdErr && INVALID_HANDLE_VALUE != hStdErr) 
    {
        CloseHandle(hStdErr);
    }

    if (NULL != hStdErrProcess && INVALID_HANDLE_VALUE != hStdErrProcess) 
    {
        CloseHandle(hStdErrProcess);
    }

    return error;
}
