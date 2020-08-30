# Windows-USB-DVD-Download-Tool (wudt)
This reposotory cloned from the official 
Windows USB/DVD Download Tool Codeplex [repository](https://archive.codeplex.com/?p=wudt) all issues also migrated

Changes over original package:

* Added Windows ISO download link
* Fixed home link #9
* Migrated solution to Visual Studio 2019
* Updated Wix to version 3.5
* Compiled via .net version 4.6.2 (make the tool work on the Windows 10 without .net 2.0 installation) #7
* Rename to Windows 7-8.1 USB/DVD download tool #3


wudt
Windows USB/DVD Download Tool

This project migrated to https://www.microsoft.com/en-us/download/details.aspx?id=56485
Windows USB/DVD Download Tool allow you to easily create bootable DVD or USB Keys for Windows 7/8 installation from the ISO file downloaded from Microsoft Store

homeissuesdiscussions
The Windows USB/DVD Download tool allows you to create a copy of your Windows 7/8 ISO file on a USB flash drive or a DVD. To create a bootable DVD or USB flash drive, download the ISO file and then run the Windows 7 USB/DVD Download tool. Once this is done, you can install Windows 7 or Windows 8 directly from the USB flash drive or DVD.

The ISO file contains all the Windows installation files combined into a single uncompressed file. When you download the ISO file, you need to copy it to some medium in order to install Windows. This tool allows you to create a copy of the ISO file to a USB flash drive or a DVD. To install Windows from your USB flash drive or DVD, all you need to do is insert the USB flash drive into your USB port or insert your DVD into your DVD drive and run Setup.exe from the root folder on the drive.

Note: You cannot install Windows from the ISO file until you copy it to a USB flash drive or DVD with the Windows 7 USB/DVD Download tool and install from there.

The copy of the ISO file that the Windows USB/DVD Download tool creates is bootable. Bootable media allows you to install Windows 7 without having to first run an existing operating system on your machine. If you change the boot order of drives in your computer's BIOS, you can run the Windows 7 installation directly from your USB flash drive or DVD when you turn on your computer. Please see the documentation for your computer for information on how to change the BIOS boot order of drives.

For Windows 7 Users

The following applications must be installed prior to installing the tool:

•Microsoft .NET Framework v4.6.2 must be installed. It can be downloaded at https://www.microsoft.com/en-us/download/details.aspx?id=53344 .
•Microsoft Image Mastering API v2 must be installed. It can be downloaded at http://www.microsoft.com/downloads/details.aspx?FamilyId=B5F726F1-4ACE-455D-BAD7-ABC4DD2F147B&displaylang=en.
