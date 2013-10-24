/* PROJECT:  Asign 3 (C#)            PROGRAM: AutoTesterUtility
 * AUTHOR: George Karaszi   
 *******************************************************************************/

using System;
using System.IO;

using SetupProgram;
using UserApp;
using PrettyPrintUtility;

namespace WorldDataAppCS
{
    class AutoTesterUtility
    {
        static void Main(string[] args)
        {

            // The 3 parallel arrays (all strings, including the N's) with
            //      - hard-coded SUFFIX values to designate which files to use
            //      - N's to limit how many records to display during testing
            // The dataFileSuffix is used for RawData*.csv, MainData*.bin,
            //      NameIndexBackup*.bin, CodeIndexBackup*.bin
            string[] dataFileSuffix = { "A3" };
            string[] transFileSuffix = {  };

            //Delete the SINGLE output Log.txt file (if it exists)
            DeleteFile("Log.txt");
            DeleteFile("IndexBackup.bin");

               
                SetupProgram.SetupProgram.Main(new string[] { dataFileSuffix[0] });
                PrettyPrintUtility.PrettyPrintUtility.Main(new string[] { dataFileSuffix[0] });
                UserApp.UserApp.Main(new string[] {});
                PrettyPrintUtility.PrettyPrintUtility.Main(new string[] { dataFileSuffix[0]});
        }
        //**************************************************************************
        private static bool DeleteFile(String fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                return true;
            }
            else
            {
                return false;
            }
        }

        //**************************************************************************
    }
}
