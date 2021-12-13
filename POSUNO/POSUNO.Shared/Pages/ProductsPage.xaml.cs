using POSUNO.Components;
using POSUNO.Dilalogs;
using POSUNO.Helpers;
using POSUNO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
    public sealed partial class ProductsPage : Page
    {
        public ProductsPage()
        {
          this.InitializeComponent();
        }
        public ObservableCollection<Product> Products { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadProductsAsync();
        }

        private async void LoadProductsAsync()
        {
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            token = MainPage.GetInstance().TokenResponse.Token;

            Response response = await ApiService.GetListAsync<Product>("products",token);
            loader.Close();

            if (!response.IsSuccesss)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error al cargar los productos");
                await dialog.ShowAsync();
                return;
            }
            List<Product> products = (List<Product>)response.Result;
            Products = new ObservableCollection<Product>(products);
            RefreshList();
        }
        private string token;

        private void RefreshList()
        {
            this.ProductsListView.ItemsSource = null;
            this.ProductsListView.Items.Clear();
            this.ProductsListView.ItemsSource = Products;
        }
        private async void AddProductsButton_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            ProductDialog dialog = new ProductDialog(product);
            await dialog.ShowAsync();
            if (!product.WasSaved)
            {
                return;
            }

            product.User = MainPage.GetInstance().TokenResponse.User;

            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.PostAsync<Product>("Products", product, token);
            loader.Close();
            if (!response.IsSuccesss)
            {
                MessageDialog dialog1 = new MessageDialog(response.Message, "Error al anexar el producto");
                await dialog1.ShowAsync();
                return;
            }
            Product newproduct = (Product)response.Result;
            Products.Add(newproduct);
            RefreshList();
        }
        private async void EditImage_Click(object sender, TappedRoutedEventArgs e)
        {
            Product product = Products[ProductsListView.SelectedIndex];
            product.IsEdit = true;
            ProductDialog dialog = new ProductDialog(product);
            await dialog.ShowAsync();
            if (!product.WasSaved)
            {
                return;
            }

            product.User = MainPage.GetInstance().TokenResponse.User;

            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.PutAsync<Product>("Products", product, product.Id, token);
            loader.Close();
            if (!response.IsSuccesss)
            {
                MessageDialog dialog1 = new MessageDialog(response.Message, "Error en la edición del producto");
                await dialog1.ShowAsync();
                return;
            }
            Product newproduct = (Product)response.Result;
            Product oldproduct = Products.FirstOrDefault(c => c.Id == newproduct.Id);
            oldproduct = newproduct;
            RefreshList();
        }
        private async void DeleteImage_Click(object sender, TappedRoutedEventArgs e)
        {
            ContentDialogResult result = await ConfirmDeleteAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Product product = Products[ProductsListView.SelectedIndex];
            Response response = await ApiService.DeleteAsync("Products", product.Id, token);
            loader.Close();
            if (!response.IsSuccesss)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error al eliminar el producto");
                await dialog.ShowAsync();
                return;
            }
            List<Product> products = Products.Where(c => c.Id != product.Id).ToList();
            Products = new ObservableCollection<Product>(products);
            RefreshList();
        }

        private async Task<ContentDialogResult> ConfirmDeleteAsync()
        {
            ContentDialog confirmDialog = new ContentDialog()
            {
                Title = "Confirmación",
                Content = "¿Estás seguro que deseas eliminar el registro?",
                PrimaryButtonText = "Si",
                CloseButtonText = "No"
            };
            return await confirmDialog.ShowAsync();
        }
    }
}
