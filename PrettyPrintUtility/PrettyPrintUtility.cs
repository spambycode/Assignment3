/* PROJECT: Asign 3 (C#) PROGRAM: PrettyPrint (AKA ShowFilesUtility)
* AUTHOR: George Karaszi 10-24-2013
* FILE ACCESSED:
* (INPUT) IndexBackup.bin
* (OUTPUT) log.txt
*
* USEAGE: To access and showcase the data stored within binaray files.
* Quick run of the mill looping structure that will loops till
* the end of the file, collecting information and displaying it
* in a formatted matter.
*******************************************************************************/

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace PrettyPrintUtility
{
    public class PrettyPrintUtility
    {
        static FileStream fMainDataFile;
        static BinaryReader bMainDataFileReader;

        static short recordCount;
        static short RootPtr;


        public static void Main(string[] args)
        {

            fMainDataFile = new FileStream("IndexBackup.bin", FileMode.OpenOrCreate);
            bMainDataFileReader = new BinaryReader(fMainDataFile);
            recordCount = bMainDataFileReader.ReadInt16();
            RootPtr = bMainDataFileReader.ReadInt16();

            string[] MainRecordList = ReadFile(bMainDataFileReader);


            PrintResults(MainRecordList);
            FinishUp();

        }

        //---------------------------------------------------------------------------------------------
        /// <summary>
        /// Loop through and store all viable records into a array
        /// </summary>
        /// <param name="fileReader">Current binary reader for the file being accessed</param>
        /// <param name="headerLength">Length (in bytes) of the header file</param>
        /// <returns>An array of formatted records ready to be displayed</returns>

        private static string[] ReadFile(BinaryReader fileReader)
        {
            short lch, rch, drp;
            string Name;

            string lch_pad, rch_pad;

            string formatRecord;
            List<string> RecordCollection = new List<string>(); //List of formatted record strings

            for (int i = 0; i < recordCount; i++)
            {

                lch  = fileReader.ReadInt16();
                Name = fileReader.ReadString();
                drp  = fileReader.ReadInt16();
                rch  = fileReader.ReadInt16();

                if (lch == -1)
                    lch_pad = lch.ToString("D2");
                else
                    lch_pad = lch.ToString("D3");

                if (rch == -1)
                    rch_pad = rch.ToString("D2");
                else
                    rch_pad = rch.ToString("D3");

                formatRecord = "[" + Convert.ToString(i).PadLeft(3, '0') + "]".PadRight(2) +
                                lch_pad.PadRight(5) +
                                Name.PadRight(18) +
                                drp.ToString("D3").PadLeft(4).PadRight(6) +
                                rch_pad.PadLeft(3);

                RecordCollection.Add(formatRecord);
            }

            return RecordCollection.ToArray();

        }


        //-----------------------------------------------------------------------------
        /// <summary>
        /// Close all files that are open
        /// </summary>
        private static void FinishUp()
        {
            bMainDataFileReader.Close();
            fMainDataFile.Close();
        }



        //------------------------------------------------------------------------------
        /// <summary>
        /// Formates the header to be displayed
        /// </summary>
        /// <returns>A ready to use string aligned in its columns</returns>
        private static string FormatHeader()
        {

            return "[SUB]".PadRight(6) +
                   "LCH".PadRight(5) +
                   "NAME".PadRight(18, '-') +
                   "DRP".PadLeft(4).PadRight(6) +
                   "RCH";
        }

        //------------------------------------------------------------------------------
        /// <summary>
        /// Print the results from the formatted text
        /// </summary>
        private static void PrintResults(string[] MainDataList)
        {
            StreamWriter logFile = new StreamWriter("Log.txt", true);
            string header = FormatHeader();

            logFile.WriteLine("\n***************Pretty Print Start***************\n");
            logFile.WriteLine("RootPtr is {0}, N is {1}", RootPtr, recordCount);
            logFile.WriteLine(header);

            foreach (string s in MainDataList)
            {
                logFile.WriteLine(s);
            }

            logFile.WriteLine("End ***********************************************************");
            logFile.WriteLine();
            

            logFile.Close();
        }

    }
}