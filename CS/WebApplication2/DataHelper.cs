using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2 {
    public class DataHelper {
        public static List<FileSystemData> CreateDataSource() {
            List<FileSystemData> list = new List<FileSystemData>();

            FileSystemData item = new FileSystemData();
            item.Id = 0;
            item.ParentId = null;
            item.Name = "Available Files";
            item.IsFolder = true;
            item.LastWriteTime = null;
            list.Add(item);

            item = new FileSystemData();
            item.Id = 1;
            item.ParentId = 0;
            item.Name = "User Files - My Files";
            item.IsFolder = true;
            item.LastWriteTime = null;
            list.Add(item);

            item = new FileSystemData();
            item.Id = 2;
            item.ParentId = 0;
            item.Name = "Company Files";
            item.IsFolder = true;
            item.LastWriteTime = null;
            list.Add(item);

            item = new FileSystemData();
            item.Id = 3;
            item.ParentId = 1;
            item.Name = "Some Folder";
            item.IsFolder = true;
            item.LastWriteTime = null;
            list.Add(item);

            item = new FileSystemData();
            item.Id = 4;
            item.ParentId = 0;
            item.Name = "User Files - Shared";
            item.IsFolder = true;
            item.LastWriteTime = null;
            list.Add(item);

            item = new FileSystemData();
            item.Id = 5;
            item.ParentId = 4;
            item.Name = "Employee 001";
            item.IsFolder = true;
            item.LastWriteTime = null;
            list.Add(item);

            item = new FileSystemData();
            item.Id = 6;
            item.ParentId = 4;
            item.Name = "Employee 002";
            item.IsFolder = true;
            item.LastWriteTime = null;
            list.Add(item);

            return list;
        }
    }

    public class FileSystemData {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public Byte[] Data { get; set; }
        public DateTime? LastWriteTime { get; set; }
    }
}