using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Crypt
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private FileResult selectedFile;
        private readonly string fixedDirectoryPath = "/storage/emulated/0/Download";
        public MainPage()
        {
            InitializeComponent();
            Directory.CreateDirectory(fixedDirectoryPath); // Створення директорії, якщо вона не існує
        }


        private async void PickFileButton_Clicked(object sender, EventArgs e)
        {
            selectedFile = await PickFileAsync();
            if (selectedFile != null)
            {
                fileNameLabel.Text = selectedFile.FileName;
            }
        }

        private async void EncryptFileButton_Clicked(object sender, EventArgs e)
        {
            if (selectedFile == null)
            {
                await DisplayAlert("Помилка", "Будь ласка, виберіть файл", "OK");
                return;
            }

            string password = passwordEntry.Text;
            if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Помилка", "Будь ласка, введіть пароль", "OK");
                return;
            }

            string outputFile = Path.Combine(fixedDirectoryPath, "En_" + Path.GetFileName(selectedFile.FullPath));
            FileEncryptor.EncryptFile(selectedFile.FullPath, outputFile, password);
            statusLabel.Text = $"Файл зашифровано: {outputFile}";
            passwordEntry.Text = "";
        }

        private async void DecryptFileButton_Clicked(object sender, EventArgs e)
        {
            if (selectedFile == null)
            {
                await DisplayAlert("Помилка", "Будь ласка, виберіть файл", "OK");
                return;
            }

            string password = passwordEntry.Text;
            if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Помилка", "Будь ласка, введіть пароль", "OK");
                return;
            }

            string outputFile = Path.Combine(fixedDirectoryPath, "De_" + Path.GetFileName(selectedFile.FullPath));
            FileEncryptor.DecryptFile(selectedFile.FullPath, outputFile, password);
            statusLabel.Text = $"Файл розшифровано: {outputFile}";
            passwordEntry.Text = "";
        }

        private async Task<FileResult> PickFileAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", "Не вдалося вибрати файл: " + ex.Message, "OK");
            }
            return null;
        }
    }
}
