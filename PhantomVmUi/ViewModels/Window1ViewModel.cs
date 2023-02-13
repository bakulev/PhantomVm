using System;
using System.Collections.Generic;
using System.Linq;
using DiagramDesigner;
using System.Threading.Tasks;
using System.Windows;
using Cc.Anba.PhantomOs.VirtualMachine;

namespace Cc.Anba.PhantomOs.Apps.VmUi
{
    public class Window1ViewModel : INPCBase
    {

        private List<int> savedDiagrams = new List<int>();
        private int? savedDiagramId;
        private List<SelectableDesignerItemViewModelBase> itemsToRemove;
        private IMessageBoxService messageBoxService;
        private DiagramViewModel diagramViewModel = new DiagramViewModel();
        private bool isBusy = false;

        private PvmRoot _pvmRoot;

        public Window1ViewModel()
        {

            _pvmRoot = new PvmRoot();


            messageBoxService = ApplicationServicesProvider.Instance.Provider.MessageBoxService;

            ToolBoxViewModel = new ToolBoxViewModel();
            DiagramViewModel = new DiagramViewModel();

            DeleteSelectedItemsCommand = new SimpleCommand(ExecuteDeleteSelectedItemsCommand);
            CreateNewDiagramCommand = new SimpleCommand(ExecuteCreateNewDiagramCommand);
            SaveDiagramCommand = new SimpleCommand(ExecuteSaveDiagramCommand);
            LoadDiagramCommand = new SimpleCommand(ExecuteLoadDiagramCommand);
            GroupCommand = new SimpleCommand(ExecuteGroupCommand);

            //OrthogonalPathFinder is a pretty bad attempt at finding path points, it just shows you, you can swap this out with relative
            //ease if you wish just create a new IPathFinder class and pass it in right here
            ConnectorViewModel.PathFinder = new StraightPathFinder(); // bakulev new OrthogonalPathFinder();



            SettingsDesignerItemViewModel item1 = new SettingsDesignerItemViewModel();
            item1.Parent = DiagramViewModel;
            item1.Left = 100;
            item1.Top = 100;
            DiagramViewModel.Items.Add(item1);

            PersistDesignerItemViewModel item2 = new PersistDesignerItemViewModel();
            item2.DescriptionText = "Descr 2";
            item2.Parent = DiagramViewModel;
            item2.Left = 300;
            item2.Top = 300;
            item2.ItemHeight = 200;
            item2.ItemWidth = 250;
            DiagramViewModel.Items.Add(item2);

            PersistDesignerItemViewModel item3 = new PersistDesignerItemViewModel();
            item3.DescriptionText = "Descr 3";
            item3.Parent = DiagramViewModel;
            item3.Left = 400;
            item3.Top = 400;
            DiagramViewModel.Items.Add(item3);

            ConnectorViewModel con1 = new ConnectorViewModel(item1.RightConnector, item2.TopConnector);
            con1.Parent = DiagramViewModel;
            DiagramViewModel.Items.Add(con1);

        }


        public SimpleCommand DeleteSelectedItemsCommand { get; private set; }
        public SimpleCommand CreateNewDiagramCommand { get; private set; }
        public SimpleCommand SaveDiagramCommand { get; private set; }
        public SimpleCommand GroupCommand { get; private set; }
        public SimpleCommand LoadDiagramCommand { get; private set; }
        public ToolBoxViewModel ToolBoxViewModel { get; private set; }


        public DiagramViewModel DiagramViewModel
        {
            get
            {
                return diagramViewModel;
            }
            set
            {
                if (diagramViewModel != value)
                {
                    diagramViewModel = value;
                    NotifyChanged("DiagramViewModel");
                }
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    NotifyChanged("IsBusy");
                }
            }
        }


        public List<int> SavedDiagrams
        {
            get
            {
                return savedDiagrams;
            }
            set
            {
                if (savedDiagrams != value)
                {
                    savedDiagrams = value;
                    NotifyChanged("SavedDiagrams");
                }
            }
        }

        public int? SavedDiagramId
        {
            get
            {
                return savedDiagramId;
            }
            set
            {
                if (savedDiagramId != value)
                {
                    savedDiagramId = value;
                    NotifyChanged("SavedDiagramId");
                }
            }
        }



