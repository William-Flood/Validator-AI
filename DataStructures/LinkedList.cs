using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public class LinkedList <T> : IEnumerable<T>
    {
                                                            // A type to hold a value and 
                                                            // point to another value.
        class ListElement<AlsoT>
        {
            public AlsoT value {get; set;}
            public ListElement <AlsoT> next { get; set; }
            public ListElement(AlsoT value) {
                this.value = value;
            }

            public ListElement(AlsoT value, ListElement<AlsoT> next)
            {
                this.next = next;
                this.value = value;
            }

        }
         
                                                            // Holds the first element,
                                                            // the last element,
                                                            // and the total number of elements.
        class ElementProvider
        {
            public ListElement<T> firstElement;
            public ListElement<T> lastElement;
            public int numberOfElements;

            public ElementProvider()
            {
                numberOfElements = 0;
                lastElement = new ListElement<T>(default(T));
                firstElement = new ListElement<T>(default(T), lastElement);
            }
        }

        ElementProvider elements;

                                                            // The element currently being
                                                            // considered by the linked list.
        ListElement<T> considering;
        ListElement<T> behindConsidering;
        int indexOn;

        public LinkedList()
        {
            elements = new ElementProvider();
            considering = elements.firstElement;
            indexOn = -1;
        }
                                                            // Creates a shallow copy of toCopy.
        public LinkedList(LinkedList<T> toCopy) 
        {
            this.elements = toCopy.elements;
            considering = this.elements.firstElement;
            indexOn = -1;
        }

                                                            // Returns the current index pointed at
                                                            // by this linked list
        public int CurrentIndex {
            get
            {
                return indexOn;
            }
        }

                                                            // Adds a value to the beginning of
                                                            // the linked list.
        public void AddAtStart(T value)
        {
            elements.numberOfElements++;
            elements.firstElement.next = new ListElement<T>(value, elements.firstElement.next);
            if (indexOn >= 0)
            {
                indexOn++;
            }
        }

                                                            // Adds a value to the linked list after
                                                            // the element being considered.
        public void AddAfter(T value)
        {
            elements.numberOfElements++;
            considering.next = new ListElement<T>(value, considering.next);
        }

                                                            // Advances the considered element up by one.
        public void MoveUp()
        {
            if (considering.next != elements.lastElement) 
            { 
                behindConsidering = considering;
                considering = considering.next;
            }
            else
            {
                throw new ArgumentOutOfRangeException("index", "Index out of range!");
            }
                indexOn++;
        }

                                                            // Sets the value being considered
                                                            // to just before index 0;
        public void ResetPointer()
        {
            considering = elements.firstElement;
            behindConsidering = elements.firstElement;
            indexOn = -1;
        }

        // Indicates if invoking moveUp is safe.
        public bool ElementsRemain{
            get {
                return considering.next != elements.lastElement &&
                    considering != elements.lastElement;
            }
        }

                                                            // Returns the number of elements
                                                            // in the linked list.
        public int Length
        { get
            {
                return elements.numberOfElements;
            }
        }

                                                            // Removes the element being considered.
        public void Remove()
        {
            if (considering != elements.lastElement && considering != elements.firstElement)
            {
                elements.numberOfElements--;
                behindConsidering.next = considering.next;
                considering = behindConsidering.next;
            }
        }

                                                            // Returns the value of the element
                                                            // being considered.
        public T Value
        {
            get
            {
                return considering.value;
            }
        }

                                                            // Accesses a given element.
        public T this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index", "Negative index requested!");
                }
                if(indexOn > index)
                {
                    this.ResetPointer();
                }
                
                for (int i = indexOn; i < index; i++)
                {
                    MoveUp();
                }
                return considering.value;
            }
            set
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index", "Negative index requested!");
                }
                if (indexOn > index)
                {
                    this.ResetPointer();
                }

                for (int i = indexOn; i < index; i++)
                {
                    if (!ElementsRemain)
                    {
                        AddAfter(default(T));
                    }
                    MoveUp();
                }
                considering.value = value;
            }

        }

                                                            // Creates a linked list consisting of
                                                            // the combination of elements from
                                                            // two other linked lists.
        public static LinkedList<T> operator + (LinkedList<T> firstList, LinkedList<T> secondList)
        {
            LinkedList<T> returnList = new LinkedList<T>();
            foreach (T listValue in firstList)
            {
                returnList.AddAfter(listValue);
                returnList.MoveUp();
            }
            foreach (T listValue in secondList)
            {
                returnList.AddAfter(listValue);
                returnList.MoveUp();
            }
            returnList.ResetPointer();
            return returnList;
        }

        public LinkedList<T> Slice(int start, int end)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException("start", "Negative index requested!");
            }
            if (end > elements.numberOfElements)
            {
                throw new ArgumentOutOfRangeException("end", "Index too high!");
            }

            LinkedList<T> returnList = new LinkedList<T>();
            if(start < indexOn)
            {
                ResetPointer();
            }
            else
            {
                start -= indexOn;
                end -= indexOn;
            }

            for(int i = 0; i <= start; i++)
            {
                MoveUp();
            }

            for(int i = start; i < end; i++)
            {
                returnList.AddAfter(Value);
                returnList.MoveUp();
                this.MoveUp();
            }

            return returnList;
        }

        public int? IndexOf(T item)
        {
            int? index = 0;
            var scanning = this.elements.firstElement.next;
            while(scanning != this.elements.lastElement)
            {
                if(scanning.value.Equals(item))
                {
                    return index;
                }
                index++;
                scanning = scanning.next;
            }
            return null;
        }

                                                            // Sends an enumerator, allowing
                                                            // a foreach loop to be used.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        
        public IEnumerator<T> GetEnumerator()
        {
            return(IEnumerator<T>) new LinkedListEnum<T>(this);
        }

        public T[] ToArray()
        {
            var results = new T[this.Length];
            var scanElement = this.elements.firstElement.next;
            var index = 0;
            while(scanElement!=this.elements.lastElement)
            {
                results[index] = scanElement.value;
                scanElement = scanElement.next;
                index++;
            }
            return results;
        }
     

    }

                                                            // Provides functionality for
                                                            // a foreach loop.
    public class LinkedListEnum<T> : IEnumerator<T>
    {
        LinkedList<T> listToEnumerate;

        public LinkedListEnum(LinkedList<T> listToEnumerate)
        {
            this.listToEnumerate = listToEnumerate;
            this.listToEnumerate.ResetPointer();
            /*if(this.listToEnumerate.elementsRemain()){
                this.listToEnumerate.moveUp();
            }*/
        }

        public bool MoveNext()
        {
            if (listToEnumerate.ElementsRemain)
            {
                listToEnumerate.MoveUp();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            listToEnumerate.ResetPointer();
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public T Current
        {
            get
            {
                try
                {
                    return listToEnumerate.Value;
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose()
        {

        }
    }
}