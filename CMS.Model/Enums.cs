﻿namespace CMS.Model
{
    public enum UserType : byte
    {
        Unknown = 0,
        Admin = 100
    }
    public enum AlertType 
    {
        info,
        success,
        warning,
        error,
    }
    public enum FileExtensions
    {
        Unknown,
        dir,
        pdf,
        jpeg ,
        jpg,
        png,
        gif,
        bmp,
        tiff ,
        mp4 ,
        cer ,
        xls ,
        xlsx,
        doc ,
        docx ,
        zip ,
        rar ,
        _7z
    }
    public enum PathType
    {
        File,
        Folder
    }
    public enum MenuType : byte
    {
        Unknown = 0,
        Title =1,
        Content =2,
        Categorie=3,
        Link=4
    }

}
