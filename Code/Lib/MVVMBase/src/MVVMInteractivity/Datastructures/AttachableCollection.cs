using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;

namespace MVVMBase.Interactivity
{
    public abstract class AttachableCollection<T> : FreezableCollection<T>, IAttachedObject where T : DependencyObject, IAttachedObject
    {
        private Collection<T> snapshot;
        private DependencyObject associatedObject;
        
        internal DependencyObject AssociatedObject
        {
            get
            {
                ReadPreamble();
                return associatedObject;
            }
        }

        DependencyObject IAttachedObject.AssociatedObject => AssociatedObject;

        internal AttachableCollection()
        {
            CollectionChanged += OnCollectionChanged;
            snapshot = new Collection<T>();
        }
        
        protected abstract void OnAttached();
        
        protected abstract void OnDetaching();

        protected abstract void ItemAdded(T item);

        protected abstract void ItemRemoved(T item);

        [Conditional("DEBUG")]
        private void VerifySnapshotIntegrity()
        {
            if (Count != snapshot.Count)
                return;
            for (var index = 0; index < Count; ++index)
            {
                if (!Equals(this[index], snapshot[index]))
                    break;
            }
        }

        private void VerifyAdd(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (snapshot.Contains(item))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "DuplicateItemInCollectionExceptionMessage" + " " + (object) typeof (T).Name + " " + (object) GetType().Name));
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var enumerator1 = e.NewItems.GetEnumerator();
                    try
                    {
                        while (enumerator1.MoveNext())
                        {
                            var obj = (T)enumerator1.Current;
                            try
                            {
                                VerifyAdd(obj);
                                ItemAdded(obj);
                            }
                            finally
                            {
                                snapshot.Insert(IndexOf(obj), obj);
                            }
                        }
                        break;
                    }
                    finally
                    {
                        var disposable = enumerator1 as IDisposable;
                        disposable?.Dispose();
                    }
                case NotifyCollectionChangedAction.Remove:
                    var enumerator2 = e.OldItems.GetEnumerator();
                    try
                    {
                        while (enumerator2.MoveNext())
                        {
                            var obj = (T)enumerator2.Current;
                            ItemRemoved(obj);
                            snapshot.Remove(obj);
                        }
                        break;
                    }
                    finally
                    {
                        var disposable = enumerator2 as IDisposable;
                        disposable?.Dispose();
                    }
                case NotifyCollectionChangedAction.Replace:
                    foreach (T obj in e.OldItems)
                    {
                        ItemRemoved(obj);
                        snapshot.Remove(obj);
                    }
                    var enumerator3 = e.NewItems.GetEnumerator();
                    try
                    {
                        while (enumerator3.MoveNext())
                        {
                            var obj = (T)enumerator3.Current;
                            try
                            {
                                VerifyAdd(obj);
                                ItemAdded(obj);
                            }
                            finally
                            {
                                snapshot.Insert(IndexOf(obj), obj);
                            }
                        }
                        break;
                    }
                    finally
                    {
                        var disposable = enumerator3 as IDisposable;
                        disposable?.Dispose();
                    }
                case NotifyCollectionChangedAction.Reset:
                    foreach (var obj in snapshot)
                        ItemRemoved(obj);
                    snapshot = new Collection<T>();
                    using (var enumerator4 = GetEnumerator())
                    {
                        while (enumerator4.MoveNext())
                        {
                            var current = enumerator4.Current;
                            VerifyAdd(current);
                            ItemAdded(current);
                        }
                        break;
                    }
            }
        }
        public void Attach(DependencyObject dependencyObject)
        {
            if (Equals(dependencyObject, AssociatedObject))
                return;
            if (AssociatedObject != null)
                throw new InvalidOperationException();
            if (Interaction.ShouldRunInDesignMode || !(bool)GetValue(DesignerProperties.IsInDesignModeProperty))
            {
                WritePreamble();
                associatedObject = dependencyObject;
                WritePostscript();
            }
            OnAttached();
        }
        public void Detach()
        {
            OnDetaching();
            WritePreamble();
            associatedObject = null;
            WritePostscript();
        }
    }
}