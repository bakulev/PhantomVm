using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiagramDesigner;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Cc.Anba.PhantomOs.Apps.VmUi
{
    public class Property : INPCBase
    {

        public Property(string propertyName, string propertyValue)
        {
            _propertyName = propertyName;
            _propertyValue = propertyValue;
        }

        private string _propertyName = "";

        public string PropertyName
        {
            get => _propertyName;
            set
            {
                if (_propertyName != value)
                {
                    _propertyName = value;
                    NotifyChanged("PropertyName");
                }
            }
        }

        private string _propertyValue = "";

        public string PropertyValue
        {
            get => _propertyValue;
            set
            {
                if (_propertyValue != value)
                {
                    _propertyValue = value;
                    NotifyChanged("PropertyValue");
                }
            }
        }
    }

    public class PersistDesignerItemViewModel : DesignerItemViewModelBase, ISupportDataChanges
    {
        private IUIVisualizerService visualiserService;

        public PersistDesignerItemViewModel(int id, IDiagramViewModel parent, double left, double top, string descriptionText) : base(id,parent, left,top)
        {
            this.DescriptionText = descriptionText;
            Init();
        }
        public PersistDesignerItemViewModel(int id, IDiagramViewModel parent, double left, double top, double itemWidth, double itemHeight, string descriptionText) : base(id, parent, left, top, itemWidth, itemHeight)
        {
            this.DescriptionText = descriptionText;
            Init();
        }

        public PersistDesignerItemViewModel() : base()
        {
            Init();
        }


        public String DescriptionText { get; set; }

        public ObservableCollection<Property> Departments { get; set; } = new ObservableCollection<Property>();

        public ICommand ShowDataChangeWindowCommand { get; private set; }

        public void ExecuteShowDataChangeWindowCommand(object parameter)
        {
            PersistDesignerItemData data = new PersistDesignerItemData(DescriptionText);
            if (visualiserService.ShowDialog(data) == true)
            {
                this.DescriptionText = "data.DescriptionText";
            }
        }

        private double _contentWitdh;

        public double ContentWitdh {
            set
            {
                _contentWitdh = value;
                System.Diagnostics.Debug.WriteLine("ContentWitdh = ", _contentWitdh);
            }
        }

        private void Init()
        {

            visualiserService = ApplicationServicesProvider.Instance.Provider.VisualizerService;
            ShowDataChangeWindowCommand = new SimpleCommand(ExecuteShowDataChangeWindowCommand);
            this.ShowConnectors = false;

            Departments.Add(new Property("Prop 1", "VALUE 1"));
            Departments.Add(new Property("Prop 2", "VALUE 2 long"));
            Departments.Add(new Property("Prop 3", "VALUE 3"));
        }
    }
}
