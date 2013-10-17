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
        private short counter;

        //**************************** PUBLIC GET/SET METHODS **********************


        //**************************** PUBLIC CONSTRUCTOR(S) ***********************
        public NameIndex()
        {
            _tree = new object[1];
            _tree[0] = -1;
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
                _tree[counter++] = new BSTNode(RD);
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
                    ((BSTNode)_tree[parentIndex]).RChildPtr = counter;
                    //insert Right
                }
                else
                {
                    ((BSTNode)_tree[parentIndex]).LChildPtr = counter;
                    //insert Left
                }

                _tree[counter++] = new BSTNode(RD);
            }

            true;
        }

        //**************************** PRIVATE METHODS *****************************


    }
}
