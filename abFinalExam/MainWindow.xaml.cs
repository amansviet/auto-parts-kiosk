using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace abFinalExam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentItemSelected = -1;
        private int ordertotal = 0;
        private bool? shipToHome = false;

        //A List to store items for an order
        List<Item> ordersList;
        public MainWindow()
        {
            InitializeComponent();
            ordersList = new List<Item>();
            SetStatus("initialized","Please Start by selecting an item and submit an order");
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ResetGuiToInitialState(false);
                currentItemSelected = cbItemSelection.SelectedIndex;
                switch (currentItemSelected)
                {
                    case 0:
                        Console.WriteLine("Tire Selected");
                        tbTireModelName.IsEnabled = true;
                        tbWheelDiameter.IsEnabled = true;
                        tbWiperLength.IsEnabled = false;
                        tbBatteryVoltage.IsEnabled = false;
                        chbShipToHome.IsEnabled = false;
                        break;
                    case 1:
                        Console.WriteLine("Wiper Selected");
                        tbWiperLength.IsEnabled = true;
                        tbTireModelName.IsEnabled = false;
                        tbWheelDiameter.IsEnabled = false;
                        tbBatteryVoltage.IsEnabled = false;
                        chbShipToHome.IsEnabled = true;
                        break;
                    case 2:
                        Console.WriteLine("Battery Selected");
                        tbBatteryVoltage.IsEnabled = true;
                        tbWiperLength.IsEnabled = false;
                        tbTireModelName.IsEnabled = false;
                        tbWheelDiameter.IsEnabled = false;
                        chbShipToHome.IsEnabled = true;
                        break;
                }
            }
            catch(Exception exception)
            {
                SetStatus("failed", $"Exception: {exception.Message}\n\nComboBox Selection Failed\nPlease Try Again !!!");
            }          
        }
        private void BtnSubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            try {
                shipToHome = chbShipToHome.IsChecked;
                if (currentItemSelected == -1)
                    {
                        SetStatus("invalid", "Please select an item from dropdown above");
                        return;
                    }
                switch (currentItemSelected)
                {
                  case 0:
                    if (String.IsNullOrWhiteSpace(tbItemName.Text) || String.IsNullOrWhiteSpace(tbTireModelName.Text) || String.IsNullOrWhiteSpace(tbWheelDiameter.Text))
                    {
                        SetStatus("invalid", "Required Fields cannot be empty");
                        return;
                    }
                    else
                    {
                        // Add a new Tire object to orderlist
                        Tire tire = new Tire();
                        tire.ItemName = tbItemName.Text;
                        tire.TireModelName = tbTireModelName.Text;
                        tire.WheelDiameter = Int32.Parse(tbWheelDiameter.Text);
                        ordersList.Add(tire);
                        SetStatus("success", "Tire successfully added to your order");
                        UpdateOrderTotal(tire.Cost);
                    }
                    break;
                  case 1:
                    if (String.IsNullOrWhiteSpace(tbItemName.Text) || String.IsNullOrWhiteSpace(tbWiperLength.Text))
                    {
                        SetStatus("invalid", "Required Fields cannot be empty");
                        return;
                    }
                    else
                    {
                        // Add a new Wiper object to orderlist
                        Wiper wiper = new Wiper();
                        wiper.ItemName = tbItemName.Text;
                        wiper.WiperLength = Int32.Parse(tbWiperLength.Text);
                        if ((bool)shipToHome)
                        {
                            wiper.Ship = true;
                            wiper.Cost += wiper.ShipItem();
                        }

                        ordersList.Add(wiper);
                        SetStatus("success", "Wiper successfully added to your order");
                        UpdateOrderTotal(wiper.Cost);
                    }
                    break;
                  case 2:
                    if (String.IsNullOrWhiteSpace(tbItemName.Text) || String.IsNullOrWhiteSpace(tbBatteryVoltage.Text))
                    {
                        SetStatus("invalid", "Required Fields cannot be empty");
                        return;
                    }
                    else
                    {
                        // Add a new Battery object to orderlist
                        Battery battery = new Battery();
                        battery.ItemName = tbItemName.Text;
                        battery.Voltage = Int32.Parse(tbBatteryVoltage.Text);
                        if ((bool)shipToHome)
                        {
                            battery.Ship = true;
                            battery.Cost += battery.ShipItem();
                        }
                        ordersList.Add(battery);
                        SetStatus("success", "Battery successfully added to your order");
                        UpdateOrderTotal(battery.Cost);
                    }
                    break;
            }
            ResetGuiToInitialState(false);
        } 
        catch (Exception exception) 
            {
                SetStatus("failed", $"Exception: {exception.Message}\n\nSubmit Order failed.\nPlease Try Again !!!");
            }
        }
        private void SetStatus(string status, String resultText)
        {
            if (status == "failed" || status == "invalid")
            {
                lblStatus.Content = "Error";
                lblStatus.Background = new SolidColorBrush(Colors.OrangeRed);
                tbResult.Text = resultText;
            }
            else if (status == "success")
            {
                lblStatus.Content = "Success";
                lblStatus.Background = new SolidColorBrush(Colors.Green);
                tbResult.Text = resultText;
            }
            else
            {
                lblStatus.Content = "Info";
                tbResult.Text = resultText;
                lblStatus.Background = new SolidColorBrush(Colors.White);
            }
        }
        private void UpdateOrderTotal(int cost)
        {
            ordertotal += cost;
            lblTotalCostOfOrders.Content = ordertotal;
        }
        private void ResetGuiToInitialState(bool resetTotal = true) 
        {
            try 
            {
                tbItemName.Text = "";
                tbTireModelName.Text = "";
                tbWheelDiameter.Text = "";
                tbWiperLength.Text = "";
                tbBatteryVoltage.Text = "";
                chbShipToHome.IsChecked = false;
                if (resetTotal)
                {
                    lblTotalCostOfOrders.Content = "0";
                }
            }
            catch(Exception exception) 
            {
                SetStatus("failed", $"Exception: {exception.Message}\n\nReset GUI Failed\nPlease Try Again !!!");
            }
        }
        private void BtnLinqQuery_Click(object sender, RoutedEventArgs e)
        {
            var tbLinqQueryResultsStringBuilder = new System.Text.StringBuilder();
            try
            {   
                // Get the {item,count} from the order list and then check for items with highest count
                // This helps in getting all items and their counts to find if multiple items exits with max count
                var query = from Object order in ordersList
                            group order by order.GetType().Name into itemType
                            select new
                            {
                                item = itemType.Key,
                                count = itemType.Count(),
                            };
                if (query.Count() < 1)
                {
                    SetStatus("info", "");
                    String resultString = "Results for LINQ:\n\nNo orders found, try submitting some orders and try again";
                    tbLinqQueryResultsStringBuilder.AppendLine(resultString);
                    tbLinqQueryResults.Text = tbLinqQueryResultsStringBuilder.ToString();
                    return;
                }
                else
                {
                    String resultString = "Results for LINQ:\n\nThe most ordered item is/are: ";
                    tbLinqQueryResultsStringBuilder.AppendLine(resultString);

                    int max = 0;
                    foreach (var order in query)
                    {
                        if (order.count > max)
                        {
                            max = order.count;
                        }
                    }
                    foreach (var order in query)
                    {
                        if (order.count == max)
                        {
                            tbLinqQueryResultsStringBuilder.AppendLine($"{order.item} : {order.count}");
                        }
                    }
                    tbLinqQueryResults.Text = tbLinqQueryResultsStringBuilder.ToString();
                    SetStatus("success","You just executed a LINQ query successfully");
                }
            }
            catch (Exception exception)
            {
                SetStatus("failed","Try placing some orders and then try running LINQ Query again");
                tbLinqQueryResults.Text = $"Exception: {exception.Message}\nPlease Try Again !!!";
            }
        }
        private void MnuSaveOrders_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ordersList.Count == 0) 
                {
                    SetStatus("failed", "No orders places that can be saved.\n" +
                        "\nPlease submit atleast one order before saving");
                    return;
                }
                TextWriter writer = new StreamWriter("orders_list.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Item>));
                serializer.Serialize(writer, ordersList);
                writer.Close();

                SetStatus("success", "Orders saved successfully.\n\nPlease check the xml file at /bin/debug/orders_list.xml");
            }
            catch (Exception exception)
            {
                SetStatus("failed", $"Exception: {exception.Message}\n\nSaving Orders failed.\nPlease Try Again !!!");
            }
        }
        private void MnuLoadOrders_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetGuiToInitialState();
                // Check whether a valid xml file exists in bin/debug folder of the application
                string xmlFileToRead = "orders_list.xml";
                if (!File.Exists(xmlFileToRead))
                {
                    SetStatus("failed", $"File {xmlFileToRead} does not exist or is not a valid xml file\nIf you havent saved an order yet, please try submitting an order first");
                    return;
                }
                this.ordersList = null;
                // Desrialize the xml file into a item list object
                XmlSerializer serializer = new XmlSerializer(typeof(List<Item>));
                StreamReader streamReader = new StreamReader(xmlFileToRead);
                this.ordersList = (List<Item>)serializer.Deserialize(streamReader);
                streamReader.Close();

                // reset link query result text if any 
                tbLinqQueryResults.Text = "";
                SetTotalOnFileLoad();
                SetStatus("success", "Orders loaded successfully." +
                    $"\nThere are {ordersList.Count} items in your ordersList" +
                    $"\n\nPlease check the xml file at /bin/debug/orders_list.xml");
            }
            catch (Exception exception)
            {
                SetStatus("failed", $"Exception: {exception.Message}\n\nLoading of orders failed.\nPlease Try Again !!!");
            }
        }
        private void SetTotalOnFileLoad() {
            try 
            {
                int totalOrdersCost = 0;
                foreach (var order in ordersList)
                {
                    totalOrdersCost += order.Cost;
                }
                lblTotalCostOfOrders.Content = totalOrdersCost;
                ordertotal = totalOrdersCost;
                return;
            }
            catch(Exception exception) 
            {
                SetStatus("failed", $"Exception: {exception.Message}\n\nLoading of Grand total failed.\nPlease Try Again !!!");
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
