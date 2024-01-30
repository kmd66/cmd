namespace CMS.Model
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
        Title = 1,
        Content = 2,
        Categorie = 3,
        Link = 4,
        ProductCategorie = 5
    }
    public enum PostAccessType : byte
    {
        Unknown = 0,
        Publiced = 1,
    }
    public enum ProductType : byte
    {
        Unknown = 0,
        خرید = 1,
        پیش_خرید = 2,
    }
    public enum OrderType : byte
    {
        Unknown = 0,
        سفارش = 1,
        درحال_بررسی = 2,
        آماده_سازی = 3,

        ارسال = 20,

        تحویل = 50,

        بازگشت = 200,
        لغو = 201,
        انصراف = 202,
    }
    public enum CommentType : byte
    {
        Unknown = 0,
        در_انتضار_تایید = 1,
        منتشر_شده = 2,
        حذف = 3,
    }
    public enum MessageType : byte
    {
        Unknown = 0,
        خوانده_نشده = 1,
        خوانده_شده = 2,
        حذف = 3,
    }
}
