using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
                                                            // Contains a key-value pair, as well
                                                            // as information to navigate and
                                                            // manage a binary search tree.
    class TreeElement <Key, Value>
    {
                                                            // A key used to locate a given value.
        public Key key { get; set; }

                                                            // A value stored in the binary
                                                            // search tree.
        public Value value { get; set; }

                                                            // The number of elements on the 
                                                            // left branch of this element.
        public int onLeft { get; set; }

                                                            // Indicates if any elements
                                                            // exist on the left branch of
                                                            // this element.
        public bool hasLeft { get; set; }

                                                            // Indicates if any elements
                                                            // exist on the right branch
                                                            // of this element.
        public bool hasRight { get; set; }

                                                            // The first element on the left
                                                            // branch of this element.
        TreeElement<Key, Value> leftElement;

                                                            // The first element on the left
                                                            // branch of this element.
        TreeElement<Key, Value> rightElement;

                                                            // Allows access to the left element.
        public TreeElement<Key, Value> leftBranch {
            get
            {
                return leftElement;
            }
            set
            {
                leftElement = value;
                hasLeft = true;
            }
        }

                                                            // Allows access to the right element.
        public TreeElement<Key, Value> rightBranch
        {
            get
            {
                return rightElement;
            }
            set
            {
                rightElement = value;
                hasRight = true;
            }
        }
        
        public TreeElement(Key key, Value value)
        {
            this.key = key;
            this.value = value;
            onLeft = 0;
            hasLeft = false;
            hasRight = false;

        }
    }

                                                            // Contains information about a
                                                            // range of indexes in a data
                                                            // structure.  Used to build
                                                            // optimized binary search
                                                            // trees.
    class TreeConstructor
    {
        public int index, minRange, maxRange;
        public TreeConstructor leftBranch { get; set; }
        public TreeConstructor rightBranch { get; set; }
        public TreeConstructor nextBranch { get; set; }
        public bool hasNext { get; set; }

        public TreeConstructor(int minRange, int maxRange)
        {
            this.minRange = minRange;
            this.maxRange = maxRange;
            this.index = (maxRange - minRange) / 2 + minRange;
            hasNext = false;
        }

                                                            // Generates more constructor elements.
        public void bud()
        {
            leftBranch = new TreeConstructor(minRange, index - 1);
            rightBranch = new TreeConstructor(index + 1, maxRange);
            leftBranch.leadTo(rightBranch);
        }

                                                            // Determines whether more constructor 
                                                            // elements can be grown from this one.
        public bool atEnd()
        {
            return minRange >= maxRange;
        }

                                                            // Determines if the element can be used 
                                                            // to generate a new element in the tree.
        public bool valid()
        {
            return minRange <= maxRange;
        }

                                                            // Allows the element to point
                                                            // to the next element to grow from.
        public void leadTo(TreeConstructor next)
        {
            hasNext = true;
            nextBranch = next;
        }
    }



    public class BinarySearchTree <Key, Value>
    {
        int numberOfElements;
        TreeElement<Key, Value> firstElement;
        int indexRequestedLast;
        TreeElement<Key, Value> foundLast;
        public delegate int compareDelegate(Key keyLeft, Key keyRight);
        public delegate Value getNewValue(int index);
        public delegate Key getNewKey(int index);
        compareDelegate compare;

        public BinarySearchTree(compareDelegate compare)
        {
            numberOfElements = 0;
            this.compare = compare;
        }

                                                            // Determines if a given key exists in the tree.
        public bool has(Key key)
        {
            if(numberOfElements == 0)
            {
                return false;
            }
            bool latch = true;
            TreeElement<Key, Value> scanning = firstElement;

                                                            // Searches the tree until a match for the
                                                            // given key is found
            while (latch)
            {
                if(0 == compare(scanning.key, key))
                {
                    foundLast = scanning;
                    return true;
                }
                else if(0 < compare(scanning.key, key))
                {
                    if (scanning.hasLeft)
                    {
                        scanning = scanning.leftBranch;
                    }
                    else
                    {
                        latch = false;
                        foundLast = scanning;
                    }
                }
                else
                {
                    if (scanning.hasRight)
                    {
                        scanning = scanning.rightBranch;
                    }
                    else
                    {
                        latch = false;
                        foundLast = scanning;
                    }
                }
            }
            return false;
        }

                                                            // Retrieves the value of the
                                                            // element with the specified key.
        public Value search (Key key)
        {
                                                            // Returns the value of the previously located
                                                            // element if its key matched the search key
            if (foundLast != null && 0 == compare(foundLast.key, key))
            {
                return foundLast.value;
            }
            else
            {
                bool latch = true;
                TreeElement<Key, Value> scanning = firstElement;
                while (latch)
                {
                    if (0 == compare(scanning.key, key))
                    {
                        foundLast = scanning;
                        return foundLast.value;
                    }
                    else if (0 < compare(scanning.key, key))
                    {
                        if (scanning.hasLeft)
                        {
                            scanning = scanning.leftBranch;
                        }
                        else
                        {
                            latch = false;
                        }
                    }
                    else
                    {
                        if (scanning.hasRight)
                        {
                            scanning = scanning.rightBranch;
                        }
                        else
                        {
                            latch = false;
                        }
                    }
                }
                throw new ArgumentOutOfRangeException("key", "Key not found!");
            }
                
        }

                                                            // Adds an element with the specified
                                                            // key-value pair to the tree
        public void add(Key key, Value value)
        {
                                                            // No element has a negative index - this
                                                            // effectively reset indexRequestedLast
            indexRequestedLast = -1;

                                                            // If no elements have been added to the
                                                            // binary search tree, the key-value pair
                                                            // is added as the first element
            if (0 == numberOfElements)
            {
                firstElement = new TreeElement<Key, Value>(key, value);
                numberOfElements++;
            }

                                                            // If the given key matches the key of the
                                                            // previously located element, its value
                                                            // is simply added to the binary search
                                                            // tree
            else if (null != foundLast && 0 == compare(foundLast.key, key))
            {
                foundLast.value = value;
            }

                                                            // If the given key already exists in the binary
                                                            // search tree, its value is set to the
                                                            // given value
            else if (has(key))
            {
                    foundLast.value = value;

            }
            else
            {
                bool latch = true;
                TreeElement<Key, Value> scanning = firstElement;
                while (latch)
                {
                    if (0 < compare(scanning.key, key))
                    {
                        scanning.onLeft++;
                        if (scanning.hasLeft)
                        {
                            scanning = scanning.leftBranch;
                        }
                        else
                        {
                            scanning.leftBranch = new TreeElement<Key, Value>(key, value);
                            latch = false;
                        }
                    }
                    else
                    {
                        if (scanning.hasRight)
                        {
                            scanning = scanning.rightBranch;
                        }
                        else
                        {
                            scanning.rightBranch = new TreeElement<Key, Value>(key, value);
                            latch = false;
                        }

                    }
                }
                numberOfElements++;
            }
        }

                                                            // Removes an element with a given
                                                            // key value from the tree
        public void prune(Key key)
        {
                                                            // If the given key does not exist in
                                                            // the binary search tree, no action is
                                                            // needed.
            if (has(key))
            {
                bool latch = true;
                TreeElement<Key, Value> keyElement = firstElement;
                TreeElement<Key, Value> parentOfKey = firstElement;
                bool keyOnRight = false;
                                                            // Loops until an element with the given 
                                                            // key value is found.  Keeps track of
                                                            // the element this element is immediately
                                                            // below
                while (latch)
                {
                    if (0 == compare(keyElement.key, key))
                    {
                        latch = false;
                    }
                    else if (0 < compare(keyElement.key, key))
                    {
                        if (keyElement.hasLeft)
                        {
                            parentOfKey = keyElement;
                            keyOnRight = false;
                            keyElement.onLeft--;
                            keyElement = keyElement.leftBranch;
                        }
                        else
                        {
                            latch = false;
                        }
                    }
                    else
                    {
                        if (keyElement.hasRight)
                        {
                            parentOfKey = keyElement;
                            keyOnRight = true;
                            keyElement = keyElement.rightBranch;
                        }
                        else
                        {
                            latch = false;
                        }
                    }
                }

                if(0 == compare(keyElement.key, key))
                {
                    numberOfElements--;
                    if (keyElement.hasRight)
                    {
                        TreeElement<Key, Value> swapKeyFor = keyElement.rightBranch;
                        latch = true;
                                                            // Finds the element with the lowest
                                                            // key on the right branch of the
                                                            // binary search tree
                        while (swapKeyFor.hasLeft)
                        {
                            swapKeyFor.onLeft--;
                            if (false == swapKeyFor.leftBranch.hasLeft)
                            {
                                                            // The current element referenced by 
                                                            // swapKeyFor will have its left element
                                                            // removed.
                                swapKeyFor.hasLeft = false;
                            }
                            swapKeyFor = swapKeyFor.leftBranch;
                        }

                        TreeElement<Key, Value> pushRightTo = swapKeyFor;
                                                            // Finds the element with the highest
                                                            // key on the right branch of swapKeyFor;
                                                            // the right element of the element to 
                                                            // be removed will be assigned to this
                                                            // element.
                        while (pushRightTo.hasRight)
                        {
                            pushRightTo = pushRightTo.rightBranch;
                        }
                        pushRightTo.rightBranch = keyElement.rightBranch;
                        swapKeyFor.onLeft = keyElement.onLeft;
                        swapKeyFor.leftBranch = keyElement.leftBranch;
                        if (0 == compare(parentOfKey.key, keyElement.key))
                        {
                            firstElement = swapKeyFor;
                        }
                        else if (keyOnRight)
                        {
                            parentOfKey.rightBranch = swapKeyFor;
                        }
                        else
                        {
                            parentOfKey.leftBranch = swapKeyFor;
                        }
                    }
                    else if (keyElement.hasLeft)
                    {
                        parentOfKey.leftBranch = keyElement.leftBranch;
                    }
                    else
                    {
                        parentOfKey.hasLeft = false;
                    }
                }
            }
            
        }

        public int getSize()
        {
            return numberOfElements;
        }

                                                            //Returns a key located at a certain index
        public Key this[int index]
        {
            get
            {

                return this.elementAt(index).key;
            }
                                                            // Use add(this[index], valueToAssign) to
                                                            // assign a new value to a the element at
                                                            // a given index, assuming this index exists.
        }

                                                            //Returns an element at a certain index
        TreeElement<Key, Value> elementAt(int index)
        {
            bool latch;
            TreeElement<Key, Value> scanning;
            int adjust = 0;
            if(index == indexRequestedLast)
            {
                scanning = foundLast;
                latch = false;
            }
            else
            {
                scanning = firstElement;
                latch = true;
            }
            scanning = firstElement;
            latch = true;
            while (latch)
            {
                if (scanning.onLeft + adjust == index)
                {
                    latch = false;

                }
                else if (scanning.onLeft + adjust > index)
                {
                    if (scanning.leftBranch != null)
                    {
                        scanning = scanning.leftBranch;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("index", "Index not found!");
                    }
                }
                else
                {
                    if (scanning.rightBranch != null)
                    {
                        adjust += scanning.onLeft + 1;
                        scanning = scanning.rightBranch;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("index", "Index not found!");
                    }
                }
            }
            indexRequestedLast = index;
            foundLast = scanning;
            return scanning;
        }


                                                            //Generates a binary search tree from
                                                            // data made available elsewhere
        public BinarySearchTree(int size, getNewKey keyGetter, getNewValue valueGetter, compareDelegate compare)
        {
            this.compare = compare;
            bool latch = true;
            TreeConstructor firstInChain = new TreeConstructor(0, size - 1);
            TreeConstructor workingBranch;
            TreeConstructor previousBranch;
            add(keyGetter(firstInChain.index),valueGetter(firstInChain.index));
            while(latch)
            {
                workingBranch = firstInChain;
                workingBranch.bud();
                if(false == workingBranch.leftBranch.atEnd())
                {
                    add(keyGetter(workingBranch.leftBranch.index), 
                        valueGetter(workingBranch.leftBranch.index));
                }
                else
                {
                    latch = false;
                    if(workingBranch.leftBranch.valid())
                    {
                        add(keyGetter(workingBranch.leftBranch.index),
                            valueGetter(workingBranch.leftBranch.index));
                    }
                }
                if (false == workingBranch.rightBranch.atEnd())
                {
                    latch = true;
                    add(keyGetter(workingBranch.rightBranch.index),
                        valueGetter(workingBranch.rightBranch.index));
                }
                else
                {
                    if (workingBranch.rightBranch.valid())
                    {
                        add(keyGetter(workingBranch.rightBranch.index),
                            valueGetter(workingBranch.rightBranch.index));
                    }
                }
                while(workingBranch.hasNext)
                {
                    previousBranch = workingBranch;
                    workingBranch = workingBranch.nextBranch;
                    workingBranch.bud();
                    previousBranch.rightBranch.leadTo(workingBranch.leftBranch);
                    if (false == workingBranch.leftBranch.atEnd())
                    {
                        latch = true;
                        add(keyGetter(workingBranch.leftBranch.index),
                            valueGetter(workingBranch.leftBranch.index));
                    }
                    else
                    {
                        if (workingBranch.leftBranch.valid())
                        {
                            add(keyGetter(workingBranch.leftBranch.index),
                                valueGetter(workingBranch.leftBranch.index));
                        }
                    }
                    if (false == workingBranch.rightBranch.atEnd())
                    {
                        latch = true;
                        add(keyGetter(workingBranch.rightBranch.index),
                            valueGetter(workingBranch.rightBranch.index));
                    }
                    else
                    {
                        if (workingBranch.rightBranch.valid())
                        {
                            add(keyGetter(workingBranch.rightBranch.index),
                                valueGetter(workingBranch.rightBranch.index));
                        }
                    }
                }
                firstInChain = firstInChain.leftBranch;
            }

        }
        

                                                            //Returns a value at a given index
        public Value valueAt(int index)
        {
            return this.elementAt(index).value;
        }

                                                            // Produces an optimized version of
                                                            // an existing binary search tree
        public BinarySearchTree(BinarySearchTree<Key, Value> toOptimize) : 
        this(toOptimize.numberOfElements,
                i => toOptimize[i],
                i => toOptimize.valueAt(i),
                toOptimize.compare){
        }

        public int IndexOf(Key toSearchfor)
        {
            var result = 0;

            bool latch = true;
            TreeElement<Key, Value> scanning = firstElement;
            while (latch)
            {
                if (0 == compare(scanning.key, toSearchfor))
                {
                    result = result + scanning.onLeft;
                    foundLast = scanning;
                    return result;
                }
                else if (0 < compare(scanning.key, toSearchfor))
                {
                    if (scanning.hasLeft)
                    {
                        scanning = scanning.leftBranch;
                    }
                    else
                    {
                        latch = false;
                    }
                }
                else
                {
                    if (scanning.hasRight)
                    {
                        result = result + scanning.onLeft;
                        scanning = scanning.rightBranch;
                    }
                    else
                    {
                        latch = false;
                    }
                }
            }
            throw new ArgumentOutOfRangeException("key", "Key not found!");
        }

    }
}
