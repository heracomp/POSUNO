using POSUNO.Helpers;
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
	public sealed partial class CustomerDialog : ContentDialog
	{
		public CustomerDialog(Customer customer)
		{
			this.InitializeComponent();
			Customer = customer;
            if (Customer.IsEdit)
            {
				TitleTextBlock.Text = $"Editar el cliente:{Customer.FullName}";
            }
            else
            {
				TitleTextBlock.Text = "Nuevo cliente";
			}
		}
        public Customer Customer { get; set; }
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
			Customer.WasSaved = true;
			Hide();
		}

        private async Task<bool> ValidateFormAsync()
        {
			MessageDialog messageDialog;

            if (string.IsNullOrEmpty(Customer.FirstName))
            {
				messageDialog = new MessageDialog("Debes ingresar un nombre(s) del cliente.","Error");
				await messageDialog.ShowAsync();
				return false;
            }
			if (string.IsNullOrEmpty(Customer.LasttName))
			{
				messageDialog = new MessageDialog("Debes ingresar sus apellidos del cliente.", "Error");
				await messageDialog.ShowAsync();
				return false;
			}
			if (string.IsNullOrEmpty(Customer.Phonenumber))
			{
				messageDialog = new MessageDialog("Debes ingresar un teléfono del cliente.", "Error");
				await messageDialog.ShowAsync();
				return false;
			}
			if (string.IsNullOrEmpty(Customer.Address))
			{
				messageDialog = new MessageDialog("Debes ingresar la direección del cliente.", "Error");
				await messageDialog.ShowAsync();
				return false;
			}
			if (string.IsNullOrEmpty(Customer.Email))
			{
				messageDialog = new MessageDialog("Debes ingresar un email del cliente.", "Error");
				await messageDialog.ShowAsync();
				return false;
			}
			if (!RegexUtilities.IsValidEmail(Customer.Email))
			{
				messageDialog = new MessageDialog("Debes ingresar un email válido.", "Error");
				await messageDialog.ShowAsync();
				return false;
			}
			return true;
		}
	}
}
