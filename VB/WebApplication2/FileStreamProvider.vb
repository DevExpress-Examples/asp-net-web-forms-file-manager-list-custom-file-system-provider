Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports DevExpress.Web.ASPxFileManager
Imports System.IO
Imports DevExpress.Web.ASPxClasses.Internal
Imports System.Reflection
Imports WebApplication2

Public Class FileStreamProvider
	Inherits FileSystemProviderBase
	Private root As New FileSystemData()
	Private ReadOnly Property DataSource() As List(Of FileSystemData)
		Get
			Return CType(HttpContext.Current.Session("DataSource"), List(Of FileSystemData))
		End Get
	End Property

	Public Sub New(ByVal rootFolder As String)
		MyBase.New(rootFolder)
	End Sub

	Public Overrides ReadOnly Property RootFolderDisplayName() As String
		Get
			Return GetRootFolder().Name
		End Get
	End Property

	Public Overrides Sub CreateFolder(ByVal parent As FileManagerFolder, ByVal name As String)
		DataSource.Add(New FileSystemData() With {.Id = GetHashCode(), .IsFolder = True, .LastWriteTime = DateTime.Now, .Name = name, .ParentId = FindFolderItem(parent).Id})
	End Sub
	Public Overrides Sub DeleteFile(ByVal file As FileManagerFile)
		Dim item As FileSystemData = FindFileItem(file)
		DataSource.Remove(item)
	End Sub
	Public Overrides Sub DeleteFolder(ByVal folder As FileManagerFolder)
		Dim item As FileSystemData = FindFolderItem(folder)
		DataSource.Remove(item)
	End Sub
	Public Overrides Sub MoveFile(ByVal file As FileManagerFile, ByVal newParentFolder As FileManagerFolder)
		Dim item As FileSystemData = FindFileItem(file)
		item.ParentId = FindFolderItem(newParentFolder).Id
	End Sub
	Public Overrides Sub MoveFolder(ByVal folder As FileManagerFolder, ByVal newParentFolder As FileManagerFolder)
		Dim item As FileSystemData = FindFolderItem(folder)
		item.ParentId = FindFolderItem(newParentFolder).Id
	End Sub
	Public Overrides Sub RenameFile(ByVal file As FileManagerFile, ByVal name As String)
		Dim item As FileSystemData = FindFileItem(file)
		item.Name = name
	End Sub
	Public Overrides Sub RenameFolder(ByVal folder As FileManagerFolder, ByVal name As String)
		Dim item As FileSystemData = FindFolderItem(folder)
		item.Name = name
	End Sub
	Public Overrides Sub UploadFile(ByVal folder As FileManagerFolder, ByVal fileName As String, ByVal content As Stream)
		DataSource.Add(New FileSystemData() With {.Id = GetHashCode(), .IsFolder = False, .LastWriteTime = DateTime.Now, .Name = fileName, .ParentId = FindFolderItem(folder).Id})
	End Sub

	Public Overrides Function GetFolders(ByVal parentFolder As FileManagerFolder) As IEnumerable(Of FileManagerFolder)
		Dim dbFolderItem As FileSystemData = FindFolderItem(parentFolder)
		Return From dbItem In DataSource _
		       Where dbItem.IsFolder AndAlso dbItem.ParentId = dbFolderItem.Id _
		       Select New FileManagerFolder(Me, parentFolder, dbItem.Name)
	End Function
	Public Overrides Function GetFiles(ByVal folder As FileManagerFolder) As IEnumerable(Of FileManagerFile)
		Dim folderItem As FileSystemData = FindFolderItem(folder)
		Return From dbItem In DataSource _
		       Where (Not dbItem.IsFolder) AndAlso dbItem.ParentId = folderItem.Id _
		       Select New FileManagerFile(Me, folder, dbItem.Name)
	End Function
	Public Overrides Function Exists(ByVal file As FileManagerFile) As Boolean
		Return FindFileItem(file) IsNot Nothing
	End Function
	Public Overrides Function Exists(ByVal folder As FileManagerFolder) As Boolean
		Return FindFolderItem(folder) IsNot Nothing
	End Function
	Public Overrides Function ReadFile(ByVal file As FileManagerFile) As System.IO.Stream
		Return New MemoryStream(FindFileItem(file).Data.ToArray())
	End Function
	Public Overrides Function GetLastWriteTime(ByVal file As FileManagerFile) As DateTime
		Dim dbFileItem = FindFileItem(file)
		Return dbFileItem.LastWriteTime.GetValueOrDefault(DateTime.Now)
	End Function
	Private Function GetRootFolder() As FileSystemData
		Return DataSource.Where(Function(x) x.IsFolder AndAlso x.ParentId Is Nothing).FirstOrDefault()
	End Function
	Protected Function FindFolderItem(ByVal folder As FileManagerFolder) As FileSystemData
		Dim folders = DataSource.Where(Function(x) x.IsFolder)
		Return (From item In folders _
		        Where item.IsFolder AndAlso GetRelativeName(item) = folder.RelativeName _
		        Select item).FirstOrDefault()
	End Function
	Protected Function GetRelativeName(ByVal folder As FileSystemData) As String
		Dim root As FileSystemData = GetRootFolder()
		If folder.Id = root.Id Then
			Return String.Empty
		End If
		If folder.ParentId = root.Id Then
			Return folder.Name
		End If
		Dim folders = DataSource.Where(Function(x) x.IsFolder)
		Dim name As String = GetRelativeName(folders.Where(Function(x) x.Id = folder.ParentId).FirstOrDefault())
		Return If(name Is Nothing, Nothing, Path.Combine(name, folder.Name))
	End Function
	Protected Function FindFileItem(ByVal file As FileManagerFile) As FileSystemData
		Dim folderItem As FileSystemData = FindFolderItem(file.Folder)
		Dim files As List(Of FileSystemData) = DataSource.Where(Function(x) (Not x.IsFolder)).ToList()
		If folderItem Is Nothing Then
			Return Nothing
		End If
		Return (From dbItem In files _
		        Where dbItem.ParentId = folderItem.Id AndAlso (Not dbItem.IsFolder) AndAlso dbItem.Name = file.Name _
		        Select dbItem).FirstOrDefault()
	End Function
End Class