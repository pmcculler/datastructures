/* The MIT License (MIT)

Copyright (c) 2015 Patrick McCuller

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public class CuckooHash<TKey, TValue> : IDictionary<TKey, TValue>
    {
        // ReSharper disable once InconsistentNaming
        private const int DEFAULT_TABLE_SIZE = 1024;
        private const int DEFAULT_SLOT_WIDTH = 7;
        private const int DEFAULT_NUMBER_OF_TABLES = 3;
        private const int MAX_CHAIN_DEPTH = 1024;
        private const double LOAD_FACTOR = 0.75;

        private readonly Random _random = new Random();

        protected HashBucket<TKey, TValue>[][] Tables;
        protected Func<TKey, int, int>[] HashFunctions;
        private int _count;
        private readonly int _bucketWidth;
        private readonly int _numberOfTables;
        private int _sizeOfTables;

        // Usually for tests.
        public int StorageSlots => _sizeOfTables;

        [Serializable]
        public class KeyInsertFailedException : Exception
        {
            public KeyInsertFailedException(string errorMessage) : base(errorMessage)
            {
            }
        }

        protected struct HashSlot<TKey2, TValue2>
        {
            public TKey2 Key;
            public TValue2 Value;

            public HashSlot(TKey2 key, TValue2 value)
            {
                Key = key;
                Value = value;
            }
        }

        protected class HashBucket<TKey3, TValue3> : IEnumerable
        {
            private long _data;
            //private HashSlot<TKey3, TValue3>[] Store;
            private const string LOCK_REQUEST_OUT_OF_BOUNDS_MESSAGE = "Lock request for slot out of bounds.";
            private const string USE_REQUEST_OUT_OF_BOUNDS_MESSAGE = "Allocation or free request for slot out of bounds.";
            private const int USE_OFFSET = 8;

            public int Length => DEFAULT_SLOT_WIDTH;

            public bool Lock(int slot)
            {
                Preconditions.InRange(0, true, slot, DEFAULT_SLOT_WIDTH, false, LOCK_REQUEST_OUT_OF_BOUNDS_MESSAGE);
                if (BitIsSet(slot))
                {
                    SetBit(slot, true);
                    return true;
                }
                return false;
            }

            public void Unlock(int slot)
            {
                Preconditions.InRange(0, true, slot, DEFAULT_SLOT_WIDTH, false, LOCK_REQUEST_OUT_OF_BOUNDS_MESSAGE);
                SetBit(slot, false);
            }

            private void Use(int slot)
            {
                Preconditions.InRange(0, true, slot, DEFAULT_SLOT_WIDTH, false, USE_REQUEST_OUT_OF_BOUNDS_MESSAGE);
                SetBit(slot + USE_OFFSET, true);
            }

            public bool Used(int slot)
            {
                Preconditions.InRange(0, true, slot, DEFAULT_SLOT_WIDTH, false, USE_REQUEST_OUT_OF_BOUNDS_MESSAGE);
                return !BitIsSet(slot + USE_OFFSET);
            }

            private void Free(int slot)
            {
                Preconditions.InRange(0, true, slot, DEFAULT_SLOT_WIDTH, false, USE_REQUEST_OUT_OF_BOUNDS_MESSAGE);
                SetBit(slot + USE_OFFSET, false);
            }

            private bool BitIsSet(int bit)
            {
                return ((1 << bit) & _data) == 0;
            }

            private void SetBit(int bit, bool on)
            {
                if (on)
                {
                    _data |= 1 << bit;
                }
                else
                {
                    _data ^= 1 << bit;
                }
            }

            public HashBucket()
            {
                //Store = new HashSlot<TKey3, TValue3>[DEFAULT_SLOT_WIDTH]; // TODO can't be changed
                _data = 0;
            }

            private HashSlot<TKey3, TValue3> _slot0;
            private HashSlot<TKey3, TValue3> _slot1;
            private HashSlot<TKey3, TValue3> _slot2;
            private HashSlot<TKey3, TValue3> _slot3;
            private HashSlot<TKey3, TValue3> _slot4;
            private HashSlot<TKey3, TValue3> _slot5;
            private HashSlot<TKey3, TValue3> _slot6;

            public void ClearSlot(int index)
            {

                switch (index)
                {
                    case 0:
                        _slot0.Key = default(TKey3);
                        _slot0.Value = default(TValue3);
                        Free(0);
                        return;
                    case 1:
                        _slot1.Key = default(TKey3);
                        _slot1.Value = default(TValue3);
                        Free(1);
                        return;                                           
                    case 2:
                        _slot2.Key = default(TKey3);
                        _slot2.Value = default(TValue3);
                        Free(2);
                        return;
                    case 3:
                        _slot3.Key = default(TKey3);
                        _slot3.Value = default(TValue3);
                        Free(3);
                        return;
                    case 4:
                        _slot4.Key = default(TKey3);
                        _slot4.Value = default(TValue3);
                        Free(4);
                        return;
                    case 5:
                        _slot5.Key = default(TKey3);
                        _slot5.Value = default(TValue3);
                        Free(5);
                        return;
                    case 6:
                        _slot6.Key = default(TKey3);
                        _slot6.Value = default(TValue3);
                        Free(6);
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            public HashSlot<TKey3, TValue3> this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return _slot0;
                        case 1:
                            return _slot1;
                        case 2:
                            return _slot2;
                        case 3:
                            return _slot3;
                        case 4:
                            return _slot4;
                        case 5:
                            return _slot5;
                        case 6:
                            return _slot6;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                set
                {
                    switch (index)
                    {
                        case 0:
                            _slot0 = value;
                            Use(0);
                            return;
                        case 1:
                            _slot1 = value;
                            Use(1);
                            return;
                        case 2:
                            _slot2 = value;
                            Use(2);
                            return;
                        case 3:
                            _slot3 = value;
                            Use(3);
                            return;
                        case 4:
                            _slot4 = value;
                            Use(4);
                            return;
                        case 5:
                            _slot5 = value;
                            Use(5);
                            return;
                        case 6:
                            _slot6 = value;
                            Use(6);
                            return;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                yield return _slot0;
                yield return _slot1;
                yield return _slot2;
                yield return _slot3;
                yield return _slot4;
                yield return _slot5;
                yield return _slot6;
            }

            public IList<HashSlot<TKey3, TValue3>> ToList()
            {
                var list = new List<HashSlot<TKey3, TValue3>>();
                for (int i = 0; i < Length; i++)
                {
                    if (Used(i))
                    {
                        list.Add(this[i]);
                    }
                }
                return list;
            }
        }

        public CuckooHash() : this(DEFAULT_TABLE_SIZE)
        {
        }

        public CuckooHash(int initialSize, int bucketWidth = DEFAULT_SLOT_WIDTH)
        {
            _bucketWidth = bucketWidth;
            _sizeOfTables = initialSize;
            _numberOfTables = DEFAULT_NUMBER_OF_TABLES; // TODO
            Tables = CreateInitializedBuckets();
            InitializeHashFunctions();
        }

        protected HashBucket<TKey, TValue>[][] CreateInitializedBuckets(int tableSize = -1)
        {
            if (tableSize == -1)
            {
                tableSize = _sizeOfTables;
            }
            HashBucket<TKey, TValue>[][] newTables = new HashBucket<TKey, TValue>[_numberOfTables][];
            for (int i = 0; i < newTables.GetLength(0); i++)
            {
                newTables[i] = new HashBucket<TKey, TValue>[tableSize];
            }
            return newTables;
        }

        protected void InitializeHashFunctions()
        {
            // for the moment, just use the three we have defined.
            // TODO: automatically mutate functions to create however many different hash functions we need.
            HashFunctions = new Func<TKey, int, int>[DEFAULT_NUMBER_OF_TABLES];
            HashFunctions[0] = Hashes<TKey>.Hash;
            HashFunctions[1] = Hashes<TKey>.Hash2;
            HashFunctions[2] = Hashes<TKey>.Hash3;
        }

        protected int CorrectModulus(int modulus)
        {
            if (modulus != -1)
            {
                return modulus;
            }
            return _sizeOfTables;
        }

        protected TValue CheckAndReturn(HashBucket<TKey, TValue>[] table, int index, TKey key, out bool success)
        {
            HashBucket<TKey, TValue> bucket = table[index];
            if (bucket != null)
            {
                foreach (HashSlot<TKey, TValue> slot in bucket)
                {
                    if (key.Equals(slot.Key))
                    {
                        success = true;
                        return slot.Value;
                    }
                }
            }
            success = false;
            return default(TValue);
        }

        protected int FindEmptySlot(HashBucket<TKey, TValue> bucket)
        {
            for (int i = 0; i < bucket.Length; i++)
            {
                if (!bucket.Used(i) && bucket.Lock(i))
                {
                    return i;
                }
            }
            return -1;
        }

        protected bool Set(HashBucket<TKey, TValue>[][] tables, TKey key, TValue value)
        {
            while (true)
            {
                try
                {
                    SetAndEvict(tables, key, value);
                    return true;
                }
                catch (KeyInsertFailedException)
                {
                    EnsureAppropriateStorage(forceIncrease: true);
                    Set(tables, key, value);
                }
            }
        }

        protected void SetAndEvict(HashBucket<TKey, TValue>[][] tables, TKey key, TValue value, int depth = 0, int tableToSkip = -1)
        {
            if (depth >= MAX_CHAIN_DEPTH)
            {
                throw new KeyInsertFailedException("Maximum search chain depth reached.");
            }
            int tableNumber = PickTableNumberExcept(tableToSkip);
            HashBucket<TKey, TValue>[] table = tables[tableNumber];
            int bucketIndex = HashFunctions[tableNumber](key, table.Length);
            var bucket = table[bucketIndex];
            if (bucket == null)
            {
                bucket = new HashBucket<TKey, TValue>();
                table[bucketIndex] = bucket;
            }
            var newSlot = new HashSlot<TKey, TValue>(key, value);
            int targetSlotNumber = FindEmptySlot(bucket);
            if (targetSlotNumber < 0)
            {
                // No empty slots. Time to evict someone.
                targetSlotNumber = PickSlotNumber(bucket);
                if (targetSlotNumber < 0)
                {
                    throw new KeyInsertFailedException("Fully locked bucket found. Cannot insert value.");
                }
                SetAndEvict(tables, bucket[targetSlotNumber].Key, bucket[targetSlotNumber].Value, depth + 1, tableNumber);
            }
            bucket[targetSlotNumber] = newSlot;
            bucket.Unlock(targetSlotNumber);
        }

        private int PickTableNumberExcept(int tableToSkip)
        {
            while (true)
            {
                int tableNumber = _random.Next(Tables.GetLength(0));
                if (tableNumber != tableToSkip)
                {
                    return tableNumber;
                }
            }
        }

        // Returns -1 if all slots are locked
        private int PickSlotNumber(HashBucket<TKey, TValue> bucket)
        {
            int slotNumber = _random.Next(_bucketWidth);
            if (bucket.Lock(slotNumber))
            {
                return slotNumber;
            }
            for (int i = 0; i < DEFAULT_SLOT_WIDTH; i++)
            {
                if (i == slotNumber)
                {
                    continue;
                }
                if (bucket.Lock(i))
                {
                    return i;
                }
            }
            return -1;
        }

        private TValue TryGet(TKey key, out bool success)
        {
            for (int i = 0; i < _numberOfTables; i++)
            {
                TValue found = CheckAndReturn(Tables[i], HashFunctions[i](key, _sizeOfTables), key, out success);
                if (success)
                {
                    return found;
                }
            }
            success = false;
            return default(TValue);
        }

        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException();
                }

                bool success;
                TValue found = TryGet(key, out success);
                if (success)
                {
                    return found;
                }
                throw new KeyNotFoundException();
            }

            // It is safe to insert and read at the same time, because the eviction chain
            // is done by backtracking from the last eviction to the first, overwriting
            // a slot only when its evicted key has already been written at its destination.
            set
            {
                EnsureAppropriateStorage();
                if (key == null)
                {
                    throw new ArgumentNullException();
                }
                if (ContainsKey(key))
                {
                    // Note: this check is suggested by original cuckoo hash paper.
                    // Keeps us from putting the same key into multiple tables.
                    Remove(key);
                }
                Set(Tables, key, value);
                _count++;
            }
        }

        private void EnsureAppropriateStorage(bool forceIncrease = false)
        {
            bool tooManySlots = LoadFactorIsTooLow();
            bool tooFewSlots = forceIncrease || LoadFactorIsTooHigh();

            if (!tooManySlots && !tooFewSlots)
            {
                return;
            }
            if (tooFewSlots)
            {
                ResizeTables(_sizeOfTables * 2);
            }
            else
            {
                ResizeTables(_sizeOfTables / 2);
            }
        }

        // Note: may choose a larger table size if it can't hash all values into this size.
        // Trying to shrink the table could fail, for instance.
        private void ResizeTables(int targetTableSize)
        {
            HashBucket<TKey, TValue>[][] newBuckets;
            while (true)
            {
                _sizeOfTables = targetTableSize;
                newBuckets = CreateInitializedBuckets(targetTableSize);
                try
                {
                    foreach (var table in Tables)
                    {
                        foreach (var bucket in table)
                        {
                            if (bucket == null)
                            {
                                continue;
                            }
                            int i = 0;
                            foreach (HashSlot<TKey, TValue> slot in bucket)
                            {
                                if (!bucket.Used(i++))
                                {
                                    continue;
                                }
                                SetAndEvict(newBuckets, slot.Key, slot.Value);
                            }
                        }
                    }
                    break;
                }
                catch (KeyInsertFailedException)
                {
                    targetTableSize *= 2;
                }
            }
            Tables = newBuckets;
        }

        protected bool LoadFactorIsTooHigh()
        {
            return (_count > NumberOfSlots() * LOAD_FACTOR);
        }

        protected bool LoadFactorIsTooLow()
        {
            int load = _count;
            if (load <= DefaultNumberOfSlots())
            {
                return false;
            }
            return (load < (NumberOfSlots() * LOAD_FACTOR) / 2);
        }

        private static int DefaultNumberOfSlots()
        {
            return DEFAULT_NUMBER_OF_TABLES * DEFAULT_SLOT_WIDTH * DEFAULT_TABLE_SIZE;
        }

        protected int NumberOfSlots()
        {
            return _bucketWidth * _sizeOfTables * _numberOfTables;
        }

        public int Count => _count;

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return
                    Tables.SelectMany(
                        table =>
                            table.SelectMany(
                                bucket =>
                                {
                                    return bucket?.ToList()
                                               .Select(slot => slot.Key)
                                               .Where(slot => slot != null) ?? new List<TKey>();
                                })).ToList();
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return Tables.SelectMany(
                    table =>
                        table.SelectMany(
                            bucket =>
                            {
                                return bucket?.ToList()
                                    .Select(slot => slot.Value)
                                    .Where(slot => slot != null) ?? new List<TValue>();
                            })).ToList();
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this[item.Key] = item.Value;
        }

        public void Add(TKey key, TValue value)
        {
            this[key] = value;
        }

        public void Clear()
        {
            Tables = CreateInitializedBuckets();
            _count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            for (int i = 0; i < Tables.GetLength(0); i++)
            {
                int bucketIndex = HashFunctions[i](item.Key, _sizeOfTables);
                if (Tables[i][bucketIndex].ToList().Any(
                    slot => slot.Key != null && slot.Key.Equals(item.Key) && slot.Value.Equals(item.Value)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            for (int i = 0; i < Tables.GetLength(0); i++)
            {
                int bucketIndex = HashFunctions[i](key, _sizeOfTables);
                if (Tables[i][bucketIndex] != null)
                {
                    if (Tables[i][bucketIndex].ToList().Any(
                        slot => slot.Key != null && slot.Key.Equals(key)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            int i = 0;
            foreach (KeyValuePair<TKey, TValue> kvp in this)
            {
                array[arrayIndex + i++] = kvp;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (TKey key in Keys)
            {
                yield return new KeyValuePair<TKey, TValue>(key, this[key]);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (RemoveOnMatch(item.Key, item.Value, true))
            {
                return true;
            }
            return false;
        }

        private bool RemoveOnMatch(TKey key, TValue value, bool matchOnValue)
        {
            for (int i = 0; i < Tables.GetLength(0); i++)
            {
                int bucketIndex = HashFunctions[i](key, _sizeOfTables);
                if (Tables[i][bucketIndex] != null)
                {
                    for (int j = 0; j < Tables[i][bucketIndex].Length; j++)
                    {
                        var slot = Tables[i][bucketIndex][j];
                        if (Tables[i][bucketIndex].Used(j) && slot.Key != null && slot.Key.Equals(key))
                        {
                            if (!matchOnValue || slot.Value.Equals(value))
                            {
                                Tables[i][bucketIndex].ClearSlot(j);
                                _count--;
                                EnsureAppropriateStorage();
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool Remove(TKey key)
        {
            if (RemoveOnMatch(key, default(TValue), false))
            {
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            bool success;
            value = TryGet(key, out success);
            return success;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (TKey key in Keys)
            {
                yield return new KeyValuePair<TKey, TValue>(key, this[key]);
            }
        }
    }
}
