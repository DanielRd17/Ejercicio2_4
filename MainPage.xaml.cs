using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Threading.Tasks;


namespace Ejercicio2_4
{
    public partial class MainPage : ContentPage
    {
        private string videoFilePath;

        public MainPage()
        {
            InitializeComponent();
            RequestPermissions();
        }

        private async void RequestPermissions()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permissions Required", "Please grant storage permission to save the video.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Permission request failed: {ex.Message}", "OK");
            }
        }

        private async void OnRecordButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var options = new MediaPickerOptions
                {
                    Title = "Grabar video"
                };

                var videoFile = await MediaPicker.CaptureVideoAsync(options);
                if (videoFile != null)
                {
                    videoFilePath = videoFile.FullPath;
                    videoPreview.Source = videoFilePath;
                    // Habilitar el botón de guardar una vez que se haya grabado el video
                    btnSave.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unable to capture video: {ex.Message}", "OK");
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(videoFilePath))
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var fileName = Path.GetFileName(videoFilePath);
                var newFilePath = Path.Combine(documentsPath, fileName);

                try
                {
                    // Asegúrate de que el directorio existe
                    if (!Directory.Exists(documentsPath))
                    {
                        Directory.CreateDirectory(documentsPath);
                    }

                    File.Copy(videoFilePath, newFilePath);
                    await DisplayAlert("Éxito", "El video se ha guardado correctamente.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo guardar el video: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No hay video para guardar.", "OK");
            }
        }
    }
}