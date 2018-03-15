using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TestManager.Lib.ViewModelsSearch.SearchModels;
using TestManager.Lib.ViewModelsSearch.SearchModels.Enums;

namespace TestManager.Lib.ViewModels.Tree
{
    public class TreeViewItemViewModel : ViewModelBase
    {
        #region fields
        protected static DispatcherPriority _childrenEditPrio = DispatcherPriority.DataBind;
        protected MatchType _Match;
        protected bool _isItemExpanded;

        #endregion fields

        #region properties
        /// <summary>
        /// Gets the <seealso cref="TreeViewItemViewModel"/> Parent (if any) of this item.
        /// </summary>
        public TreeViewItemViewModel Parent { get; protected set; }

        /// <summary>
        /// Gets the <see cref="MatchType"/> for this item. This field
        /// determines the classification of a match towards a current
        /// search criteria.
        /// </summary>
        public MatchType Match
        {
            get { return _Match; }

            private set
            {
                if (_Match != value)
                {
                    _Match = value;
                    this.RaisePropertyChanged("Match");
                }
            }
        }

        /// <summary>
        /// Gets/sets whether this item is expanded in
        /// the tree view (items under this item are visible), or not.
        /// </summary>
        public bool IsItemExpanded
        {
            get { return _isItemExpanded; }
            set
            {
                if (_isItemExpanded != value)
                {
                    _isItemExpanded = value;
                    this.RaisePropertyChanged(() => IsItemExpanded);
                }
            }
        }

        /// <summary>
        /// Gets the technical ID (for data reference) of this item.
        /// </summary>
        public int ID { get; }

        public virtual string Name { get; set; }

        #endregion properties

        #region constructors
        /// <summary>
        /// Parameterized Class Constructor
        /// </summary>
        public TreeViewItemViewModel(TreeViewItemViewModel parent) : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Class Constructor
        /// </summary>
        protected TreeViewItemViewModel()
        {
            _isItemExpanded = false;

            _Match = MatchType.NoMatch;
            Parent = null;
        }
        #endregion constructors

        /// <summary>
        /// Sets the tree view item to the corresponding expanded state
        /// as indicated in the <seealso cref="IsExpanded"/> property.
        /// </summary>
        /// <param name="isExpanded"></param>
        internal void SetExpand(bool isExpanded)
        {
            IsItemExpanded = isExpanded;
        }

        /// <summary>
        /// Sets the type of match detmerined for this item against a certain
        /// match criteria.
        /// 
        /// We use this method instead of a setter to make this accessible for
        /// the root viewmodel but next to invisible for everyone else...
        /// </summary>
        /// <param name="match"></param>
        internal MatchType SetMatch(MatchType match)
        {
            this.Match = match;

            return match;
        }
    }

    /// <summary>
    /// Implements an items viewmodel that should be bound to an items collection in a tree view.
    /// </summary>
    public class TreeViewItemViewModel<T>: TreeViewItemViewModel where T: TreeViewItemViewModel
    {
        #region fields

        private List<T> _backUpNodes = null;

        private readonly ObservableCollection<T> _items = null;

        #endregion fields

        #region properties

        /// <summary>
        /// Gets the COMPLETE collection of child items under this item (complete sub-tree).
        /// This collection is always complete, maintained, and always available list of
        /// items under this item.
        /// 
        /// The <see cref="Items"/> collection contains child items (sub-tree) matched
        /// to the current search criteria (if any).
        /// </summary>
        public IEnumerable<T> BackUpNodes
        {
            get
            {
                return _backUpNodes;
            }
        }

        /// <summary>
        /// Gets all child items (nodes) under this item. This collection (sub-tree)
        /// may hold no items, if this item is filtered and none of the items in the
        /// <see cref="Children"/> collection matched the current search criteria.
        /// 
        /// The <see cref="BackUpNodes"/> collection contains the complete, maintained,
        /// and always available list of items under this item.
        /// </summary>
        public IEnumerable<T> Children
        {
            get
            {
                return _items;
            }
        }

        /// <summary>
        /// Returns the total number of child items (nodes) under this item.
        /// </summary>
        public int ChildrenCount => _backUpNodes.Count;

        #endregion properties

