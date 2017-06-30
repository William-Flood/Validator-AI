using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace Logic
{
                                                            // Holds a linked list of characters,
                                                            // as well as a boolean describing whether 
                                                            // the list represents a line read from a
                                                            // file or a scrambled version
    class TestAgainst
    {
        LinkedList<char> testArray;

                                                            // Indicates if the string has been scrambled
        bool _isReal;
        public bool isReal
        {
            get
            {
                return _isReal;
            }
        }

        
        public TestAgainst(String testString)
        {
            Random rng = new Random();
            this.testArray = new LinkedList<char>();
            bool swapLetter;
            bool addLetter;
            if(0 == rng.Next(2))
            {
                int placement;
                foreach(char character in testString)
                {
                    switch (character)
                    {
                        case ' ':
                        case '.':
                        case ',':
                            swapLetter = false;
                            break;
                        default:
                            swapLetter = true;
                            break;
                    }
                    if(swapLetter)
                    {
                        placement = rng.Next(testArray.Length);
                        if (placement > testArray.CurrentIndex)
                        {
                            testArray.ResetPointer();
                        }
                        else
                        {
                            placement -= testArray.CurrentIndex;
                        }
                        for (int i = 0; i < placement; i++)
                        {
                            testArray.MoveUp();
                        }
                        if(Char.IsLetter(character))
                        {
                            testArray.AddAfter(Char.ToLower(character));
                        }
                        else
                        {
                            testArray.AddAfter(character);
                        }
                    }
                }
                testArray.ResetPointer();
                foreach(char character in testString)
                {
                    switch (character)
                    {
                        case ' ':
                        case '.':
                        case ',':
                            addLetter = true;
                            break;
                        default:
                            addLetter = false;
                            break;
                    }
                    if(addLetter)
                    {
                        testArray.AddAfter(character);
                    }
                    testArray.MoveUp();
                }
                _isReal = false;
            }
            else
            {
                foreach (char character in testString)
                {
                    if (Char.IsLetter(character))
                    {
                        testArray.AddAfter(Char.ToLower(character));
                    }
                    else
                    {
                        testArray.AddAfter(character);
                    }
                    testArray.MoveUp();
                }
                _isReal = true;
            }
            testArray.ResetPointer();
        }

                                                            // Produces a copy of an instance
        public TestAgainst(TestAgainst toCopy)
        {
            this.testArray = new LinkedList<char>(toCopy.testArray);
            this._isReal = toCopy._isReal;
        }

                                                            // Returns the next character in testArray
        public char getNext()
        {
            testArray.MoveUp();
            return testArray.Value;
        }

                                                            // Returns the current character in testArray
        public char value()
        {
            return testArray.Value;
        }

                                                            // Indicates if there are any more characters
                                                            // which could be sent.
        public bool charsRemain()
        {
            return testArray.ElementsRemain;
        }

                                                            // Resets testArray.
        public void resetPointer()
        {
            testArray.ResetPointer();
        }
    }
}
