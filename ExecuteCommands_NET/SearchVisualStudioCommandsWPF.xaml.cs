using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ExecuteCommands.Helpers;

namespace ExecuteCommands
{
    public partial class SearchVisualStudioCommandsWPF : Window
    {
        private List<CommandViewModel> _allCommands = new List<CommandViewModel>();
        private ICollectionView? _view;

        public SearchVisualStudioCommandsWPF()
        {
            InitializeComponent();
            LoadCommands();
        }

        private void LoadCommands()
        {
            string commandsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vs_commands.json");
            if (!File.Exists(commandsPath))
            {
                // Try looking up three levels (project root during dev)
                string devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "vs_commands.json");
                if (File.Exists(devPath))
                {
                    commandsPath = devPath;
                }
            }

            VisualStudioCommandLoader.LoadCommands(commandsPath);
            var commands = VisualStudioCommandLoader.GetCommands();

            _allCommands = commands.Select(c => new CommandViewModel(c)).ToList();
            
            ResultsList.ItemsSource = _allCommands;
            
            _view = CollectionViewSource.GetDefaultView(ResultsList.ItemsSource);
            _view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            _view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Category", System.ComponentModel.ListSortDirection.Ascending));
            _view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            
            // Initial filter: show nothing by default until the user types a search
            _view.Filter = item => false;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = SearchBox.Text.Trim();
            if (_view == null) return;

            if (string.IsNullOrWhiteSpace(filterText))
            {
                // No search text => show nothing
                _view.Filter = item => false;
            }
            else
            {
                _view.Filter = item =>
                {
                    if (item is CommandViewModel cmd)
                    {
                        return cmd.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                               cmd.NaturalLanguageExample.Contains(filterText, StringComparison.OrdinalIgnoreCase);
                    }
                    return false;
                };
            }

            // Refresh view so changes take effect immediately
            _view.Refresh();
        }

        private void CopyCommand_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsList.SelectedItem is CommandViewModel selected)
            {
                System.Windows.Clipboard.SetText(selected.NaturalLanguageExample);
                System.Windows.MessageBox.Show($"Copied to clipboard: \"{selected.NaturalLanguageExample}\"", "Copied", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a command to copy.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OpenFileLocation_Click(object sender, RoutedEventArgs e)
        {
            string commandsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vs_commands.json");
            if (File.Exists(commandsPath))
            {
                Process.Start("explorer.exe", "/select,\"" + commandsPath + "\"");
            }
            else
            {
                System.Windows.MessageBox.Show("vs_commands.json not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class CommandViewModel
    {
        public VisualStudioCommandInfo OriginalCommand { get; }
        public string Name => OriginalCommand.Name;
        public string Category { get; }
        public string NaturalLanguageExample { get; }
        public string BindingsDisplay { get; }
        public bool HasBindings => OriginalCommand.Bindings != null && OriginalCommand.Bindings.Count > 0;

        public CommandViewModel(VisualStudioCommandInfo command)
        {
            OriginalCommand = command;
            
            // Extract Category (e.g., "Edit.Copy" -> "Edit")
            int dotIndex = command.Name.IndexOf('.');
            Category = dotIndex > 0 ? command.Name.Substring(0, dotIndex) : "General";

            // Generate Natural Language Example
            // "Edit.Copy" -> "Edit Copy"
            // "Window.ApplyWindowLayout1" -> "Window Apply Window Layout 1"
            // Split by dot and camel case
            NaturalLanguageExample = GenerateNaturalLanguage(command.Name);

            BindingsDisplay = command.Bindings != null ? string.Join(", ", command.Bindings) : "";
        }

        private string GenerateNaturalLanguage(string name)
        {
            // Replace dots with spaces
            string text = name.Replace(".", " ");
            // Insert spaces before capital letters (simple version)
            // This is a basic heuristic.
            // "ApplyWindowLayout" -> "Apply Window Layout"
            
            // Better approach:
            // 1. Split by dot.
            // 2. For each part, split by camel case.
            
            var parts = name.Split('.');
            var naturalParts = parts.Select(SplitCamelCase);
            return string.Join(" ", naturalParts);
        }

        private string SplitCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            
            var result = new System.Text.StringBuilder();
            result.Append(input[0]);
            
            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]) && !char.IsUpper(input[i - 1]))
                {
                    result.Append(' ');
                }
                result.Append(input[i]);
            }
            return result.ToString();
        }
    }
}
