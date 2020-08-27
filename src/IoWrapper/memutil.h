#pragma once
//-------------------------------------------------------------------------------------------------
// <copyright file="memutil.h" company="Microsoft">
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
//    Header for memory helper functions.
// </summary>
//-------------------------------------------------------------------------------------------------

#ifdef __cplusplus
extern "C" {
#endif

#define ReleaseMem(p) if (p) { MemFree(p); }
#define ReleaseNullMem(p) if (p) { MemFree(p); p = NULL; }


HRESULT DAPI MemInitialize();
void DAPI MemUninitialize();

LPVOID DAPI MemAlloc(
    __in SIZE_T cbSize,
    __in BOOL fZero
    );
LPVOID DAPI MemReAlloc(
    __in LPVOID pv,
    __in SIZE_T cbSize,
    __in BOOL fZero
    );

HRESULT DAPI MemFree(
    __in LPVOID pv
    );
SIZE_T DAPI MemSize(
    __in LPVOID pv
    );

#ifdef __cplusplus
}
#endif

