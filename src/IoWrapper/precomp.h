#pragma once
//-------------------------------------------------------------------------------------------------
// <copyright file="precomp.h" company="Microsoft">
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
//    dutil precompiled header.
// </summary>
//-------------------------------------------------------------------------------------------------

#ifndef _WIN32_WINNT
#define _WIN32_WINNT 0x0500
#endif

#include <tchar.h>
#include <windows.h>
#include <windowsx.h>
#include <intsafe.h>
#include <strsafe.h>
#include <wininet.h>
#include <msi.h>
#include <psapi.h>
#include <gdiplus.h>
#include <shlobj.h>

#include "dutil.h"
#include "memutil.h"  // NOTE: almost everying is inlined so there is a small .cpp file
#include "strutil.h"
