#pragma once
//-------------------------------------------------------------------------------------------------
// <copyright file="memutil.cpp" company="Microsoft">
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
//    Memory helper functions.
// </summary>
//-------------------------------------------------------------------------------------------------

#include "precomp.h"


#if DEBUG
static BOOL vfMemInitialized = FALSE;
#endif

extern "C" HRESULT DAPI MemInitialize()
{
#if DEBUG
    vfMemInitialized = TRUE;
#endif
    return S_OK;
}

extern "C" void DAPI MemUninitialize()
{
#if DEBUG
    vfMemInitialized = FALSE;
#endif
}

extern "C" LPVOID DAPI MemAlloc(
    __in SIZE_T cbSize,
    __in BOOL fZero
    )
{
    // AssertSz(vfMemInitialized, "MemInitialize() not called, this would normally crash");
    return ::HeapAlloc(::GetProcessHeap(), fZero ? HEAP_ZERO_MEMORY : 0, cbSize);
}


extern "C" LPVOID DAPI MemReAlloc(
    __in LPVOID pv,
    __in SIZE_T cbSize,
    __in BOOL fZero
    )
{
    // AssertSz(vfMemInitialized, "MemInitialize() not called, this would normally crash");
    return ::HeapReAlloc(::GetProcessHeap(), fZero ? HEAP_ZERO_MEMORY : 0, pv, cbSize);
}


extern "C" HRESULT DAPI MemFree(
    __in LPVOID pv
    )
{
    // AssertSz(vfMemInitialized, "MemInitialize() not called, this would normally crash");
    return ::HeapFree(::GetProcessHeap(), 0, pv) ? S_OK : HRESULT_FROM_WIN32(::GetLastError());
}


extern "C" SIZE_T DAPI MemSize(
    __in LPVOID pv
    )
{
    // AssertSz(vfMemInitialized, "MemInitialize() not called, this would normally crash");
    return ::HeapSize(::GetProcessHeap(), 0, pv);
}
