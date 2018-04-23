using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web.ASPxFileManager;
using System.IO;
using DevExpress.Web.ASPxClasses.Internal;
using System.Reflection;
using WebApplication2;

public class FileStreamProvider : FileSystemProviderBase {
    FileSystemData root = new FileSystemData();
    List<FileSystemData> DataSource {
        get { return (List<FileSystemData>)HttpContext.Current.Session["DataSource"]; }
    }

    public FileStreamProvider(string rootFolder)
        : base(rootFolder) { }

    public override string RootFolderDisplayName { get { return GetRootFolder().Name; } }

    public override void CreateFolder(FileManagerFolder parent, string name) {
        DataSource.Add(new FileSystemData() {
            Id = GetHashCode(),
            IsFolder = true,
            LastWriteTime = DateTime.Now,
            Name = name,
            ParentId = FindFolderItem(parent).Id
        });
    }
    public override void DeleteFile(FileManagerFile file) {
        FileSystemData item = FindFileItem(file);
        DataSource.Remove(item);
    }
    public override void DeleteFolder(FileManagerFolder folder) {
        FileSystemData item = FindFolderItem(folder);
        DataSource.Remove(item);
    }
    public override void MoveFile(FileManagerFile file, FileManagerFolder newParentFolder) {
        FileSystemData item = FindFileItem(file);
        item.ParentId = FindFolderItem(newParentFolder).Id;
    }
    public override void MoveFolder(FileManagerFolder folder, FileManagerFolder newParentFolder) {
        FileSystemData item = FindFolderItem(folder);
        item.ParentId = FindFolderItem(newParentFolder).Id;
    }
    public override void RenameFile(FileManagerFile file, string name) {
        FileSystemData item = FindFileItem(file);
        item.Name = name;
    }
    public override void RenameFolder(FileManagerFolder folder, string name) {
        FileSystemData item = FindFolderItem(folder);
        item.Name = name;
    }
    public override void UploadFile(FileManagerFolder folder, string fileName, Stream content) {
        DataSource.Add(new FileSystemData() {
            Id = GetHashCode(),
            IsFolder = false,
            LastWriteTime = DateTime.Now,
            Name = fileName,
            ParentId = FindFolderItem(folder).Id
        });
    }

    public override IEnumerable<FileManagerFolder> GetFolders(FileManagerFolder parentFolder) {
        FileSystemData dbFolderItem = FindFolderItem(parentFolder);
        return
            from dbItem in DataSource
            where dbItem.IsFolder && dbItem.ParentId == dbFolderItem.Id
            select new FileManagerFolder(this, parentFolder, dbItem.Name);
    }
    public override IEnumerable<FileManagerFile> GetFiles(FileManagerFolder folder) {
        FileSystemData folderItem = FindFolderItem(folder);
        return
            from dbItem in DataSource
            where !dbItem.IsFolder && dbItem.ParentId == folderItem.Id
            select new FileManagerFile(this, folder, dbItem.Name);
    }
    public override bool Exists(FileManagerFile file) {
        return FindFileItem(file) != null;
    }
    public override bool Exists(FileManagerFolder folder) {
        return FindFolderItem(folder) != null;
    }
    public override System.IO.Stream ReadFile(FileManagerFile file) {
        return new MemoryStream(FindFileItem(file).Data.ToArray());
    }
    public override DateTime GetLastWriteTime(FileManagerFile file) {
        var dbFileItem = FindFileItem(file);
        return dbFileItem.LastWriteTime.GetValueOrDefault(DateTime.Now);
    }
    private FileSystemData GetRootFolder() {
        return DataSource
            .Where(x => x.IsFolder && x.ParentId == null)
            .FirstOrDefault();
    }
    protected FileSystemData FindFolderItem(FileManagerFolder folder) {
        var folders = DataSource.Where(x => x.IsFolder);
        return
            (from item in folders
             where item.IsFolder && GetRelativeName(item) == folder.RelativeName
             select item).FirstOrDefault();
    }
    protected string GetRelativeName(FileSystemData folder) {
        FileSystemData root = GetRootFolder();
        if (folder.Id == root.Id) return string.Empty;
        if (folder.ParentId == root.Id) return folder.Name;
        var folders = DataSource.Where(x => x.IsFolder);
        string name = GetRelativeName(folders.Where(x => x.Id == folder.ParentId).FirstOrDefault());
        return name == null ? null : Path.Combine(name, folder.Name);
    }
    protected FileSystemData FindFileItem(FileManagerFile file) {
        FileSystemData folderItem = FindFolderItem(file.Folder);
        List<FileSystemData> files = DataSource.Where(x => !x.IsFolder).ToList();
        if (folderItem == null)
            return null;
        return
            (from dbItem in files
             where dbItem.ParentId == folderItem.Id && !dbItem.IsFolder && dbItem.Name == file.Name
             select dbItem).FirstOrDefault();
    }
}