using System;
using System.Collections.Generic;
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

namespace Tema3_PixelArt
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Brush color = Brushes.White;
        private bool isFirstTime = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void createPanel(int size)
        {
            pixelPanelGrid.Children.Clear();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Border bd = new Border();
                    bd.Style = (Style)this.Resources["borderPixelArt"];
                    pixelPanelGridBorder.BorderThickness = new Thickness(3);
                    pixelPanelGridBorder.BorderBrush = Brushes.Black;
                    pixelPanelGrid.Margin = new Thickness(0);
                    pixelPanelGrid.Children.Add(bd);
                }
            }
        }

        private bool isPanelEmpty()
        {
            foreach (var o in pixelPanelGrid.Children)
            {
                if (((Border)o).Background.ToString() != "#FFFFFFFF") return false;
            }
            return true;
        }
        private void resizePanel(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int size = int.Parse(b.Tag.ToString());
            if (!isFirstTime && !isPanelEmpty())
            {
                if (MessageBox.Show("¿Seguro que quieres perder tu dibujo?",
                    "Nuevo dibujo",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    createPanel(size);
                }
            }
            else
            {
                createPanel(size);
                isFirstTime = false;
            }
        }

        private void radioButtonColor_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Tag.ToString() == "Personalizado") colorPersonalizadoTextBox.IsEnabled = true;
            else cambioColor(rb.Tag.ToString(), false);
        }
        private void personalizadoRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            colorPersonalizadoTextBox.IsEnabled = false;
            personalizadoRadioButton.Foreground = Brushes.Black;
        }
        private void colorPersonalizadoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) cambioColor(((TextBox)sender).Text, true);
        }
        private void cambioColor(String colorNuevo, bool isPersonalized)
        {
            BrushConverter bc = new BrushConverter();
            try
            {
                color = (Brush)bc.ConvertFrom(colorNuevo);
                if (isPersonalized) personalizadoRadioButton.Foreground = (Brush)bc.ConvertFrom(colorNuevo);
            }
            catch (FormatException)
            {
                MessageBox.Show($"Código de color {colorNuevo} no válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void bordeBorder_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            ((Border)sender).Background = color;
        }

        private void bordeBorder_MouseEnter(object sender, RoutedEventArgs e)
        {
            Border b = (Border)sender;
            if (System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed)
                b.Background = color;
        }

        private void rellenarButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Object o in pixelPanelGrid.Children)
            {
                ((Border)o).Background = color;
            }
        }

        private void panelPersonalizadoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    int size = int.Parse(panelPersonalizadoTextBox.Text);
                    if ((size < 1 || size > 100) && errorNumero.Visibility == Visibility.Collapsed)
                    {
                        errorNumero.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        errorNumero.Visibility = Visibility.Collapsed;
                        createPanel(int.Parse(panelPersonalizadoTextBox.Text));
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show($"Número '{panelPersonalizadoTextBox.Text}' no válido.\n Introduce un número entero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void panelPersonalizadoTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            panelPersonalizadoTextBox.Foreground = Brushes.Black;
            panelPersonalizadoTextBox.Text = "";
        }

        private void panelPersonalizadoTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            panelPersonalizadoTextBox.Foreground = Brushes.LightGray;
            if (String.IsNullOrEmpty(panelPersonalizadoTextBox.Text))
                panelPersonalizadoTextBox.Text = "ej. 60";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if (b.ToolTip.ToString() == "Borde") pixelPanelGridBorder.BorderThickness = new Thickness(3);
            else pixelPanelGridBorder.BorderThickness = new Thickness(0);
        }
    }
}
