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
        public LinkedList<char> TestArray { get; set; }

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
            this.TestArray = new LinkedList<char>();
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
                        placement = rng.Next(TestArray.Length);
                        if (placement > TestArray.CurrentIndex)
                        {
                            TestArray.ResetPointer();
                        }
                        else
                        {
                            placement -= TestArray.CurrentIndex;
                        }
                        for (int i = 0; i < placement; i++)
                        {
                            TestArray.MoveUp();
                        }
                        if(Char.IsLetter(character))
                        {
                            TestArray.AddAfter(Char.ToLower(character));
                        }
                        else
                        {
                            TestArray.AddAfter(character);
                        }
                    }
                }
                TestArray.ResetPointer();
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
                        TestArray.AddAfter(character);
                    }
                    TestArray.MoveUp();
                }
                _isReal = false;
            }
            else
            {
                foreach (char character in testString)
                {
                    if (Char.IsLetter(character))
                    {
                        TestArray.AddAfter(Char.ToLower(character));
                    }
                    else
                    {
                        TestArray.AddAfter(character);
                    }
                    TestArray.MoveUp();
                }
                _isReal = true;
            }
            TestArray.ResetPointer();
        }

                                                            // Produces a copy of an instance
        public TestAgainst(TestAgainst toCopy)
        {
            this.TestArray = new LinkedList<char>(toCopy.TestArray);
            this._isReal = toCopy._isReal;
        }

                                                            // Returns the next character in testArray
        public char getNext()
        {
            TestArray.MoveUp();
            return TestArray.Value;
        }

                                                            // Returns the current character in testArray
        public char value()
        {
            return TestArray.Value;
        }

                                                            // Indicates if there are any more characters
                                                            // which could be sent.
        public bool charsRemain()
        {
            return TestArray.ElementsRemain;
        }

                                                            // Resets testArray.
        public void resetPointer()
        {
            TestArray.ResetPointer();
        }
    }
}
