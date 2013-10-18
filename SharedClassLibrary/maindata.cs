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

        private BinaryReader mainDataFile;                //RandomAccess File structure
        private UserInterface _LogFile;                  //Log file Access
        private string fileName;                        //Holds the file name of main data
        private int _sizeOfHeaderRec;                   //Size of the reader record
        private int _sizeOfDataRec;                     //Size of all the data fields
        private char[] _code = new char[3];             //Country code
        private char[] _name = new char[18];            //Name of country
        private char[] _continent = new char[13];       //What continent the country is located
        private char[] _population = new char[10];      //Total population of the country
        private char[] _lifeExpectancy = new char[4];   //The average time someone is alive in the country

        //**************************** PUBLIC GET/SET METHODS **********************


        //**************************** PUBLIC CONSTRUCTOR(S) ***********************
        public MainData(UserInterface LogInterFace)
        {
            //Calculate sizes for RandomAccess byte offset
            _sizeOfHeaderRec = 0;
            _sizeOfDataRec   = _code.Length + _name.Length + _continent.Length
                               + _population.Length + _lifeExpectancy.Length + 2;


            //Open and create a new file
            fileName = "MainDataA3.txt";

            //Allow access to log file
            _LogFile = LogInterFace;

            //Open or Create Main data file
            mainDataFile = new BinaryReader(new FileStream(fileName, FileMode.Open));
            
            _LogFile.WriteToLog("Opened " + fileName + " File");
        }

        //**************************** PUBLIC SERVICE METHODS **********************

        public string GetThisData(int RRN)
        {
            int ByteOffSet = CalculateByteOffSet(RRN);
            mainDataFile.BaseStream.Seek(ByteOffSet, SeekOrigin.Begin);

            _code           = mainDataFile.ReadChars(_code.Length);
            _name           = mainDataFile.ReadChars(_name.Length);
            _continent      = mainDataFile.ReadChars(_continent.Length);
            _population     = mainDataFile.ReadChars(_population.Length);
            _lifeExpectancy = mainDataFile.ReadChars(_lifeExpectancy.Length);

            return FormatRecord();
        }

        //-------------------------------------------------------------------------
        /// <summary>
        /// Closes the main data file 
        /// </summary>
        public void FinishUp()
        {
            mainDataFile.Close();
            _LogFile.WriteToLog("Closed " + fileName + " File");
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
            return _sizeOfHeaderRec + ((RRN-1) * _sizeOfDataRec);
        }

        //------------------------------------------------------------------------------
        /// <summary>
        /// Formates the string to de-aligned with its header columns
        /// </summary>
        /// <returns>formatted string ready to be used</returns>
        private string FormatRecord()
        {
            return new string(_code).PadRight(6) +
                   new string(_name).PadRight(18) +
                   new string(_continent).PadLeft(15).PadRight(15) +
                   string.Format("{0:#,###,###,###}", Convert.ToInt64(new string(_population))).PadLeft(15).PadRight(17) +
                   string.Format("{0:0.0#}", Convert.ToSingle(new string(_lifeExpectancy))).PadRight(5).PadLeft(6);
        }

    }
}
