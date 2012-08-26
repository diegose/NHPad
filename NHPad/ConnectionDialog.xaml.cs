using System.Windows;
using System.Xml.Linq;
using LINQPad.Extensibility.DataContext;

namespace NHPad
{
    public partial class ConnectionDialog
    {
        public ConnectionDialog(IConnectionInfo cxInfo)
        {
            InitializeComponent();
            DataContext = new DriverDataWrapper(cxInfo.DriverData);
        }

        private void HandleOk(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }

    public class DriverDataWrapper
    {
        readonly XElement driverData;

        public DriverDataWrapper(XElement driverData)
        {
            this.driverData = driverData;
        }

        public string ClientAssembly
        {
            get { return (string)driverData.Element("ClientAssembly"); }
            set { driverData.SetElementValue("ClientAssembly", value); }
        }
    }
}
