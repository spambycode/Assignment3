/* PROJECT:  Asign 3 (C#)            PROGRAM: UserApp
 * AUTHOR: George Karaszi   
 *******************************************************************************/

using System;

using SharedClassLibrary;

namespace UserApp
{
    public class UserApp
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("OK, starting UserApp");

            // Detect whether this program is being run by AutoTesterUtility,
            //      or manually by developer & fix fileNameSuffix accordingly.
            // <WRITE APPROPRIATE CODE HERE>
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            int CommandCount        = 0;
            string transFileSuffix = "";
            string transFileName = "TransDataA3";
            string command = "";

            if(args.Length > 0)
            {
                transFileSuffix = args[0];
            }
            else
            {
                transFileSuffix = "";
            }

            transFileName += transFileSuffix + ".txt";

            SharedClassLibrary.UserInterface UI = new UserInterface(true, true, transFileName);
            SharedClassLibrary.MainData MD = new MainData(UI);
            SharedClassLibrary.NameIndex NI = new NameIndex(MD, UI, true);

            UI.WriteToLog("\n***************User App Start***************\n");

            while(UI.GetOneTransdata(out command) == false)
            {
                UI.WriteToLog(command);

                switch(command.Substring(0, 2))
                {
                    case "QN":
                        NI.QueryByName(QueryData(command));
                        break;
                        
                    case "LN":
                        NI.ListByName();
                        break;

                    default:
                        UI.WriteToLog("**ERROR: Command is not working");
                        break;
                }

                CommandCount++;

            }

            MD.FinishUp();
            NI.FinishUp();
            UI.WriteToLog("\n***************User App END***************\n");
            UI.WriteToLog(string.Format("UserApp completed: {0} transactions handled", CommandCount));
            UI.FinishUp(true, true);


        }
        //*********************** PRIVATE METHODS ********************************

        private static string QueryData(string input)
        {
            return input.Substring(3);
        }



    }
}
