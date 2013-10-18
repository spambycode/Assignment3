/* PROJECT:  Asign 1 (C#)            PROGRAM: SetupProgram
 * AUTHOR: George Karaszi   
 *******************************************************************************/

using System;
using System.IO;

using SharedClassLibrary;

namespace SetupProgram
{
    public class SetupProgram
    {
        public static void Main(string[] args)
        {
            int RecordCount   = 0;

            string fileNameSuffix;
            if (args.Length > 0)
            {
                fileNameSuffix = args[0];
            }
            else
            {
                fileNameSuffix = "A3";
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            string FileName = "RawData" + fileNameSuffix + ".csv";

            SharedClassLibrary.UserInterface UI = new UserInterface();
            SharedClassLibrary.RawData RD = new RawData(UI, FileName);
            SharedClassLibrary.MainData MD = new MainData(UI);
            SharedClassLibrary.NameIndex NI = new NameIndex(MD, UI, false);

            UI.WriteToLog("\n***************Setup App Start***************\n");
            while (RD.ReadOneCountry() != true)
            {
                ++RecordCount;
                NI.StoreOneCountry(RD);
            }

            UI.WriteToLog("Setup completed " + RecordCount + " records process");
            UI.WriteToLog("\n***************Setup App END***************\n");
            MD.FinishUp();
            RD.FinishUp();
            NI.FinishUp();
            UI.FinishUp(true, false);



        }
        //*********************** PRIVATE METHODS ********************************


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
    }
}
