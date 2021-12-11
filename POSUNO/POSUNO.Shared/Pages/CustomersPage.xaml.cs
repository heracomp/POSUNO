using POSUNO.Components;
using POSUNO.Dilalogs;
using POSUNO.Helpers;
using POSUNO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace POSUNO.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomersPage : Page
    {
        public CustomersPage()
        {
            InitializeComponent();
        }
        public ObservableCollection<Customer> Customers { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadCustomersAsync();
        }

        private async void LoadCustomersAsync()
        {
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.GetListAsync<Customer>("customers");
            loader.Close();

            if (!response.IsSuccesss)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error al cargar los clientes");
                await dialog.ShowAsync();
                return;
            }
            List<Customer> customer = (List<Customer>)response.Result;
            Customers = new ObservableCollection<Customer>(customer);
            RefreshList();
        }

        private void RefreshList()
        {
            CustomersListView.ItemsSource = null;
            CustomersListView.Items.Clear();
            CustomersListView.ItemsSource = Customers;
        }
        private async void AddCustoerButton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer();
            CustomerDialog dialog = new CustomerDialog(customer);
            await dialog.ShowAsync();
            if (!customer.WasSaved)
            {
                return;
            }

            customer.User = MainPage.GetInstance().User;

            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.PostAsync<Customer>("Customers", customer);
            loader.Close();
            if (!response.IsSuccesss)
            {
                MessageDialog dialog1 = new MessageDialog(response.Message, "Error al anexar el cliente");
                await dialog1.ShowAsync();
                return;
            }
            Customer newcustomer=(Customer)response.Result;
            Customers.Add(newcustomer);
            RefreshList();
        }
        private async void EditImage_Click(object sender, TappedRoutedEventArgs e)
        {
            Customer customer = Customers[CustomersListView.SelectedIndex];
            customer.IsEdit = true;
            CustomerDialog dialog = new CustomerDialog(customer);
            await dialog.ShowAsync();
            if (!customer.WasSaved)
            {
                return;
            }

            customer.User = MainPage.GetInstance().User;

            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.PutAsync<Customer>("Customers", customer,customer.Id);
            loader.Close();
            if (!response.IsSuccesss)
            {
                MessageDialog dialog1 = new MessageDialog(response.Message, "Error en la edición del cliente");
                await dialog1.ShowAsync();
                return;
            }
            Customer newcustomer = (Customer)response.Result;
            Customer oldcustomer = Customers.FirstOrDefault(c => c.Id == newcustomer.Id);
            oldcustomer = newcustomer;
            RefreshList();
        }
    }
}
