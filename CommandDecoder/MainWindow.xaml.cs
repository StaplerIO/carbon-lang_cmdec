using CommandDecoder.Models;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommandDecoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PackageMetadata packageMetadata;
        private readonly DataTable commandTable;
        private readonly DataTable metadataTable;
        private readonly DataTable functionTable;

        public MainWindow()
        {
            InitializeComponent();

            packageMetadata = new PackageMetadata();

            commandTable = new DataTable();
            commandTable.Columns.Add("Location");
            commandTable.Columns.Add("Command");
            commandTable.Columns.Add("Description");
            CommandDataGrid.ItemsSource = commandTable.DefaultView;
            
            metadataTable = new DataTable();
            metadataTable.Columns.Add("Property");
            metadataTable.Columns.Add("Value");
            MetadataDataGrid.ItemsSource = metadataTable.DefaultView;

            functionTable = new DataTable();
            functionTable.Columns.Add("Index");
            functionTable.Columns.Add("Entry point");
            FunctionTableDataGrid.ItemsSource = functionTable.DefaultView;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            StatusBarText.Text = "Shutting down...";

            Application.Current.Shutdown();
        }

        private void OpenPackageButton_Click(object sender, RoutedEventArgs e)
        {
            StatusBarText.Text = "Opening file...";
            var dialog = new OpenFileDialog
            {
                Filter = "Carbon package|*.cbp"
            };
            dialog.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dialog.FileName))
            {
                // Update title bar
                Title = $"CommandDecoder - {dialog.SafeFileName}";

                var fileContent = File.ReadAllBytes(dialog.FileName);

                StatusBarText.Text = "Decoding commands...";
                var result = CbpManager.DecodePackage(fileContent);

                // Update command table
                commandTable.Clear();
                foreach (var command in result.CommandTable)
                {
                    string rawData = "";
                    foreach (var item in command.RawData)
                    {
                        var str = Convert.ToString(item, 16).PadLeft(2, '0');

                        rawData += str + ' ';
                    }

                    commandTable.Rows.Add($"0x{Convert.ToString(command.Location, 16).PadLeft(8, '0')} ({command.Location})", $"{rawData.ToUpper()}", command.Description);
                }

                // Update metadata table
                metadataTable.Clear();
                var metadataDisplayList = result.Metadata.GetMetadataList();
                foreach (var metadata in metadataDisplayList)
                {
                    metadataTable.Rows.Add(metadata.Key, metadata.Value);
                }

                // Update function table
                functionTable.Clear();
                foreach(var function in result.FunctionTable)
                {
                    functionTable.Rows.Add(function.FunctionId.ToString(), $"0x{string.Format("{0:x8}", function.EntryPointAddress)} ({function.EntryPointAddress})");
                }
                

                StatusBarText.Text = "Ready";
            }
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
                Content = $"Data slot alignment: {packageMetadata.DataSlotAlignment} \n" +
                          $"Data slignment: {packageMetadata.DataAlignment} \n" +
                          $"Address alignment: {packageMetadata.AddressAlignment} \n" +
                          $"Entry point address: {packageMetadata.EntryPointAddress} \n" +
                          $"Domain layer count alignment: {packageMetadata.DomainLayerCountAlignment}"
            }.ShowDialog();
        }

        private void Expander_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = MouseWheelEvent,
                    Source = sender
                };

                ContentTableScrollViewer.RaiseEvent(eventArg);
            }
        }
    }
}
