using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace PlanerDnia;

public partial class MainWindow : Window
{
    private List<Zadanie> zadania = new();

    public MainWindow()
    {
        InitializeComponent();
        AktualizujPodsumowanie();
    }

    private void DodajZadanie(object? sender, RoutedEventArgs e)
    {
        string nazwa = ZadanieTextBox.Text?.Trim() ?? "";
        string kategoria = (CategoryBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

        if (!string.IsNullOrWhiteSpace(nazwa) && !string.IsNullOrWhiteSpace(kategoria))
        {
            var zadanie = new Zadanie
            {
                Nazwa = nazwa,
                Kategoria = kategoria,
                CzyUkonczone = false
            };
            zadania.Add(zadanie);
            ZadaniaPanel.Children.Add(StworzPanelZadania(zadanie));
            ZadanieTextBox.Text = "";
            AktualizujPodsumowanie();
        }
    }

    private StackPanel StworzPanelZadania(Zadanie zadanie)
    {
        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
        };

        var text = new TextBlock
        {
            Text = zadanie.Nazwa,
            Width = 120,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
        };

        var combo = new ComboBox
        {
            Width = 120,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            SelectedItem = zadanie.Kategoria
        };
        combo.SelectionChanged += (_, _) =>
        {
            if (combo.SelectedItem is string selected)
                zadanie.Kategoria = selected;
        };

        var checkbox = new CheckBox
        {
            IsChecked = false,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
        };
        checkbox.Checked += (_, _) => { zadanie.CzyUkonczone = true; AktualizujPodsumowanie(); };
        checkbox.Unchecked += (_, _) => { zadanie.CzyUkonczone = false; AktualizujPodsumowanie(); };

        var usunPrzycisk = new Button
        {
            Content = "Usuń",
        };
        usunPrzycisk.Click += (_, _) =>
        {
            zadania.Remove(zadanie);
            ZadaniaPanel.Children.Remove(panel);
            AktualizujPodsumowanie();
        };

        panel.Children.Add(text);
        panel.Children.Add(combo);
        panel.Children.Add(checkbox);
        panel.Children.Add(usunPrzycisk);

        return panel;
    }

    private void AktualizujPodsumowanie()
    {
        int wszystkich = zadania.Count;
        int ukonczonych = zadania.Count(z => z.CzyUkonczone);
        PodsumowanieText.Text = $"Wszystkich: {wszystkich}, Ukończonych: {ukonczonych}";
    }
}