﻿/* PROJECT: WorldDataAppCS (C#)         CLASS: NameIndex
 * AUTHOR:
 * FILES ACCESSED:
 * INTERNAL INDEX STRUCTURE: 
 * FILE STRUCTURE: 
 * DESCRIPTION:
*******************************************************************************/

using System;
using System.IO;

namespace SharedClassLibrary
{
    public class NameIndex
    {
        //**************************** PRIVATE DECLARATIONS ************************
        private Object[] _tree;
        private short _counter;
        private BinaryWriter _bIndexFileW;
        private BinaryReader _bIndexFileR;
        private FileStream _fIndexFile;
        private MainData _mainData;
        private UserInterface _log;
        //**************************** PUBLIC GET/SET METHODS **********************


        //**************************** PUBLIC CONSTRUCTOR(S) ***********************
        public NameIndex(MainData MD, UserInterface log)
        {
            _mainData = MD;
            _log = log;
            _tree = new object[1];
            _tree[0] = -1;

            _fIndexFile = new FileStream("IndexBackup.bin", FileMode.OpenOrCreate);
            _bIndexFileW = new BinaryWriter(_fIndexFile);
            _bIndexFileR = new BinaryReader(_fIndexFile);

        }
        //**************************** PUBLIC SERVICE METHODS **********************

        /// <summary>
        /// Insert Object record from Raw Data into the BTree
        /// </summary>
        /// <param name="RD">Class storing the information in the tree</param>
        public bool StoreOneCountry(RawData RD)
        {
            int index = 0;
            int parentIndex = 0;
            BSTNode currentNode = null;

            if((int)_tree[0] == -1)
            {
                _tree[_counter++] = new BSTNode(RD);
            }
            else
            {
                Array.Resize<Object>(ref _tree, _tree.Length + 1);


                //Loop Through and find an empty location by name comparision
                while (index != -1)
                {
                    currentNode = (BSTNode)_tree[index];
                    parentIndex = index;

                    if (currentNode.CompareTo(RD.NAME) > 0)
                    {

                        index = currentNode.RChildPtr;
                        //RightNode Child
                    }
                    else
                    {
                        index = currentNode.LChildPtr;
                        //LeftNode Child
                    }

                }

                //Find which node from its previous one searched, to place the new node in the tree.
                if (currentNode.CompareTo(RD.NAME) > 0)
                {
                    ((BSTNode)_tree[parentIndex]).RChildPtr = _counter;
                    //insert Right
                }
                else
                {
                    ((BSTNode)_tree[parentIndex]).LChildPtr = _counter;
                    //insert Left
                }

                _tree[_counter++] = new BSTNode(RD); //Asign new node to tree
            }

            return true;
        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// Gets the the name by searching the tree.
        /// </summary>
        /// <param name="queryID">Name that wants to be seached</param>
        public void QueryByName(string queryID)
        {
            int queryCounter = 0;
            int index = 0;
            bool QueryFound = false;
            BSTNode currentNode = null;
            string record = string.Empty;

            if ((int)_tree[0] == -1)
                return;

            while (index != -1)
            {
                ++queryCounter;
                currentNode = (BSTNode)_tree[index];

                if(currentNode.CompareTo(queryID) == 0)
                {
                    record = _mainData.GetThisData(currentNode.DRP);
                    QueryFound = true;
                    index = -1;
                }
                else if (currentNode.CompareTo(queryID) > 0)
                {
                    index = currentNode.RChildPtr;
                }
                else 
                {
                    index = currentNode.LChildPtr;
                }

                if (QueryFound == true)
                {
                    _log.WriteToLog(record);
                }
                else
                    _log.WriteToLog("**Error:Could not find " + queryID);

                _log.WriteToLog("[" + (--queryCounter) + " BST nodes visited]");
            }

        }

        public void ListByName()
        {
            IOT(0);
        }

        

        //**************************** PRIVATE METHODS *****************************

        //---------------------------------------------------------------------------------
        /// <summary>
        /// Recursive Structure that preforms a In-Order traversal of the tree and its leafs
        /// </summary>
        /// <param name="index">Index of the trees array</param>
        private void IOT(int index)
        {
           
            if ((int)_tree[index] == -1)
                return;
            else if (index == -1)
                return;

            IOT(((BSTNode)_tree[index]).LChildPtr);
            VisitNode((BSTNode)_tree[index]);
            IOT(((BSTNode)_tree[index]).RChildPtr);
        }

        //------------------------------------------------------------------------------------
        /// <summary>
        /// Sends the data over to mainData class and Get's the corresponding record that is
        /// formatted for display.
        /// </summary>
        /// <param name="currentNode">The Node that needs to be grabbed from maindata</param>
        private void VisitNode(BSTNode currentNode)
        {
            string DataLine = _mainData.GetThisData(currentNode.DRP);
            _log.WriteToLog(DataLine);
        }

        //-----------------------------------------------------------------------------------
        /// <summary>
        /// Writes the current tree to a binary file that can be reinialized later
        /// </summary>
        private void WriteTreeToFile()
        {
            BSTNode currentNode = null;
            _bIndexFileW.BaseStream.Seek(0, SeekOrigin.Begin);

            _bIndexFileW.Write(_counter); //Write header with record Count;

            foreach(object j in _tree)
            {
                currentNode = (BSTNode)j;
                _bIndexFileW.Write(currentNode.LChildPtr);
                _bIndexFileW.Write(currentNode.KeyValue);
                _bIndexFileW.Write(currentNode.DRP);
                _bIndexFileW.Write(currentNode.RChildPtr);

            }
        }

        //---------------------------------------------------------------------------------
        /// <summary>
        /// Reads the index backup file and re-does the tree in the order it was placed in
        /// </summary>
        private void ReadTreeFile()
        {
            short rchild, lchild, DRP;
            string KeyValue;
            
            _bIndexFileR.BaseStream.Seek(0, SeekOrigin.Begin);
            _counter = _bIndexFileR.ReadInt16();
            _tree = new object[_counter];

            for(int i = 0; i < _counter; i++)
            {
                lchild = _bIndexFileR.ReadInt16();
                KeyValue = _bIndexFileR.ReadString();
                DRP = _bIndexFileR.ReadInt16();
                rchild = _bIndexFileR.ReadInt16();

                _tree[i] = new BSTNode(lchild, KeyValue, DRP, rchild);
            }
        }

    }
}