        private void ExecuteDeleteSelectedItemsCommand(object parameter)
        {
            itemsToRemove = DiagramViewModel.SelectedItems;
            List<SelectableDesignerItemViewModelBase> connectionsToAlsoRemove = new List<SelectableDesignerItemViewModelBase>();

            foreach (var connector in DiagramViewModel.Items.OfType<ConnectorViewModel>())
            {
                if (ItemsToDeleteHasConnector(itemsToRemove, connector.SourceConnectorInfo))
                {
                    connectionsToAlsoRemove.Add(connector);
                }

                if (ItemsToDeleteHasConnector(itemsToRemove, (FullyCreatedConnectorInfo)connector.SinkConnectorInfo))
                {
                    connectionsToAlsoRemove.Add(connector);
                }

            }
            itemsToRemove.AddRange(connectionsToAlsoRemove);
            foreach (var selectedItem in itemsToRemove)
            {
                DiagramViewModel.RemoveItemCommand.Execute(selectedItem);
            }
        }

        private void ExecuteCreateNewDiagramCommand(object parameter)
        {
            //ensure that itemsToRemove is cleared ready for any new changes within a session
            itemsToRemove = new List<SelectableDesignerItemViewModelBase>();
            SavedDiagramId = null;
            DiagramViewModel.CreateNewDiagramCommand.Execute(null);
        }

