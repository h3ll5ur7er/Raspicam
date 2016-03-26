using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;

namespace MVVMBase.Interactivity
{
    public class FreezableCollection<T> : Animatable, IList, IList<T>, INotifyCollectionChanged, INotifyPropertyChanged where T : DependencyObject
    {
        private readonly SimpleMonitor _monitor = new SimpleMonitor();
        internal List<T> _collection;
        internal uint _version;
        private const string CountPropertyName = "Count";
        private const string IndexerPropertyName = "Item[]";

        public T this[int index]
        {
            get
            {
                ReadPreamble();
                return _collection[index];
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("Collection_NoNull");
                CheckReentrancy();
                WritePreamble();
                var oldValue = _collection[index];
                var num = (object) oldValue != value as object ? 1 : 0;
                if (num != 0)
                {
                    OnFreezablePropertyChanged(oldValue, value);
                    _collection[index] = value;
                }
                _version = _version + 1U;
                WritePostscript();
                if (num == 0)
                    return;
                OnCollectionChanged(NotifyCollectionChangedAction.Replace, index, oldValue, index, value);
            }
        }
        
        public int Count
        {
            get
            {
                ReadPreamble();
                return _collection.Count;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                ReadPreamble();
                return IsFrozen;
            }
        }

        bool IList.IsReadOnly => ((ICollection<T>)this).IsReadOnly;

