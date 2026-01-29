Imports System.Collections.Generic

Namespace WebApplication2

    Public Class DataHelper

        Public Shared Function CreateDataSource() As List(Of FileSystemData)
            Dim list As List(Of FileSystemData) = New List(Of FileSystemData)()
            Dim item As FileSystemData = New FileSystemData()
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

        Public Property Id As Integer?

        Public Property ParentId As Integer?

        Public Property Name As String

        Public Property IsFolder As Boolean

        Public Property Data As Byte()

        Public Property LastWriteTime As Date?
    End Class
End Namespace
