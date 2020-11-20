using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using AutoLotModel;
using System.Data.Entity;
using System.Data;
using System.ComponentModel;

namespace Marian_Bianca_Lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //lab6part21
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {//lab6part21
        ActionState action = ActionState.Nothing;
        //lab6part23
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource customerViewSource;
        CollectionViewSource inventoryViewSource;
        CollectionViewSource customerOrdersViewSource;
        Binding firstNameTextBoxBinding = new Binding();
        Binding lastNameTextBoxBinding = new Binding();
        Binding colorTextBoxBinding = new Binding();
        Binding makeTextBoxBinding = new Binding();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            firstNameTextBoxBinding.Path = new PropertyPath("FirstName");
            lastNameTextBoxBinding.Path = new PropertyPath("LastName");
            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBoxBinding);
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBoxBinding);

            colorTextBoxBinding.Path = new PropertyPath("Color");
            makeTextBoxBinding.Path = new PropertyPath("Make");
            colorTextBox.SetBinding(TextBox.TextProperty, colorTextBoxBinding);
            makeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
           customerViewSource.Source = ctx.Customers.Local;
            ctx.Customers.Load();
           inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // inventoryViewSource.Source = [generic data source]
            inventoryViewSource.Source = ctx.Inventories.Local;
            ctx.Inventories.Load();
            customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            //customerOrdersViewSource.Source = ctx.Orders.Local;
            ctx.Orders.Load();
            ctx.Inventories.Load();

            cmbCustomers.ItemsSource = ctx.Customers.Local;
            //cmbCustomers.DisplayMemberPath = "FirstName";
            cmbCustomers.SelectedValuePath = "CustId";

            cmbInventory.ItemsSource = ctx.Inventories.Local;
            //cmbInventory.DisplayMemberPath = "Make";
            cmbInventory.SelectedValuePath = "CardId";
            BindDataGrid();
        }
        //lab6part24
        private void btnSave_Click_C(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    customer = new Customer()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim(),
                    };
                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew_C.IsEnabled = true;
                btnEdit_C.IsEnabled = true;
                btnCancel_C.IsEnabled = false;
                btnPrevious_C.IsEnabled = true;
                btnNext_C.IsEnabled = true;
                lastNameTextBox.IsEnabled = false;
                firstNameTextBox.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBoxBinding);
                lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBoxBinding);



            }
            else
                if (action == ActionState.Edit)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                customerViewSource.View.MoveCurrentTo(customer);
                btnNew_C.IsEnabled = true;
                btnEdit_C.IsEnabled = true;
                btnDelete_C.IsEnabled = true;

                btnCancel_C.IsEnabled = true;
                btnPrevious_C.IsEnabled = true;
                btnNext_C.IsEnabled = true;
                lastNameTextBox.IsEnabled = false;
                firstNameTextBox.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBoxBinding);
                lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBoxBinding);




            }
            else
                if (action == ActionState.Delete)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();

                btnNew_C.IsEnabled = true;
                btnEdit_C.IsEnabled = true;
                btnDelete_C.IsEnabled = true;

                btnCancel_C.IsEnabled = true;
                btnPrevious_C.IsEnabled = true;
                btnNext_C.IsEnabled = true;
                lastNameTextBox.IsEnabled = false;
                firstNameTextBox.IsEnabled = false;

                lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBoxBinding);
                firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBoxBinding);

            }
        }
        //lab6part25
        private void btnNext_Click_C(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }

        private void btnPrevious_Click_C(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNew_Click_C(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew_C.IsEnabled = false;
            btnEdit_C.IsEnabled = false;
            btnDelete_C.IsEnabled = false;

    
            btnCancel_C.IsEnabled = true;
            btnNext_C.IsEnabled = false;
            btnPrevious_C.IsEnabled = false;
            lastNameTextBox.IsEnabled = true;
            firstNameTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
           // customerDataGrid.SelectedItem = null;

            SetValidationBinding();

        }

        private void btnEdit_Click_C(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;


            btnNew_C.IsEnabled = false;
            btnEdit_C.IsEnabled = false;
            btnDelete_C.IsEnabled = false;

           
            btnCancel_C.IsEnabled = true;
            btnPrevious_C.IsEnabled = false;
            btnNext_C.IsEnabled = false;
            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);

            SetValidationBinding();
        }

        private void btnDelete_Click_C(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();

            btnNew_C.IsEnabled = false;
            btnEdit_C.IsEnabled = false;
            btnDelete_C.IsEnabled = false;

            btnSave_C.IsEnabled = true;
            btnCancel_C.IsEnabled = true;
            btnPrevious_C.IsEnabled = false;
            btnNext_C.IsEnabled = false;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;

        }

        private void btnCancel_Click_C(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            btnNew_C.IsEnabled = true;
            btnEdit_C.IsEnabled = true;
            btnEdit_C.IsEnabled = true;

            btnSave_C.IsEnabled = false;
            btnCancel_C.IsEnabled = false;
            btnPrevious_C.IsEnabled = true;
            btnNext_C.IsEnabled = true;
            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;

            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBoxBinding);
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBoxBinding);
        }

        private void btnNew_Click_I(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew_I.IsEnabled = false;
            btnEdit_I.IsEnabled = false;
            btnDelete_I.IsEnabled = false;

            btnSave_I.IsEnabled = true;
            btnCancel_I.IsEnabled = true;
            btnNext_I.IsEnabled = false;
            btnPrevious_I.IsEnabled = false;
            colorTextBox.IsEnabled = true;
            makeTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            colorTextBox.Text = "";
            makeTextBox.Text = "";
        }

        private void btnEdit_Click_I(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempColor = colorTextBox.Text.ToString();
            string tempMake = makeTextBox.Text.ToString();

            btnNew_I.IsEnabled = false;
            btnEdit_I.IsEnabled = false;
            btnDelete_I.IsEnabled = false;

            btnSave_I.IsEnabled = true;
            btnCancel_I.IsEnabled = true;
            btnPrevious_I.IsEnabled = false;
            btnNext_I.IsEnabled = false;
            colorTextBox.IsEnabled = true;
            makeTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            colorTextBox.Text = tempColor;
            makeTextBox.Text = tempMake;

        }

        private void btnDelete_Click_I(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempColor = colorTextBox.Text.ToString();
            string tempMake = makeTextBox.Text.ToString();

            btnNew_I.IsEnabled = false;
            btnEdit_I.IsEnabled = false;
            btnDelete_I.IsEnabled = false;

            btnSave_I.IsEnabled = true;
            btnCancel_I.IsEnabled = true;
            btnPrevious_I.IsEnabled = false;
            btnNext_I.IsEnabled = false;

            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            colorTextBox.Text = tempColor;
            makeTextBox.Text = tempMake;
        }

        private void btnSave_Click_I(object sender, RoutedEventArgs e)
        {
            Inventory inventory = null;
            if (action == ActionState.New)
            {
                try
                {
                    inventory = new Inventory()
                    {
                        Color = colorTextBox.Text.Trim(),
                        Make = makeTextBox.Text.Trim(),
                    };
                    ctx.Inventories.Add(inventory);
                    inventoryViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew_I.IsEnabled = true;
                btnEdit_I.IsEnabled = true;
                btnSave_I.IsEnabled = false;
                btnCancel_I.IsEnabled = false;
                btnPrevious_I.IsEnabled = true;
                btnNext_I.IsEnabled = true;
                colorTextBox.IsEnabled = false;
                makeTextBox.IsEnabled = false;


            }
            else
                if (action == ActionState.Edit)
                {
                 try
                 {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    inventory.Color = colorTextBox.Text.Trim();
                    inventory.Make = makeTextBox.Text.Trim();
                    ctx.SaveChanges();
                 }
                 catch (DataException ex)
                 {
                    MessageBox.Show(ex.Message);
                 }
                 inventoryViewSource.View.Refresh();
                 inventoryViewSource.View.MoveCurrentTo(inventory);
                 btnNew_I.IsEnabled = true;
                 btnEdit_I.IsEnabled = true;
                 btnDelete_I.IsEnabled = true;
                 btnSave_I.IsEnabled = false;
                 btnCancel_I.IsEnabled = true;
                 btnPrevious_I.IsEnabled = true;
                 btnNext_I.IsEnabled = true;
                 colorTextBox.IsEnabled = false;
                 makeTextBox.IsEnabled = false;

                 colorTextBox.SetBinding(TextBox.TextProperty, colorTextBoxBinding);
                 makeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);
                }
            else
                if (action == ActionState.Delete)
                {
                 try
                 {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    ctx.Inventories.Remove(inventory);
                    ctx.SaveChanges();
                 }
                 catch (DataException ex)
                 {
                    MessageBox.Show(ex.Message);
                 }
                 inventoryViewSource.View.Refresh();

                 btnNew_I.IsEnabled = true;
                 btnEdit_I.IsEnabled = true;
                 btnDelete_I.IsEnabled = true;
                 btnSave_I.IsEnabled = false;
                 btnCancel_I.IsEnabled = true;
                 btnPrevious_I.IsEnabled = true;
                 btnNext_I.IsEnabled = true;
                 colorTextBox.IsEnabled = false;
                 makeTextBox.IsEnabled = false;

                 colorTextBox.SetBinding(TextBox.TextProperty, colorTextBoxBinding);
                 makeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);

                }
        }

            private void btnCancel_Click_I(object sender, RoutedEventArgs e)
            {
                action = ActionState.Nothing;

                btnNew_I.IsEnabled = true;
                btnEdit_I.IsEnabled = true;
                btnEdit_I.IsEnabled = true;

                btnSave_I.IsEnabled = false;
                btnCancel_I.IsEnabled = false;
                btnPrevious_I.IsEnabled = true;
                btnNext_I.IsEnabled = true;
                colorTextBox.IsEnabled = false;
                makeTextBox.IsEnabled = false;

                colorTextBox.SetBinding(TextBox.TextProperty, colorTextBoxBinding);
                makeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);
            }
        private void btnPrevious_Click_I(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNext_Click_I(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToNext();
        }

        private void btnNew_Click_O(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew_O.IsEnabled = false;
            btnEdit_O.IsEnabled = false;
            btnDelete_O.IsEnabled = false;

            btnSave_O.IsEnabled = true;
            btnCancel_O.IsEnabled = true;
            btnNext_O.IsEnabled = false;
            btnPrevious_O.IsEnabled = false;


        }

        private void btnEdit_Click_O(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);

            btnNew_O.IsEnabled = false;
            btnEdit_O.IsEnabled = false;
            btnDelete_O.IsEnabled = false;

            btnSave_O.IsEnabled = true;
            btnCancel_O.IsEnabled = true;
            btnPrevious_O.IsEnabled = false;
            btnNext_O.IsEnabled = false;

        }

        private void btnDelete_Click_O(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;

            btnNew_O.IsEnabled = false;
            btnEdit_O.IsEnabled = false;
            btnDelete_O.IsEnabled = false;

            btnSave_O.IsEnabled = true;
            btnCancel_O.IsEnabled = true;
            btnPrevious_O.IsEnabled = false;
            btnNext_O.IsEnabled = false;

            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
        }

        private void btnSave_Click_O(object sender, RoutedEventArgs e)
        {
            Order order = null;
            if(action == ActionState.New)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory inventory = (Inventory)cmbInventory.SelectedItem;
                    order = new Order()
                    {
                        CustId = customer.CustId,
                        CardId = inventory.CardId
                    };
                    ctx.Orders.Add(order);
                    customerOrdersViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch(DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew_O.IsEnabled = true;
                btnEdit_O.IsEnabled = true;
                btnSave_O.IsEnabled = false;
                btnCancel_O.IsEnabled = false;
                btnPrevious_O.IsEnabled = true;
                btnNext_O.IsEnabled = true;
            }
            else
                if (action == ActionState.Edit)
                {
                dynamic selectedOrder = ordersDataGrid.SelectedItem;
                 try
                 {
                    int curr_id = selectedOrder.OrderId;
                    var editedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (editedOrder != null)
                    {
                        editedOrder.CustId = Int32.Parse(cmbCustomers.SelectedValue.ToString());
                        editedOrder.CardId = Convert.ToInt32(cmbInventory.SelectedValue.ToString());
                        ctx.SaveChanges();
                    }
                 }
                  catch(DataException ex)
                  {
                    MessageBox.Show(ex.Message);
                  }
                 BindDataGrid();
                 customerViewSource.View.MoveCurrentTo(selectedOrder);
                SetValidationBinding();
                }
            else
                if (action == ActionState.Delete)
                {
                 try
                 {
                    dynamic selectedOrder = ordersDataGrid.SelectedItem;
                    int curr_id = selectedOrder.OrderId;
                    var deletedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (deletedOrder != null)
                    {
                        ctx.Orders.Remove(deletedOrder);
                        ctx.SaveChanges();
                        MessageBox.Show("Order Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                 catch(DataException ex)
                 {
                     MessageBox.Show(ex.Message);
                 }
            }
        }

        private void btnCancel_Click_O(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            btnNew_O.IsEnabled = true;
            btnEdit_O.IsEnabled = true;
            btnEdit_O.IsEnabled = true;

            btnSave_O.IsEnabled = false;
            btnCancel_O.IsEnabled = false;
            btnPrevious_O.IsEnabled = true;
            btnNext_O.IsEnabled = true;


        }

        private void btnPrevious_Click_O(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNext_Click_O(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToNext();
        }
        private void BindDataGrid()
        {
            var queryOrder = from ord in ctx.Orders
                             join cust in ctx.Customers on ord.CustId equals cust.CustId join inv in ctx.Inventories on ord.CardId equals inv.CardId
                             select new
                             {
                                 ord.OrderId,
                                 ord.CardId,
                                 ord.CustId,
                                 cust.FirstName,
                                 cust.LastName,
                                 inv.Make,
                                 inv.Color
                             };
            customerOrdersViewSource.Source = queryOrder.ToList();
        }

        private void SetValidationBinding()
        {
            Binding firstNameValidationBinding = new Binding();
            firstNameValidationBinding.Source = customerViewSource;
            firstNameValidationBinding.Path = new PropertyPath("FirstName");
            firstNameValidationBinding.NotifyOnValidationError = true;
            firstNameValidationBinding.Mode = BindingMode.TwoWay;
            firstNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string required
            firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            firstNameTextBox.SetBinding(TextBox.TextProperty,firstNameValidationBinding);
            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = customerViewSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            lastNameTextBox.SetBinding(TextBox.TextProperty,
           lastNameValidationBinding);
        }
    }
}