        bool IList.IsFixedSize
        {
            get
            {
                ReadPreamble();
                return IsFrozen;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = Cast(value);
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                ReadPreamble();
                if (!IsFrozen)
                    return Dispatcher != null;
                return true;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                ReadPreamble();
                return this;
            }
        }

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                CollectionChanged += value;
            }
            remove
            {
                CollectionChanged -= value;
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PrivatePropertyChanged += value;
            }
            remove
            {
                PrivatePropertyChanged -= value;
            }
        }

        private event PropertyChangedEventHandler PrivatePropertyChanged;

        protected FreezableCollection()
        {
            _collection = new List<T>();
        }
        
        public FreezableCollection(int capacity)
        {
            _collection = new List<T>(capacity);
        }
        
        public FreezableCollection(IEnumerable<T> collection)
        {
            WritePreamble();
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            var c = collection as IList<T> ?? collection.ToList();
            var count = GetCount(c);
            _collection = count <= 0 ? new List<T>() : new List<T>(count);
            foreach (var obj in c)
            {
                if (obj == null)
                    throw new ArgumentException("Collection_NoNull");
                OnFreezablePropertyChanged(null, obj);
                _collection.Add(obj);
            }
            WritePostscript();
        }
        
        public new FreezableCollection<T> Clone()
        {
            return (FreezableCollection<T>)base.Clone();
        }
        
        public new FreezableCollection<T> CloneCurrentValue()
        {
            return (FreezableCollection<T>)base.CloneCurrentValue();
        }
        
        public void Add(T value)
        {
            AddHelper(value);
        }
        
        public void Clear()
        {
            CheckReentrancy();
            WritePreamble();
            for (var index = _collection.Count - 1; index >= 0; --index)
                OnFreezablePropertyChanged(_collection[index], null);
            _collection.Clear();
            _version = _version + 1U;
            WritePostscript();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset, 0, default(T), 0, default(T));
        }
        
        public bool Contains(T value)
        {
            ReadPreamble();
            return _collection.Contains(value);
        }
        
        public int IndexOf(T value)
        {
            ReadPreamble();
            return _collection.IndexOf(value);
        }
        
        public void Insert(int index, T value)
        {
            if (value == null)
                throw new ArgumentException("Collection_NoNull");
            CheckReentrancy();
            WritePreamble();
            OnFreezablePropertyChanged(null, value);
            _collection.Insert(index, value);
            _version = _version + 1U;
            WritePostscript();
            OnCollectionChanged(NotifyCollectionChangedAction.Add, 0, default(T), index, value);
        }

        public bool Remove(T value)
        {
            WritePreamble();
            var index = IndexOf(value);
            if (index < 0)
                return false;
            CheckReentrancy();
            var oldValue = _collection[index];
            OnFreezablePropertyChanged(oldValue, null);
            _collection.RemoveAt(index);
            _version = _version + 1U;
            WritePostscript();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, index, oldValue, 0, default(T));
            return true;
        }

        public void RemoveAt(int index)
        {
            var oldValue = _collection[index];
            RemoveAtWithoutFiringPublicEvents(index);
            WritePostscript();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, index, oldValue, 0, default(T));
        }

        internal void RemoveAtWithoutFiringPublicEvents(int index)
        {
            CheckReentrancy();
            WritePreamble();
            OnFreezablePropertyChanged(_collection[index], null);
            _collection.RemoveAt(index);
            _version = _version + 1U;
        }

        public void CopyTo(T[] array, int index)
        {
            ReadPreamble();
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (index < 0 || index + _collection.Count > array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            _collection.CopyTo(array, index);
        }

        public Enumerator GetEnumerator()
        {
            ReadPreamble();
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList.Add(object value)
        {
            return AddHelper(Cast(value));
        }

        bool IList.Contains(object value)
        {
            return Contains(value as T);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf(value as T);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, Cast(value));
        }

        void IList.Remove(object value)
        {
            Remove(value as T);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ReadPreamble();
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (index < 0 || index + _collection.Count > array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (array.Rank != 1)
                throw new ArgumentException("Collection_BadRank");
            try
            {
                var count = _collection.Count;
                for (var index1 = 0; index1 < count; ++index1)
                    array.SetValue(_collection[index1], index + index1);
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException("Collection_BadDestArray "+(object)GetType().Name, ex);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged == null)
                return;
            using (BlockReentrancy())
            {
                CollectionChanged(this, e);
            }
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PrivatePropertyChanged?.Invoke(this, e);
        }

        private T Cast(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (!(value is T))
                throw new ArgumentException("Collection_BadType"+(object)GetType().Name+" "+(object)value.GetType().Name+""+(object)"T");
            return (T)value;
        }

        private int GetCount(IEnumerable<T> enumerable)
        {
            var collection1 = enumerable as ICollection;
            if (collection1 != null)
                return collection1.Count;
            var collection2 = enumerable as ICollection<T>;
            if (collection2 != null)
                return collection2.Count;
            return -1;
        }

        private int AddHelper(T value)
        {
            CheckReentrancy();
            var num = AddWithoutFiringPublicEvents(value);
            WritePostscript();
            OnCollectionChanged(NotifyCollectionChangedAction.Add, 0, default(T), num - 1, value);
            return num;
        }

        internal int AddWithoutFiringPublicEvents(T value)
        {
            if (value == null)
                throw new ArgumentException("Collection_NoNull");
            WritePreamble();
            OnFreezablePropertyChanged(null, value);
            _collection.Add(value);
            _version = _version + 1U;
            return _collection.Count;
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, int oldIndex, T oldValue, int newIndex, T newValue)
        {
            if (PrivatePropertyChanged == null && CollectionChanged == null)
                return;
            using (BlockReentrancy())
            {
                if (PrivatePropertyChanged != null)
                {
                    if (action != NotifyCollectionChangedAction.Replace && action != NotifyCollectionChangedAction.Move)
                        OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                }
                if (CollectionChanged == null)
                    return;
                NotifyCollectionChangedEventArgs e;
                switch (action)
                {
                    case NotifyCollectionChangedAction.Add:
                        e = new NotifyCollectionChangedEventArgs(action, newValue, newIndex);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        e = new NotifyCollectionChangedEventArgs(action, oldValue, oldIndex);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        e = new NotifyCollectionChangedEventArgs(action, newValue, oldValue, newIndex);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        e = new NotifyCollectionChangedEventArgs(action);
                        break;
                    default:
                        throw new InvalidOperationException("Freezable_UnexpectedChange");
                }
                OnCollectionChanged(e);
            }
        }
        
        protected override Freezable CreateInstanceCore()
        {
            return new FreezableCollection<T>();
        }

        private void CloneCommon(FreezableCollection<T> source, CloneCommonType cloneType)
        {
            var count = source._collection.Count;
            _collection = new List<T>(count);
            for (var index = 0; index < count; ++index)
            {
                var obj = source._collection[index];
                var freezable = (object)obj as Freezable;
                if (freezable != null)
                {
                    switch (cloneType)
                    {
                        case CloneCommonType.Clone:
                            obj = freezable.Clone() as T;
                            break;
                        case CloneCommonType.CloneCurrentValue:
                            obj = freezable.CloneCurrentValue() as T;
                            break;
                        case CloneCommonType.GetAsFrozen:
                            obj = freezable.GetAsFrozen() as T;
                            break;
                        case CloneCommonType.GetCurrentValueAsFrozen:
                            obj = freezable.GetCurrentValueAsFrozen() as T;
                            break;
                        default:
                            Debug.Assert(false, "Invalid CloneCommonType encountered.");
                            break;
                    }
                    if (obj == null)
                        throw new InvalidOperationException("Freezable_CloneInvalidType"+(object)typeof(T).Name);
                }
                OnFreezablePropertyChanged(null, obj);
                _collection.Add(obj);
            }
        }
        
        protected override void CloneCore(Freezable source)
        {
            base.CloneCore(source);
            CloneCommon((FreezableCollection<T>)source, CloneCommonType.Clone);
        }
        
        protected override void CloneCurrentValueCore(Freezable source)
        {
            base.CloneCurrentValueCore(source);
            CloneCommon((FreezableCollection<T>)source, CloneCommonType.CloneCurrentValue);
        }
        
        protected override void GetAsFrozenCore(Freezable source)
        {
            base.GetAsFrozenCore(source);
            CloneCommon((FreezableCollection<T>)source, CloneCommonType.GetAsFrozen);
        }
        
        protected override void GetCurrentValueAsFrozenCore(Freezable source)
        {
            base.GetCurrentValueAsFrozenCore(source);
            CloneCommon((FreezableCollection<T>)source, CloneCommonType.GetCurrentValueAsFrozen);
        }

        protected override bool FreezeCore(bool isChecking)
        {
            var flag = base.FreezeCore(isChecking);
            var count = _collection.Count;
            for (var index = 0; index < count & flag; ++index)
            {
                var obj = _collection[index];
                var freezable = (object)obj as Freezable;
                if (freezable != null)
                    flag &= Freeze(freezable, isChecking);
                else
                    flag &= obj.Dispatcher == null;
            }
            return flag;
        }

        private IDisposable BlockReentrancy()
        {
            _monitor.Enter();
            return _monitor;
        }

        private void CheckReentrancy()
        {
            if (_monitor.Busy)
                throw new InvalidOperationException("Freezable_Reentrant");
        }

        private enum CloneCommonType
        {
            Clone,
            CloneCurrentValue,
            GetAsFrozen,
            GetCurrentValueAsFrozen
        }
        
        public struct Enumerator : IEnumerator<T>
        {
            private T _current;
            private readonly FreezableCollection<T> _list;
            private readonly uint _version;
            private int _index;

            object IEnumerator.Current => Current;

            public T Current
            {
                get
                {
                    if (_index > -1)
                        return _current;
                    if (_index == -1)
                        throw new InvalidOperationException("Enumerator_NotStarted");
                    throw new InvalidOperationException("Enumerator_ReachedEnd");
                }
            }

            internal Enumerator(FreezableCollection<T> list)
            {
                _list = list;
                _version = list._version;
                _index = -1;
                _current = default(T);
            }

            void IDisposable.Dispose()
            {
            }
            
            public bool MoveNext()
            {
                _list.ReadPreamble();
                if ((int)_version != (int)_list._version)
                    throw new InvalidOperationException("Enumerator_CollectionChanged");
                if (_index > -2 && _index < _list._collection.Count - 1)
                {
                    var list = _list._collection;
                    var num = _index + 1;
                    _index = num;
                    var index = num;
                    _current = list[index];
                    return true;
                }
                _index = -2;
                return false;
            }
            
            public void Reset()
            {
                _list.ReadPreamble();
                if ((int)_version != (int)_list._version)
                    throw new InvalidOperationException("Enumerator_CollectionChanged");
                _index = -1;
            }
        }

        private class SimpleMonitor : IDisposable
        {
            private int _busyCount;

            public bool Busy => _busyCount > 0;

            public void Enter()
            {
                _busyCount = _busyCount + 1;
            }

            public void Dispose()
            {
                _busyCount = _busyCount - 1;
                GC.SuppressFinalize(this);
            }
        }
    }
}