        private void ExecuteSaveDiagramCommand(object parameter)
        {
            if (!DiagramViewModel.Items.Any())
            {
                messageBoxService.ShowError("There must be at least one item in order save a diagram");
                return;
            }

            IsBusy = true;

            Task<int> task = Task.Factory.StartNew<int>(() =>
            {

                if (SavedDiagramId != null)
                {
                    int currentSavedDiagramId = (int)SavedDiagramId.Value;

                    //If we have a saved diagram, we need to make sure we clear out all the removed items that
                    //the user deleted as part of this work sesssion
                    foreach (var itemToRemove in itemsToRemove)
                    {
                        //DeleteFromDatabase(wholeDiagramToSave, itemToRemove);
                    }
                    //start with empty collections of connections and items, which will be populated based on current diagram
                }
                else
                {
                    //wholeDiagramToSave = new DiagramItem();
                }

                //ensure that itemsToRemove is cleared ready for any new changes within a session
                itemsToRemove = new List<SelectableDesignerItemViewModelBase>();

                //SavePersistDesignerItem(wholeDiagramToSave, DiagramViewModel);

                //wholeDiagramToSave.Id = databaseAccessService.SaveDiagram(wholeDiagramToSave);
                return 0; // wholeDiagramToSave.Id;
            });
            task.ContinueWith((ant) =>
            {
                int wholeDiagramToSaveId = ant.Result;
                if (!savedDiagrams.Contains(wholeDiagramToSaveId))
                {
                    List<int> newDiagrams = new List<int>(savedDiagrams);
                    newDiagrams.Add(wholeDiagramToSaveId);
                    SavedDiagrams = newDiagrams;

                }
                IsBusy = false;
                messageBoxService.ShowInformation(string.Format("Finished saving Diagram Id : {0}", wholeDiagramToSaveId));

            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private void SavePersistDesignerItem(IDiagramViewModel diagramViewModel)
        {
            //Save all PersistDesignerItemViewModel
            foreach (var persistItemVM in diagramViewModel.Items.OfType<PersistDesignerItemViewModel>())
            {
                //wholeDiagramToSave.DesignerItems.Add(new DiagramItemData(persistDesignerItem.Id, typeof(PersistDesignerItem)));
            }
            //Save all SettingsDesignerItemViewModel
            foreach (var settingsItemVM in diagramViewModel.Items.OfType<SettingsDesignerItemViewModel>())
            {
                //wholeDiagramToSave.DesignerItems.Add(new DiagramItemData(settingsDesignerItem.Id, typeof(SettingsDesignerItem)));
            }
            //Save all GroupingDesignerItemViewModel
            foreach (var groupingItemVM in diagramViewModel.Items.OfType<GroupingDesignerItemViewModel>())
            {
                if(groupingItemVM.Items != null && groupingItemVM.Items.Count > 0)
                {
                    //SavePersistDesignerItem(groupDesignerItem, groupingItemVM);
                }
                //wholeDiagramToSave.DesignerItems.Add(new DiagramItemData(groupDesignerItem.Id, typeof(GroupDesignerItem)));
            }
            //Save all connections which should now have their Connection.DataItems filled in with correct Ids
            foreach (var connectionVM in diagramViewModel.Items.OfType<ConnectorViewModel>())
            {
                FullyCreatedConnectorInfo sinkConnector = connectionVM.SinkConnectorInfo as FullyCreatedConnectorInfo;

                //wholeDiagramToSave.ConnectionIds.Add(connectionVM.Id);
            }
        }

        private void ExecuteLoadDiagramCommand(object parameter)
        {
            IsBusy = true;
            if (SavedDiagramId == null)
            {
                messageBoxService.ShowError("You need to select a diagram to load");
                return;
            }

            Task<DiagramViewModel> task = Task.Factory.StartNew<DiagramViewModel>(() =>
            {
                //ensure that itemsToRemove is cleared ready for any new changes within a session
                itemsToRemove = new List<SelectableDesignerItemViewModelBase>();
                DiagramViewModel diagramViewModel = new DiagramViewModel();

                //wholeDiagramToLoad = databaseAccessService.FetchDiagram((int)SavedDiagramId.Value);

                LoadPerstistDesignerItems(diagramViewModel);

                return diagramViewModel;
            });
            task.ContinueWith((ant) =>
            {
                this.DiagramViewModel = ant.Result;
                IsBusy = false;
                //messageBoxService.ShowInformation(string.Format("Finished loading Diagram Id : {0}", wholeDiagramToLoad.Id));

            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private void LoadPerstistDesignerItems(IDiagramViewModel diagramViewModel)
        {
            //load diagram items
            //foreach (DiagramItemData diagramItemData in wholeDiagramToLoad.DesignerItems)
            {
                //if (diagramItemData.ItemType == typeof(PersistDesignerItem))
                {
                    //PersistDesignerItemViewModel persistDesignerItemViewModel =
                    //    new PersistDesignerItemViewModel(persistedDesignerItem.Id, diagramViewModel, persistedDesignerItem.Left, persistedDesignerItem.Top, persistedDesignerItem.ItemWidth, persistedDesignerItem.ItemHeight, persistedDesignerItem.HostUrl);
                    //diagramViewModel.Items.Add(persistDesignerItemViewModel);
                }
                //if (diagramItemData.ItemType == typeof(SettingsDesignerItem))
                {
                    //SettingsDesignerItemViewModel settingsDesignerItemViewModel =
                    //    new SettingsDesignerItemViewModel(settingsDesignerItem.Id, diagramViewModel, settingsDesignerItem.Left, settingsDesignerItem.Top, settingsDesignerItem.ItemWidth, settingsDesignerItem.ItemHeight, settingsDesignerItem.Setting1);
                    //diagramViewModel.Items.Add(settingsDesignerItemViewModel);
                }
                //if (diagramItemData.ItemType == typeof(GroupDesignerItem))
                {
                    //GroupingDesignerItemViewModel groupingDesignerItemViewModel =
                    //    new GroupingDesignerItemViewModel(groupDesignerItem.Id, diagramViewModel, groupDesignerItem.Left, groupDesignerItem.Top, groupDesignerItem.ItemWidth, groupDesignerItem.ItemHeight);
                    //if(groupDesignerItem.DesignerItems != null && groupDesignerItem.DesignerItems.Count > 0)
                    {
                        //LoadPerstistDesignerItems(groupingDesignerItemViewModel);
                    }
                    //diagramViewModel.Items.Add(groupingDesignerItemViewModel);
                }
            }
            //load connection items
            //foreach (int connectionId in wholeDiagramToLoad.ConnectionIds)
            {
                //DesignerItemViewModelBase sourceItem = GetConnectorDataItem(diagramViewModel, connection.SourceId, connection.SourceType);
                //ConnectorOrientation sourceConnectorOrientation = GetOrientationForConnector(connection.SourceOrientation);
                //FullyCreatedConnectorInfo sourceConnectorInfo = GetFullConnectorInfo(connection.Id, sourceItem, sourceConnectorOrientation);

                //DesignerItemViewModelBase sinkItem = GetConnectorDataItem(diagramViewModel, connection.SinkId, connection.SinkType);
                //ConnectorOrientation sinkConnectorOrientation = GetOrientationForConnector(connection.SinkOrientation);
                //FullyCreatedConnectorInfo sinkConnectorInfo = GetFullConnectorInfo(connection.Id, sinkItem, sinkConnectorOrientation);

                //ConnectorViewModel connectionVM = new ConnectorViewModel(connection.Id, diagramViewModel, sourceConnectorInfo, sinkConnectorInfo);
                //diagramViewModel.Items.Add(connectionVM);
            }
        }

        private void ExecuteGroupCommand(object parameter)
        {
            if (diagramViewModel.SelectedItems.Count > 0)
            {
                // if only one selected item is a Grouping item -> ungroup
                if (diagramViewModel.SelectedItems[0] is GroupingDesignerItemViewModel && diagramViewModel.SelectedItems.Count == 1)
                {
                    GroupingDesignerItemViewModel groupObject = diagramViewModel.SelectedItems[0] as GroupingDesignerItemViewModel;
                    foreach (var item in groupObject.Items)
                    {

                        if (item is DesignerItemViewModelBase)
                        {
                            DesignerItemViewModelBase tmp = (DesignerItemViewModelBase)item;
                            tmp.Top += groupObject.Top;
                            tmp.Left += groupObject.Left;
                        }
                        diagramViewModel.AddItemCommand.Execute(item);
                        item.Parent = DiagramViewModel;
                    }

                    // "cut" connections between DiagramItems and the Group
                    List<SelectableDesignerItemViewModelBase> GroupedItemsToRemove = new List<SelectableDesignerItemViewModelBase>();
                    foreach (var connector in DiagramViewModel.Items.OfType<ConnectorViewModel>())
                    {
                        if (groupObject == connector.SourceConnectorInfo.DataItem)
                        {
                            GroupedItemsToRemove.Add(connector);
                        }

                        if (groupObject == ((FullyCreatedConnectorInfo)connector.SinkConnectorInfo).DataItem)
                        {
                            GroupedItemsToRemove.Add(connector);
                        }
                    }
                    GroupedItemsToRemove.Add(groupObject);
                    foreach (var selectedItem in GroupedItemsToRemove)
                    {
                        DiagramViewModel.RemoveItemCommand.Execute(selectedItem);
                    }

                }
                else if (diagramViewModel.SelectedItems.Count > 1)
                {
                    double margin = 15;
                    Rect rekt = GetBoundingRectangle(diagramViewModel.SelectedItems, margin);

                    GroupingDesignerItemViewModel groupItem = new GroupingDesignerItemViewModel(0, this.diagramViewModel, rekt.Left, rekt.Top);
                    groupItem.ItemWidth = rekt.Width;
                    groupItem.ItemHeight = rekt.Height;
                    foreach (var item in diagramViewModel.SelectedItems)
                    {
                        if (item is DesignerItemViewModelBase)
                        {
                            DesignerItemViewModelBase tmp = (DesignerItemViewModelBase)item;
                            tmp.Top -= rekt.Top;
                            tmp.Left -= rekt.Left;
                        }
                        groupItem.Items.Add(item);
                        item.Parent = groupItem;

                    }

                    // "cut" connections between DiagramItems which are going to be grouped and
                    // Diagramitems which are not going to be grouped
                    List<SelectableDesignerItemViewModelBase> GroupedItemsToRemove = DiagramViewModel.SelectedItems;
                    List<SelectableDesignerItemViewModelBase> connectionsToAlsoRemove = new List<SelectableDesignerItemViewModelBase>();

                    foreach (var connector in DiagramViewModel.Items.OfType<ConnectorViewModel>())
                    {
                        if (ItemsToDeleteHasConnector(GroupedItemsToRemove, connector.SourceConnectorInfo))
                        {
                            connectionsToAlsoRemove.Add(connector);
                        }

                        if (ItemsToDeleteHasConnector(GroupedItemsToRemove, (FullyCreatedConnectorInfo)connector.SinkConnectorInfo))
                        {
                            connectionsToAlsoRemove.Add(connector);
                        }

                    }
                    GroupedItemsToRemove.AddRange(connectionsToAlsoRemove);
                    foreach (var selectedItem in GroupedItemsToRemove)
                    {
                        DiagramViewModel.RemoveItemCommand.Execute(selectedItem);
                    }

                    diagramViewModel.SelectedItems.Clear();
                    this.diagramViewModel.Items.Add(groupItem);
                }
            }

        }

        private static Rect GetBoundingRectangle(IEnumerable<SelectableDesignerItemViewModelBase> items, double margin)
        {
            double x1 = Double.MaxValue;
            double y1 = Double.MaxValue;
            double x2 = Double.MinValue;
            double y2 = Double.MinValue;

            foreach (DesignerItemViewModelBase item in items.OfType<DesignerItemViewModelBase>())
            {
                x1 = Math.Min(item.Left - margin, x1);
                y1 = Math.Min(item.Top - margin, y1);

                x2 = Math.Max(item.Left + item.ItemWidth + margin, x2);
                y2 = Math.Max(item.Top + item.ItemHeight + margin, y2);
            }

            return new Rect(new Point(x1, y1), new Point(x2, y2));
        }

        private FullyCreatedConnectorInfo GetFullConnectorInfo(int connectorId, DesignerItemViewModelBase dataItem, ConnectorOrientation connectorOrientation)
        {
            switch (connectorOrientation)
            {
                case ConnectorOrientation.Top:
                    return dataItem.TopConnector;
                case ConnectorOrientation.Left:
                    return dataItem.LeftConnector;
                case ConnectorOrientation.Right:
                    return dataItem.RightConnector;
                case ConnectorOrientation.Bottom:
                    return dataItem.BottomConnector;

                default:
                    throw new InvalidOperationException(
                        string.Format("Found invalid persisted Connector Orientation for Connector Id: {0}", connectorId));
            }
        }

        private Type GetTypeOfDiagramItem(DesignerItemViewModelBase vmType)
        {
            //if (vmType is PersistDesignerItemViewModel)
            //    return typeof(PersistDesignerItem);
            //if (vmType is SettingsDesignerItemViewModel)
            //    return typeof(SettingsDesignerItem);
            //if (vmType is GroupingDesignerItemViewModel)
            //    return typeof(GroupDesignerItem);


            //throw new InvalidOperationException(string.Format("Unknown diagram type. Currently only {0} and {1} are supported",
            //    typeof(PersistDesignerItem).AssemblyQualifiedName,
            //    typeof(SettingsDesignerItemViewModel).AssemblyQualifiedName
            //    ));
            return typeof(int);
        }

        private DesignerItemViewModelBase GetConnectorDataItem(IDiagramViewModel diagramViewModel, int conectorDataItemId, Type connectorDataItemType)
        {
            DesignerItemViewModelBase dataItem = null;

            //if (connectorDataItemType == typeof(PersistDesignerItem))
            {
                dataItem = diagramViewModel.Items.OfType<PersistDesignerItemViewModel>().Single(x => x.Id == conectorDataItemId);
            }

            //if (connectorDataItemType == typeof(SettingsDesignerItem))
            {
                dataItem = diagramViewModel.Items.OfType<SettingsDesignerItemViewModel>().Single(x => x.Id == conectorDataItemId);
            }
            //if (connectorDataItemType == typeof(GroupDesignerItem))
            {
                dataItem = diagramViewModel.Items.OfType<GroupingDesignerItemViewModel>().Single(x => x.Id == conectorDataItemId);
            }
            return dataItem;
        }

        private bool ItemsToDeleteHasConnector(List<SelectableDesignerItemViewModelBase> itemsToRemove, FullyCreatedConnectorInfo connector)
        {
            return itemsToRemove.Contains(connector.DataItem);
        }



        private void DeleteFromDatabase(SelectableDesignerItemViewModelBase itemToDelete)
        {

            //make sure the item is removes from Diagram as well as removing them as individual items from database
            if (itemToDelete is PersistDesignerItemViewModel)
            {
                //DiagramItemData diagramItemToRemoveFromParent = wholeDiagramToAdjust.DesignerItems.Where(x => x.ItemId == itemToDelete.Id && x.ItemType == typeof(PersistDesignerItem)).Single();
                //wholeDiagramToAdjust.DesignerItems.Remove(diagramItemToRemoveFromParent);
                //databaseAccessService.DeletePersistDesignerItem(itemToDelete.Id);
            }
            if (itemToDelete is SettingsDesignerItemViewModel)
            {
                //DiagramItemData diagramItemToRemoveFromParent = wholeDiagramToAdjust.DesignerItems.Where(x => x.ItemId == itemToDelete.Id && x.ItemType == typeof(SettingsDesignerItem)).Single();
                //wholeDiagramToAdjust.DesignerItems.Remove(diagramItemToRemoveFromParent);
                //databaseAccessService.DeleteSettingDesignerItem(itemToDelete.Id);
            }
            if (itemToDelete is ConnectorViewModel)
            {
                //wholeDiagramToAdjust.ConnectionIds.Remove(itemToDelete.Id);
                //databaseAccessService.DeleteConnection(itemToDelete.Id);
            }

            //databaseAccessService.SaveDiagram(wholeDiagramToAdjust);


        }

    }
}
