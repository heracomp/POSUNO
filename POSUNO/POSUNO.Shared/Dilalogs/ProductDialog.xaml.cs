using POSUNO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace POSUNO.Dilalogs
{
	public sealed partial class ProductDialog : ContentDialog
	{
		public ProductDialog(Product product)
		{
			this.InitializeComponent();
			Product = product;
			if (Product.IsEdit)
			{
				TitleTextBlock.Text = $"Editar el cliente:{Product.Name}";
			}
			else
			{
				TitleTextBlock.Text = "Nuevo producto";
				this.tbPrice.Text = "0";
				this.tbstock.Text = "0";

			}
		}
		public Product Product { get; set; }
		private void ColseImage_Tapped(object sender, TappedRoutedEventArgs e)
		{
			Hide();
		}
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Hide();
		}
		private async void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			bool isValid = await ValidateFormAsync();
			if (!isValid)
			{
				return;
			}
			Product.WasSaved = true;
			Hide();
		}

		private async Task<bool> ValidateFormAsync()
		{
			MessageDialog messageDialog;

			if (string.IsNullOrEmpty(Product.Name))
			{
				messageDialog = new MessageDialog("Debes ingresar un nombre del Producto.", "Error");
				await messageDialog.ShowAsync();
				return false;
			}
			if (string.IsNullOrEmpty(Product.Price.ToString()))
			{
				this.tbPrice.Text = "0";
			}
			if (string.IsNullOrEmpty(Product.stock.ToString()))
			{
				this.tbstock.Text = "0";
			}
			return true;
		}
	}
}
