using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using WebApplication2;

public partial class Default : System.Web.UI.Page {

    protected void Page_Init(object sender, EventArgs e) {
        GenerateDataSource();
    }

    void GenerateDataSource() {
        if (Session["DataSource"] == null) {
            Session["DataSource"] = DataHelper.CreateDataSource();
        }
    }
    protected void fileManager_FileUploading(object sender, FileManagerFileUploadEventArgs e) {
        ValidateSiteEdit(e);
    }
    protected void fileManager_ItemRenaming(object sender, FileManagerItemRenameEventArgs e) {
        ValidateSiteEdit(e);
    }
    protected void fileManager_ItemMoving(object sender, FileManagerItemMoveEventArgs e) {
        ValidateSiteEdit(e);
    }
    protected void fileManager_ItemDeleting(object sender, FileManagerItemDeleteEventArgs e) {
        ValidateSiteEdit(e);
    }
    protected void fileManager_FolderCreating(object sender, FileManagerFolderCreateEventArgs e) {
        ValidateSiteEdit(e);
    }
    void ValidateSiteEdit(FileManagerActionEventArgsBase e) {
        // comment out this line to enable editing
        e.Cancel = true;
        e.ErrorText = "Data modifications are not allowed in the example.";
    }
}

