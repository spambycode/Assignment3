/* PROJECT:  Asign 3 (C#)            PROGRAM: RawData class
 * AUTHOR: George Karaszi   
 *******************************************************************************/

using System;
using System.IO;

namespace SharedClassLibrary
{
    public class RawData
    {
        //**************************** PRIVATE DECLARATIONS ************************
        private StreamReader rawDataFile;
        private UserInterface logFile;
        private string filename;

        //**************************** PUBLIC GET/SET METHODS **********************

        public string ID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string CONTINENT { get; set; }
        public string LIFEEXPECTANCY { get; set; }
        

        //**************************** PUBLIC CONSTRUCTOR(S) ***********************

        public RawData(UserInterface LogFile, string filename)
        {
            try
            {
                logFile = LogFile;

                this.filename = filename;
                rawDataFile = new StreamReader(filename);
                logFile.WriteToLog("Open " + filename + " File");
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }
        //**************************** PUBLIC SERVICE METHODS **********************

        /// <summary>
        /// Reads one country from the rawdata file.
        /// </summary>
        /// <returns>EOF status</returns>
        public bool ReadOneCountry()
        {
            if (rawDataFile.EndOfStream != true)
            {
                var line = rawDataFile.ReadLine();
                var split = line.Split(',');


                //Asign variables from the read record
                ID   = split[0];
                CODE = split[1];
                NAME = split[2];
                CONTINENT = split[3];
                LIFEEXPECTANCY = split[4];


                return false;

            }
            return true;
        }

        /// <summary>
        /// Close opened files
        /// </summary>
        public void FinishUp()
        {
            rawDataFile.Close();
            logFile.WriteToLog("Closed " + filename + " file");
        }


        //**************************** PRIVATE METHODS *****************************

    }
}
