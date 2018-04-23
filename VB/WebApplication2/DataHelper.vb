Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Namespace WebApplication2
	Public Class DataHelper
		Public Shared Function CreateDataSource() As List(Of FileSystemData)
			Dim list As List(Of FileSystemData) = New List(Of FileSystemData)()

			Dim item As New FileSystemData()
			item.Id = 0
			item.ParentId = Nothing
			item.Name = "Available Files"
			item.IsFolder = True
			item.LastWriteTime = Nothing
			list.Add(item)

			item = New FileSystemData()
			item.Id = 1
			item.ParentId = 0
			item.Name = "User Files - My Files"
			item.IsFolder = True
			item.LastWriteTime = Nothing
			list.Add(item)

			item = New FileSystemData()
			item.Id = 2
			item.ParentId = 0
			item.Name = "Company Files"
			item.IsFolder = True
			item.LastWriteTime = Nothing
			list.Add(item)

			item = New FileSystemData()
			item.Id = 3
			item.ParentId = 1
			item.Name = "Some Folder"
			item.IsFolder = True
			item.LastWriteTime = Nothing
			list.Add(item)

			item = New FileSystemData()
			item.Id = 4
			item.ParentId = 0
			item.Name = "User Files - Shared"
			item.IsFolder = True
			item.LastWriteTime = Nothing
			list.Add(item)

			item = New FileSystemData()
			item.Id = 5
			item.ParentId = 4
			item.Name = "Employee 001"
			item.IsFolder = True
			item.LastWriteTime = Nothing
			list.Add(item)

			item = New FileSystemData()
			item.Id = 6
			item.ParentId = 4
			item.Name = "Employee 002"
			item.IsFolder = True
			item.LastWriteTime = Nothing
			list.Add(item)

			Return list
		End Function
	End Class

	Public Class FileSystemData
		Private privateId As Nullable(Of Integer)
		Public Property Id() As Nullable(Of Integer)
			Get
				Return privateId
			End Get
			Set(ByVal value As Nullable(Of Integer))
				privateId = value
			End Set
		End Property
		Private privateParentId As Nullable(Of Integer)
		Public Property ParentId() As Nullable(Of Integer)
			Get
				Return privateParentId
			End Get
			Set(ByVal value As Nullable(Of Integer))
				privateParentId = value
			End Set
		End Property
		Private privateName As String
		Public Property Name() As String
			Get
				Return privateName
			End Get
			Set(ByVal value As String)
				privateName = value
			End Set
		End Property
		Private privateIsFolder As Boolean
		Public Property IsFolder() As Boolean
			Get
				Return privateIsFolder
			End Get
			Set(ByVal value As Boolean)
				privateIsFolder = value
			End Set
		End Property
		Private privateData As Byte()
		Public Property Data() As Byte()
			Get
				Return privateData
			End Get
			Set(ByVal value As Byte())
				privateData = value
			End Set
		End Property
		Private privateLastWriteTime As Nullable(Of DateTime)
		Public Property LastWriteTime() As Nullable(Of DateTime)
			Get
				Return privateLastWriteTime
			End Get
			Set(ByVal value As Nullable(Of DateTime))
				privateLastWriteTime = value
			End Set
		End Property
	End Class
End Namespace