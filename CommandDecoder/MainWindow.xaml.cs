using CommandDecoder.Models;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CommandDecoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PackageMetadata packageMetadata;

        public MainWindow()
        {
            InitializeComponent();

            packageMetadata = new PackageMetadata();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenPackageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Carbon package|*.cbp";
            dialog.ShowDialog();

            var fileContent = File.ReadAllBytes(dialog.FileName);

            packageMetadata = CbpManager.ReadMetadata(fileContent);
        }

        private void ViewMetadataButton_Click(object sender, RoutedEventArgs e)
        {
            new TaskDialog
            {
                WindowTitle = "Package Metadata - cmdec",
                MainInstruction = "Package metadata",
                MainIcon = TaskDialogIcon.Information,
                Buttons = { new TaskDialogButton("Okay") },
                ButtonStyle = TaskDialogButtonStyle.CommandLinks,
                Content = $"Variable slot alignment: {packageMetadata.VariableSlotAlignment} \n" +
                $"Data slignment: {packageMetadata.DataAlignment} \n" +
                $"Command alignment: {packageMetadata.CommandAlignment} \n" +
                $"Entry point offset: {packageMetadata.EntryPointOffset} \n" +
                $"Domain layer count alignment: {packageMetadata.DomainLayerCountAlignment}"
            }.ShowDialog();
        }
    }
}
