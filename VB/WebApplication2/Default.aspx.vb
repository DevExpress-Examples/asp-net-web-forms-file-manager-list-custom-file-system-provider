Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web
Imports WebApplication2

Public Partial Class [Default]
    Inherits Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
        GenerateDataSource()
    End Sub

    Private Sub GenerateDataSource()
        If Session("DataSource") Is Nothing Then
            Session("DataSource") = DataHelper.CreateDataSource()
        End If
    End Sub

    Protected Sub fileManager_FileUploading(ByVal sender As Object, ByVal e As FileManagerFileUploadEventArgs)
        ValidateSiteEdit(e)
    End Sub

    Protected Sub fileManager_ItemRenaming(ByVal sender As Object, ByVal e As FileManagerItemRenameEventArgs)
        ValidateSiteEdit(e)
    End Sub

    Protected Sub fileManager_ItemMoving(ByVal sender As Object, ByVal e As FileManagerItemMoveEventArgs)
        ValidateSiteEdit(e)
    End Sub

    Protected Sub fileManager_ItemDeleting(ByVal sender As Object, ByVal e As FileManagerItemDeleteEventArgs)
        ValidateSiteEdit(e)
    End Sub

    Protected Sub fileManager_FolderCreating(ByVal sender As Object, ByVal e As FileManagerFolderCreateEventArgs)
        ValidateSiteEdit(e)
    End Sub

    Private Sub ValidateSiteEdit(ByVal e As FileManagerActionEventArgsBase)
        ' comment out this line to enable editing
        e.Cancel = True
        e.ErrorText = "Data modifications are not allowed in the example."
    End Sub
End Class
