/* PROJECT: WorldDataAppCS (C#)         CLASS: NameIndex
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
        //**************************** PUBLIC GET/SET METHODS **********************


        //**************************** PUBLIC CONSTRUCTOR(S) ***********************
        public NameIndex()
        {
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

                _tree[_counter++] = new BSTNode(RD);
            }

            return true;
        }


        public void QueryByName(string queryID)
        {

        }

        public void ListByName()
        {

        }

        

        //**************************** PRIVATE METHODS *****************************

        //--------------------------------------------------------------------------------
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
