﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MicrosoftStore.IsoTool.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MicrosoftStore.IsoTool.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap BeginBurningButton {
            get {
                object obj = ResourceManager.GetObject("BeginBurningButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap BeginCopyingButton {
            get {
                object obj = ResourceManager.GetObject("BeginCopyingButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap BrowseButton {
            get {
                object obj = ResourceManager.GetObject("BrowseButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap CancelButton {
            get {
                object obj = ResourceManager.GetObject("CancelButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ChangeUsbButton {
            get {
                object obj = ResourceManager.GetObject("ChangeUsbButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel Backup.
        /// </summary>
        internal static string ConfirmCancelCaption {
            get {
                return ResourceManager.GetString("ConfirmCancelCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You currently have a backup in progress.  Canceling the backup will leave your media in an invalid state.  Are you sure you wish to cancel?.
        /// </summary>
        internal static string ConfirmCancelMessage {
            get {
                return ResourceManager.GetString("ConfirmCancelMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap CopyButton {
            get {
                object obj = ResourceManager.GetObject("CopyButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Removable Disk.
        /// </summary>
        internal static string DefaultDriveLabel {
            get {
                return ResourceManager.GetString("DefaultDriveLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://github.com/stadub/wudt--Windows-USB-DVD-Download-Tool/blob/master/WindowsDownloadLink.md.
        /// </summary>
        internal static string DownloadLinkUrl {
            get {
                return ResourceManager.GetString("DownloadLinkUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} (estimated time remaining {1}).
        /// </summary>
        internal static string DvdBurningProgress {
            get {
                return ResourceManager.GetString("DvdBurningProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap DvdButton {
            get {
                object obj = ResourceManager.GetObject("DvdButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The disc does not have sufficient free space.  Please insert a blank DVD and try again..
        /// </summary>
        internal static string DvdFreeSpaceMessage {
            get {
                return ResourceManager.GetString("DvdFreeSpaceMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap DvdImage {
            get {
                object obj = ResourceManager.GetObject("DvdImage", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to locate a compatible DVD drive.  Please make sure your DVD drive is connected and capable of creating discs..
        /// </summary>
        internal static string DvdNoDrivesMessage {
            get {
                return ResourceManager.GetString("DvdNoDrivesMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no disc in the drive.  Please insert a blank disc and try again..
        /// </summary>
        internal static string DvdNoMediaMessage {
            get {
                return ResourceManager.GetString("DvdNoMediaMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The disc is not blank.  Please insert a blank DVD and try again..
        /// </summary>
        internal static string DvdNotBlankMessage {
            get {
                return ResourceManager.GetString("DvdNotBlankMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The DVD drive is still trying to read the disc.  Please wait a few moments and try again..
        /// </summary>
        internal static string DvdNotReadyMessage {
            get {
                return ResourceManager.GetString("DvdNotReadyMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ready to burn. Click &quot;Begin burning&quot; to create the backup DVD..
        /// </summary>
        internal static string DvdReadyMessage {
            get {
                return ResourceManager.GetString("DvdReadyMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap EraseUsbButton {
            get {
                object obj = ResourceManager.GetObject("EraseUsbButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://www.microsoft.com/en-us/download/windows-usb-dvd-download-tool.
        /// </summary>
        internal static string HelpLinkUrl {
            get {
                return ResourceManager.GetString("HelpLinkUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid ISO File.
        /// </summary>
        internal static string IsoInvalidCaption {
            get {
                return ResourceManager.GetString("IsoInvalidCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The selected file is not a valid ISO file.  Please select a valid ISO file and try again..
        /// </summary>
        internal static string IsoInvalidMessage {
            get {
                return ResourceManager.GetString("IsoInvalidMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ISO File Not Selected.
        /// </summary>
        internal static string IsoNotSelectedCaption {
            get {
                return ResourceManager.GetString("IsoNotSelectedCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please select an ISO file..
        /// </summary>
        internal static string IsoNotSelectedMessage {
            get {
                return ResourceManager.GetString("IsoNotSelectedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap MasterSprite {
            get {
                object obj = ResourceManager.GetObject("MasterSprite", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap NextButton {
            get {
                object obj = ResourceManager.GetObject("NextButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap OpenDvdButton {
            get {
                object obj = ResourceManager.GetObject("OpenDvdButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap OpenUsbButton {
            get {
                object obj = ResourceManager.GetObject("OpenUsbButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap RefreshButton {
            get {
                object obj = ResourceManager.GetObject("RefreshButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This application requires Administrator privileges.  Please restart the tool as an Administrator..
        /// </summary>
        internal static string RequireAdmin {
            get {
                return ResourceManager.GetString("RequireAdmin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap StartOverButton {
            get {
                object obj = ResourceManager.GetObject("StartOverButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Files copied successfully.  However, we were unable to run bootsect to make the USB device bootable.  If you need assistance with bootsect, please click the &quot;Online Help&quot; link above for more information..
        /// </summary>
        internal static string StatusBootloaderError {
            get {
                return ResourceManager.GetString("StatusBootloaderError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Burning disc....
        /// </summary>
        internal static string StatusBurning {
            get {
                return ResourceManager.GetString("StatusBurning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Backup canceled.  Click “Start over” to try again..
        /// </summary>
        internal static string StatusCanceled {
            get {
                return ResourceManager.GetString("StatusCanceled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Canceling....
        /// </summary>
        internal static string StatusCanceling {
            get {
                return ResourceManager.GetString("StatusCanceling", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Backup completed..
        /// </summary>
        internal static string StatusComplete {
            get {
                return ResourceManager.GetString("StatusComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Copying files....
        /// </summary>
        internal static string StatusCopying {
            get {
                return ResourceManager.GetString("StatusCopying", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The burning process failed.  Please check your DVD drive and the selected ISO file and try again..
        /// </summary>
        internal static string StatusDvdError {
            get {
                return ResourceManager.GetString("StatusDvdError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Formatting....
        /// </summary>
        internal static string StatusFormatting {
            get {
                return ResourceManager.GetString("StatusFormatting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We were unable to copy your files.  Please check your USB device and the selected ISO file and try again..
        /// </summary>
        internal static string StatusUsbError {
            get {
                return ResourceManager.GetString("StatusUsbError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TermsOfUse.rtf.
        /// </summary>
        internal static string TermsOfUseFileName {
            get {
                return ResourceManager.GetString("TermsOfUseFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Step 3 of 4: Insert blank DVD.
        /// </summary>
        internal static string TitleDvd {
            get {
                return ResourceManager.GetString("TitleDvd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Step 4 of 4: Creating bootable DVD.
        /// </summary>
        internal static string TitleDvdProgress {
            get {
                return ResourceManager.GetString("TitleDvdProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bootable DVD created successfully.
        /// </summary>
        internal static string TitleDvdProgressComplete {
            get {
                return ResourceManager.GetString("TitleDvdProgressComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Step 1 of 4: Choose ISO file.
        /// </summary>
        internal static string TitleMain {
            get {
                return ResourceManager.GetString("TitleMain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Step 2 of 4: Choose media type.
        /// </summary>
        internal static string TitleMediaType {
            get {
                return ResourceManager.GetString("TitleMediaType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Step 3 of 4: Insert USB device.
        /// </summary>
        internal static string TitleUsb {
            get {
                return ResourceManager.GetString("TitleUsb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Step 4 of 4: Creating bootable USB device.
        /// </summary>
        internal static string TitleUsbProgress {
            get {
                return ResourceManager.GetString("TitleUsbProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bootable USB device created successfully.
        /// </summary>
        internal static string TitleUsbProgressComplete {
            get {
                return ResourceManager.GetString("TitleUsbProgressComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Microsoft ISO Backup Tool.
        /// </summary>
        internal static string ToolTitle {
            get {
                return ResourceManager.GetString("ToolTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap TryAgainButton {
            get {
                object obj = ResourceManager.GetObject("TryAgainButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected error occurred.  If the problem continues please contact support..
        /// </summary>
        internal static string UnhandledException {
            get {
                return ResourceManager.GetString("UnhandledException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error in Application.
        /// </summary>
        internal static string UnhandledExceptionCaption {
            get {
                return ResourceManager.GetString("UnhandledExceptionCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap UsbButton {
            get {
                object obj = ResourceManager.GetObject("UsbButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If your device is not displayed click &quot;Refresh.&quot;.
        /// </summary>
        internal static string UsbDefaultMessage {
            get {
                return ResourceManager.GetString("UsbDefaultMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The selected USB device {0} could not be found.  Please verify the drive is properly connected and click “Refresh.”.
        /// </summary>
        internal static string UsbDeviceNotFoundMessage {
            get {
                return ResourceManager.GetString("UsbDeviceNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} - {1} Free.
        /// </summary>
        internal static string UsbDriveFormat {
            get {
                return ResourceManager.GetString("UsbDriveFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have selected to erase all of the contents from the selected USB device {0}.  All contents on this device will be lost.  Are you sure you want to do this?.
        /// </summary>
        internal static string UsbEraseConfirm {
            get {
                return ResourceManager.GetString("UsbEraseConfirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The selected USB device {0} must be erased in order to continue.  Do you want to erase all contents from this device?.
        /// </summary>
        internal static string UsbEraseMessage {
            get {
                return ResourceManager.GetString("UsbEraseMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        internal static string UsbFreeSpaceCancel {
            get {
                return ResourceManager.GetString("UsbFreeSpaceCancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not Enough Free Space.
        /// </summary>
        internal static string UsbFreeSpaceCaption {
            get {
                return ResourceManager.GetString("UsbFreeSpaceCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Erase USB Device.
        /// </summary>
        internal static string UsbFreeSpaceErase {
            get {
                return ResourceManager.GetString("UsbFreeSpaceErase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The selected USB device {0} does not have enough free space.  To continue, the contents of the device must be erased.  Would you like to erase all contents from the selected device?.
        /// </summary>
        internal static string UsbFreeSpaceMessage {
            get {
                return ResourceManager.GetString("UsbFreeSpaceMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap UsbImage {
            get {
                object obj = ResourceManager.GetObject("UsbImage", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The selected USB device {0} is in use by another program.  Please close all applications and try again..
        /// </summary>
        internal static string UsbInUseMessage {
            get {
                return ResourceManager.GetString("UsbInUseMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The selected USB device is not compatible with this tool.  Please select another USB device and try again..
        /// </summary>
        internal static string UsbInvalidMessage {
            get {
                return ResourceManager.GetString("UsbInvalidMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No compatible USB devices detected.
        /// </summary>
        internal static string UsbNoDrivesDropDown {
            get {
                return ResourceManager.GetString("UsbNoDrivesDropDown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No compatible removable USB devices were detected.  Please connect a USB device that is at least 4GB and click “Refresh.”.
        /// </summary>
        internal static string UsbNoDrivesMessage {
            get {
                return ResourceManager.GetString("UsbNoDrivesMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        internal static string UsbOverwriteCancel {
            get {
                return ResourceManager.GetString("UsbOverwriteCancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Files Already Exist.
        /// </summary>
        internal static string UsbOverwriteCaption {
            get {
                return ResourceManager.GetString("UsbOverwriteCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Overwrite Files.
        /// </summary>
        internal static string UsbOverwriteConfirm {
            get {
                return ResourceManager.GetString("UsbOverwriteConfirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Files of the same name already exist on {0}.  Would you like to overwrite these files?.
        /// </summary>
        internal static string UsbOverwriteMessage {
            get {
                return ResourceManager.GetString("UsbOverwriteMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The selected USB device {0} is not large enough.  Please select a USB device that is at least 4GB and try again..
        /// </summary>
        internal static string UsbSizeMessage {
            get {
                return ResourceManager.GetString("UsbSizeMessage", resourceCulture);
            }
        }
    }
}