        #region constructors
        /// <summary>
        /// Parameterized Class Constructor
        /// </summary>
        public TreeViewItemViewModel(TreeViewItemViewModel parent): this()
        {
           this.Parent = parent;
           ItemsClear(false);  // Lazy Load Children !!!
        }

        /// <summary>
        /// Class Constructor
        /// </summary>
        protected TreeViewItemViewModel(): base()
        {
            _items = new ObservableCollection<T>();
            _backUpNodes = new List<T>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Returns the string path either:
        /// 1) for the <paramref name="current"/> item or
        /// 2) for this item (if optional parameter <paramref name="current"/> is not set).
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public string GetStackPath(TreeViewItemViewModel current = null)
        {
            if (current == null)
                current = this;

            string result = string.Empty;

            // Traverse the list of parents backwards and
            // add each child to the path
            while (current != null)
            {
                result = "/" + Name + result;

                current = current.Parent;
            }

            return result;
        }

        /// <summary>
        /// Re-load treeview items below the root item.
        /// </summary>
        /// <returns>Number of items found below the root item.</returns>
        internal int LoadItems()
        {
            ItemsClear(false); // Clear collection of children

            if (_backUpNodes.Count() > 0)
            {
                foreach (var item in _backUpNodes)
                    this.ItemsAdd(item, false);
            }

            return _items.Count;
        }

        internal void ItemsClear(bool bClearBackup = true)
        {
            Application.Current.Dispatcher.Invoke(() => { _items.Clear(); }, _childrenEditPrio);

            if (bClearBackup == true)
                _backUpNodes.Clear();
        }

        /// <summary>
        /// Determines the <seealso cref="MatchType"/> of a node by evaluating the
        /// given search string parameter and the matchtype of its children.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="filterString"></param>
        internal MatchType ProcessNodeMatch(SearchParams searchParams)
        {
            MatchType matchThisNode = MatchType.NoMatch;

            // Determine whether this node is a match or not
            if (searchParams.MatchSearchString(Name) == true)
                matchThisNode = MatchType.NodeMatch;

            ItemsClear(false);

            if (ChildrenCount > 0)
            {
                // Evaluate children by adding only thos children that contain no 'NoMatch'
                MatchType maxChildMatch = MatchType.NoMatch;
                foreach (var item in BackUpNodes)
                {
                    if (item.Match != MatchType.NoMatch)
                    {
                        // Expand this item if it (or one of its children) contains a match
                        if (item.Match == MatchType.SubNodeMatch ||
                            item.Match == MatchType.Node_AND_SubNodeMatch)
                        {
                            item.SetExpand(true);
                        }
                        else
                            item.SetExpand(false);

                        if (maxChildMatch < item.Match)
                            maxChildMatch = item.Match;

                        ItemsAdd(item, false);
                    }
                }

                if (matchThisNode == MatchType.NoMatch && maxChildMatch != MatchType.NoMatch)
                    matchThisNode = MatchType.SubNodeMatch;

                if (matchThisNode == MatchType.NodeMatch && maxChildMatch != MatchType.NoMatch)
                    matchThisNode = MatchType.Node_AND_SubNodeMatch;
            }

            return matchThisNode;
        }

        /// <summary>
        /// Add a child item including a reference to a backupnode
        /// (add to backupnode is determined by <paramref name="bAddBackup"/>).
        /// </summary>
        /// <param name="child"></param>
        /// <param name="bAddBackup"></param>
        private void ItemsAdd(T child, bool bAddBackup = true)
        {
            Application.Current.Dispatcher.Invoke(() => { _items.Add(child); }, _childrenEditPrio);

            if (bAddBackup == true)
                _backUpNodes.Add(child);
        }

        /// <summary>
        /// Add a child node to a backupnode only.
        /// </summary>
        /// <param name="child"></param>
        private void ItemsAddBackupNodes(T child)
        {
            _backUpNodes.Add(child);
        }

        private void ItemsRemove(T child, bool bRemoveBackup = true)
        {
            Application.Current.Dispatcher.Invoke(() => { _items.Remove(child); }, _childrenEditPrio);

            if (bRemoveBackup == true)
                _backUpNodes.Remove(child);
        }

        #endregion methods
    }
}
