/* PROJECT:  Asign 1 (C#)            PROGRAM: MainData class
 * AUTHOR: George Karaszi   
 *******************************************************************************/

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace SharedClassLibrary
{
    public class MainData
    {
        //**************************** PRIVATE DECLARATIONS ************************

        private FileStream mainDataFile;                //RandomAccess File structure
        private UserInterface _LogFile;                  //Log file Access
        private string fileName;                        //Holds the file name of main data
        private int headerCount = 0;                    //Counts how many recorders
        private int _sizeOfHeaderRec;                   //Size of the reader record
        private int _sizeOfDataRec;                     //Size of all the data fields
        private char[] _headerRec = new char[3];        //Header rec of the document
        private char[] _id = new char[3];               //ID of record
        private char[] _code = new char[3];             //Country code
        private char[] _name = new char[17];            //Name of country
        private char[] _continent = new char[11];       //What continent the country is located
        private char[] _population = new char[10];      //Total population of the country
        private char[] _lifeExpectancy = new char[4];   //The average time someone is alive in the country

        //**************************** PUBLIC GET/SET METHODS **********************


        //**************************** PUBLIC CONSTRUCTOR(S) ***********************
        public MainData(UserInterface LogInterFace)
        {
            //Calculate sizes for RandomAccess byte offset
            _sizeOfHeaderRec = _headerRec.Length;
            _sizeOfDataRec   = _id.Length + _code.Length + _name.Length + _continent.Length
                               + _population.Length + _lifeExpectancy.Length;


            //Open and create a new file
            fileName = "MainData.txt";

            //Allow access to log file
            _LogFile = LogInterFace;

            //Open or Create Main data file
            mainDataFile = new FileStream(fileName, FileMode.OpenOrCreate);
            _LogFile.WriteToLog("Opened " + fileName + " File");

            //Get total records in file (Default is 0)
            headerCount = 0;
        }

        //**************************** PUBLIC SERVICE METHODS **********************

        //-------------------------------------------------------------------------
        /// <summary>
        /// Closes the main data file 
        /// </summary>
        public void FinishUp()
        {
            mainDataFile.Close();
            _LogFile.WriteToLog("Closed " + fileName + " File");
        }

        public string GetThisData(int RRN)
        {
            return string.Empty;
        }

        //**************************** PRIVATE METHODS *****************************

        //---------------------------------------------------------------------------
        /// <summary>
        /// Obtain the offset to where the file pointer needs to point
        /// </summary>
        /// <param name="RRN">An ID to what record that needs to be obtained</param>
        /// <returns>offset to file positions</returns>
        private int CalculateByteOffSet(int RRN)
        {
            return _sizeOfHeaderRec + ((RRN - 1) * _sizeOfDataRec);
        }

        //------------------------------------------------------------------------------
        /// <summary>
        /// Formates the string to de aligned with its header columns
        /// </summary>
        /// <param name="record">record from main data</param>
        /// <returns>formatted string ready to be used</returns>
        private string FormatRecord(string record)
        {
            int stringPos = 0;

            string id = record.Substring(stringPos, 3).Trim();
            stringPos += 3;
            string code = record.Substring(stringPos, 3).Trim();
            stringPos += 3;
            string name = record.Substring(stringPos, 17).Trim();
            stringPos += 17;
            string continent = record.Substring(stringPos, 11).Trim();
            stringPos += 11;
            string region = record.Substring(stringPos, 10).Trim();
            stringPos += 10;
            string surfaceArea = record.Substring(stringPos, 8).Trim();
            stringPos += 8;
            string yearOfIndep = record.Substring(stringPos, 5).Trim();
            stringPos += 5;
            string population = record.Substring(stringPos, 10).Trim();
            stringPos += 10;
            string lifeExpectancy = record.Substring(stringPos, 4).Trim();




            string t =  id.PadRight(4, ' ') +
                        code.PadRight(5, ' ') +
                        name.PadRight(20, ' ') +
                        continent.PadRight(18) +
                        region.PadRight(15, ' ') +
                        surfaceArea.PadRight(15, ' ') +
                        yearOfIndep.PadRight(9, ' ') +
                        population.PadRight(13, ' ') +
                        lifeExpectancy;

            return t;
        }

        //----------------------------------------------------------------------------
        /// <summary>
        /// Returns a header that is formated to show all data that is inputted
        /// </summary>
        /// <returns>Header string</returns>

        private string FormatHeader()
        {

            string t =  "ID".PadRight(4, ' ') +
                        "CODE".PadRight(5, ' ') +
                        "NAME".PadRight(20, ' ') +
                        "CONTINENT".PadRight(18, ' ') +
                        "REGION".PadRight(15, ' ') +
                        "AREA".PadRight(15, ' ') +
                        "INDEP".PadRight(9, ' ') +
                        "POPULATION".PadRight(13, ' ') +
                        "L.EXP";

            return t;
        }

        //--------------------------------------------------------------------------
        /// <summary>
        /// Reads one block of data from the file based on the RRN
        /// </summary>
        /// <param name="RRN">Record location</param>
        /// <returns>A string based on its RRN location in file</returns>
        private byte []ReadOneRecord(int RRN)
        {
            int byteOffSet    = CalculateByteOffSet(RRN);

            mainDataFile.Seek(byteOffSet, SeekOrigin.Begin);

            return ReadOneRecord();
        }

        //----------------------------------------------------------------------------
        /// <summary>
        /// Reads one record at its current position in the file stream.
        /// </summary>
        /// <returns>Array of the record</returns>
        private byte[] ReadOneRecord()
        {
            byte[] recordData = new byte[_sizeOfDataRec];

            mainDataFile.Read(recordData, 0, recordData.Length);

            return recordData;
        }

        //--------------------------------------------------------------------------
        /// <summary>
        /// Writes one country to the file by the given byteOffSet
        /// </summary>
        /// <param name="byteOffSet">Where in the file to begin the writing process</param>
        private void WriteOneCountry(int byteOffSet)
        {

            if(mainDataFile.Length < byteOffSet)
                mainDataFile.SetLength(byteOffSet);

            //Move file pointer to new location
            mainDataFile.Seek(byteOffSet, SeekOrigin.Begin);

            //Write the information to the maindata file
            WriteOneRecord(_id);
            WriteOneRecord(_code);
            WriteOneRecord(_name);
            WriteOneRecord(_continent);
            WriteOneRecord(_population);
            WriteOneRecord(_lifeExpectancy);

        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// Uses the write function in file stream to write a record to the main file
        /// </summary>
        /// <param name="input">Input wanted to write to file</param>

        private void WriteOneRecord(char[] input)
        {
            mainDataFile.Write(Encoding.ASCII.GetBytes(input), 0, input.Length);
        }

    }
}
