namespace GameZone.Settings
{
    public static class FileSetting
    {
        public const string ImagesPath = "/assets/images/games";
        public const string AllowedExtensions = ".jpg,.jpeg,.png";
        public const int MaxFileSizeInMB = 1;
        public const int MaxFileSizeInByte = MaxFileSizeInMB * 1024 * 1024;
    }
}
