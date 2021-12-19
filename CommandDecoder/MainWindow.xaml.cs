using CommandDecoder.Models;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Data;
using System.IO;
using System.Windows;

namespace CommandDecoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PackageMetadata packageMetadata;
        private DataTable commandTable;

        public MainWindow()
        {
            InitializeComponent();

            packageMetadata = new PackageMetadata();

            commandTable = new DataTable();
            commandTable.Columns.Add("Location");
            commandTable.Columns.Add("Command");
            commandTable.Columns.Add("Description");

            CommandDataGrid.ItemsSource = commandTable.DefaultView;
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

            var decodedCommands = CbpManager.DecodeCommands(fileContent[packageMetadata.EntryPointOffset..], packageMetadata);

            // Update command table
            decodedCommands.ForEach(command =>
            {
                string rawData = "";
                foreach (var item in command.RawData)
                {
                    var str = Convert.ToString(item, 16).PadLeft(2, '0');

                    rawData += str;
                }

                commandTable.Rows.Add($"0x{Convert.ToString(command.Location, 16).PadLeft(8, '0')} ({command.Location})", $"0x{rawData.ToUpper()}", command.Description);
            });
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
