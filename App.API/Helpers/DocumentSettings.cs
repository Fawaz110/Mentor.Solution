namespace App.API.Helpers
{
    public static class DocumentSettings
    {
        /// <summary>
        /// Images extenssions used to make sure if file uploaded is valid.
        /// </summary>
        public static string[] ImageExtensions = { "jpg", "jpeg", "png", "gif", "bmp", "tiff", "tif", "webp", "svg",  /* Scalable Vector Graphics */"ico",  /* Icon files */"heic", /* High Efficiency Image Container (HEIC)*/"heif"  /* High Efficiency Image Format (HEIF)*/};
        
        /// <summary>
        /// Method used to check if file match given extenssions or not.
        /// </summary>
        /// <param name="file">The uploaded file needed to be checked.</param>
        /// <param name="extenssions">Given extenssions known that file ust match.</param>
        /// <returns></returns>
        public static bool IsValidFile(IFormFile file, string[] extenssions)
        {
            var split = file.FileName.Split('.');

            var extenssion = split[split.Length - 1];

            return extenssions.Contains(extenssion.ToLower());
        }

        /// <summary>
        /// Method used to upload file to specific folder.
        /// </summary>
        /// <param name="file">file needed to be uploaded.</param>
        /// <param name="folderName">folder name or path need to contain this file.</param>
        /// <returns></returns>
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get located folder path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

            // 2. Get file name and make it unique
            string fileName = $"{Guid.NewGuid()}-{file.FileName}";

            // 3. Get file path
            string filePath = Path.Combine(folderPath, fileName);

            // 4. Save file as streams [data per time]
            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        /// <summary>
        /// Method used to delete file from specific folder.
        /// </summary>
        /// <param name="fileName">file name wanted to be deleted</param>
        /// <param name="folderName">folder or path that contains this file</param>
        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, fileName);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}
