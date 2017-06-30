using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BrainStructures;

namespace DataAccess
{
                                                            // Used to wrap methods of the File
                                                            // class, preventing arbitrary file
                                                            // access by classes that don't
                                                            // need that capability.
    public class FileManager
    {
                                                            // A wrapper for File.Copy
        public static void Import(String oldFile, String newFile)
        {
            File.Copy(oldFile, newFile);
        }

                                                            // A wrapper for File.Delete
        public static void DeleteNet(String filePath)
        {
            File.Delete(filePath);
        }

        public static bool FileExists(String filePath)
        {
            return File.Exists(AppData.TrainingDocumentsPath + filePath);
        }
    }
}
