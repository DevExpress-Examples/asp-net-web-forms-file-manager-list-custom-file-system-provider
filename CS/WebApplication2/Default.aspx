<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Default" %>

<%@ Register Namespace="DevExpress.Web" TagPrefix="dx" Assembly="DevExpress.Web.v22.2, Version=22.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <dx:ASPxFileManager ID="fileManager" runat="server" OnFolderCreating="fileManager_FolderCreating"
            OnItemDeleting="fileManager_ItemDeleting" OnItemMoving="fileManager_ItemMoving"
            OnItemRenaming="fileManager_ItemRenaming" OnFileUploading="fileManager_FileUploading" CustomFileSystemProviderTypeName="FileStreamProvider">
            <SettingsDataSource KeyFieldName="Id" ParentKeyFieldName="Pid" NameFieldName="Name" IsFolderFieldName="IsFolder" FileBinaryContentFieldName="Data" />
            <SettingsEditing AllowCreate="True" AllowDelete="True" AllowMove="True" AllowRename="True" />
            <Settings RootFolder="Available Files"/>
        </dx:ASPxFileManager>
    </form>
</body>
</html>

