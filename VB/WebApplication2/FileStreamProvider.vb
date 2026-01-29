Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports DevExpress.Web
Imports System.IO
Imports DevExpress.Web.Internal
Imports System.Reflection
Imports WebApplication2

Public Class FileStreamProvider
    Inherits DevExpress.Web.FileSystemProviderBase

    Private root As WebApplication2.FileSystemData = New WebApplication2.FileSystemData()

    Private ReadOnly Property DataSource As List(Of WebApplication2.FileSystemData)
        Get
            Return CType(System.Web.HttpContext.Current.Session("DataSource"), System.Collections.Generic.List(Of WebApplication2.FileSystemData))
        End Get
    End Property

    Public Sub New(ByVal rootFolder As String)
        MyBase.New(rootFolder)
    End Sub

    Public Overrides ReadOnly Property RootFolderDisplayName As String
        Get
            Return Me.GetRootFolder().Name
        End Get
    End Property

    Public Overrides Sub CreateFolder(ByVal parent As DevExpress.Web.FileManagerFolder, ByVal name As String)
        Me.DataSource.Add(New WebApplication2.FileSystemData() With {.Id = Me.GetHashCode(), .IsFolder = True, .LastWriteTime = System.DateTime.Now, .Name = name, .ParentId = Me.FindFolderItem(CType((parent), DevExpress.Web.FileManagerFolder)).Id})
    End Sub

    Public Overrides Sub DeleteFile(ByVal file As DevExpress.Web.FileManagerFile)
        Dim item As WebApplication2.FileSystemData = Me.FindFileItem(file)
        Me.DataSource.Remove(item)
    End Sub

    Public Overrides Sub DeleteFolder(ByVal folder As DevExpress.Web.FileManagerFolder)
        Dim item As WebApplication2.FileSystemData = Me.FindFolderItem(folder)
        Me.DataSource.Remove(item)
    End Sub

    Public Overrides Sub MoveFile(ByVal file As DevExpress.Web.FileManagerFile, ByVal newParentFolder As DevExpress.Web.FileManagerFolder)
        Dim item As WebApplication2.FileSystemData = Me.FindFileItem(file)
        item.ParentId = Me.FindFolderItem(CType((newParentFolder), DevExpress.Web.FileManagerFolder)).Id
    End Sub

    Public Overrides Sub MoveFolder(ByVal folder As DevExpress.Web.FileManagerFolder, ByVal newParentFolder As DevExpress.Web.FileManagerFolder)
        Dim item As WebApplication2.FileSystemData = Me.FindFolderItem(folder)
        item.ParentId = Me.FindFolderItem(CType((newParentFolder), DevExpress.Web.FileManagerFolder)).Id
    End Sub

    Public Overrides Sub RenameFile(ByVal file As DevExpress.Web.FileManagerFile, ByVal name As String)
        Dim item As WebApplication2.FileSystemData = Me.FindFileItem(file)
        item.Name = name
    End Sub

    Public Overrides Sub RenameFolder(ByVal folder As DevExpress.Web.FileManagerFolder, ByVal name As String)
        Dim item As WebApplication2.FileSystemData = Me.FindFolderItem(folder)
        item.Name = name
    End Sub

    Public Overrides Sub UploadFile(ByVal folder As DevExpress.Web.FileManagerFolder, ByVal fileName As String, ByVal content As System.IO.Stream)
        Me.DataSource.Add(New WebApplication2.FileSystemData() With {.Id = Me.GetHashCode(), .IsFolder = False, .LastWriteTime = System.DateTime.Now, .Name = fileName, .ParentId = Me.FindFolderItem(CType((folder), DevExpress.Web.FileManagerFolder)).Id})
    End Sub

    Public Overrides Function GetFolders(ByVal parentFolder As DevExpress.Web.FileManagerFolder) As IEnumerable(Of DevExpress.Web.FileManagerFolder)
        Dim dbFolderItem As WebApplication2.FileSystemData = Me.FindFolderItem(parentFolder)
        Return From dbItem In Me.DataSource Where dbItem.IsFolder AndAlso dbItem.ParentId = dbFolderItem.Id Select New DevExpress.Web.FileManagerFolder(Me, parentFolder, dbItem.Name)
    End Function

    Public Overrides Function GetFiles(ByVal folder As DevExpress.Web.FileManagerFolder) As IEnumerable(Of DevExpress.Web.FileManagerFile)
        Dim folderItem As WebApplication2.FileSystemData = Me.FindFolderItem(folder)
        Return From dbItem In Me.DataSource Where Not dbItem.IsFolder AndAlso dbItem.ParentId = folderItem.Id Select New DevExpress.Web.FileManagerFile(Me, folder, dbItem.Name)
    End Function

    Public Overrides Function Exists(ByVal file As DevExpress.Web.FileManagerFile) As Boolean
        Return Me.FindFileItem(file) IsNot Nothing
    End Function

    Public Overrides Function Exists(ByVal folder As DevExpress.Web.FileManagerFolder) As Boolean
        Return Me.FindFolderItem(folder) IsNot Nothing
    End Function

    Public Overrides Function ReadFile(ByVal file As DevExpress.Web.FileManagerFile) As System.IO.Stream
        Return New System.IO.MemoryStream(Me.FindFileItem(CType((file), DevExpress.Web.FileManagerFile)).Data.ToArray())
    End Function

    Public Overrides Function GetLastWriteTime(ByVal file As DevExpress.Web.FileManagerFile) As DateTime
        Dim dbFileItem = Me.FindFileItem(file)
        Return dbFileItem.LastWriteTime.GetValueOrDefault(System.DateTime.Now)
    End Function

    Private Function GetRootFolder() As FileSystemData
        Return Me.DataSource.Where(Function(x) x.IsFolder AndAlso x.ParentId Is Nothing).FirstOrDefault()
    End Function

    Protected Function FindFolderItem(ByVal folder As DevExpress.Web.FileManagerFolder) As FileSystemData
        Dim folders = Me.DataSource.Where(Function(x) x.IsFolder)
        Return(From item In folders Where item.IsFolder AndAlso Equals(Me.GetRelativeName(item), folder.RelativeName) Select item).FirstOrDefault()
    End Function

    Protected Function GetRelativeName(ByVal folder As WebApplication2.FileSystemData) As String
        Dim root As WebApplication2.FileSystemData = Me.GetRootFolder()
        If folder.Id = root.Id Then Return String.Empty
        If folder.ParentId = root.Id Then Return folder.Name
        Dim folders = Me.DataSource.Where(Function(x) x.IsFolder)
        Dim name As String = Me.GetRelativeName(folders.Where(Function(x) x.Id = folder.ParentId).FirstOrDefault())
        Return If(Equals(name, Nothing), Nothing, System.IO.Path.Combine(name, folder.Name))
    End Function

    Protected Function FindFileItem(ByVal file As DevExpress.Web.FileManagerFile) As FileSystemData
        Dim folderItem As WebApplication2.FileSystemData = Me.FindFolderItem(file.Folder)
        Dim files As System.Collections.Generic.List(Of WebApplication2.FileSystemData) = Me.DataSource.Where(Function(x) Not x.IsFolder).ToList()
        If folderItem Is Nothing Then Return Nothing
        Return(From dbItem In files Where dbItem.ParentId = folderItem.Id AndAlso Not dbItem.IsFolder AndAlso Equals(dbItem.Name, file.Name) Select dbItem).FirstOrDefault()
    End Function
End Class
