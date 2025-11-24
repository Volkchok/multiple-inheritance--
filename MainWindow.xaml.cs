
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfApp1
{
    public interface IEngine
    {
        double GetPrice();
        void SetPrice(double price);
    }

    public interface ITurboEngine
    {
        double GetPressure();
        void SetPressure(double pressure);
    }

    public interface IInternalCombustionEngine
    {
        double GetVolume();
        void SetVolume(double volume);
    }

    public partial class MainWindow : Window
    {
        private List<IEngine> engines = new List<IEngine>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAddTurboEngine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TurboEngine newEngine = new TurboEngine
                {
                    Horsepower = GetPositiveDoubleFromTextBox(txtTurboHorsepower),
                    Weight = GetPositiveDoubleFromTextBox(txtTurboWeight),
                    Price = GetPositiveDoubleFromTextBox(txtTurboPrice)
                };

                newEngine.SetPressure(GetPositiveDoubleFromTextBox(txtTurboPressure));

                engines.Add(newEngine);
                DisplayEngines();
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка ввода: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void btnAddICE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IceEngine newEngine = new IceEngine
                {
                    Horsepower = GetPositiveDoubleFromTextBox(txtICEHorsepower),
                    Weight = GetPositiveDoubleFromTextBox(txtICEWeight),
                    Price = GetPositiveDoubleFromTextBox(txtICEPrice)
                };

                newEngine.SetVolume(GetPositiveDoubleFromTextBox(txtICEVolume));

                engines.Add(newEngine);
                DisplayEngines();
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка ввода: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void btnAddJetEngine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JetEngine newEngine = new JetEngine
                {
                    Horsepower = GetPositiveDoubleFromTextBox(txtJetHorsepower),
                    Weight = GetPositiveDoubleFromTextBox(txtJetWeight),
                    Price = GetPositiveDoubleFromTextBox(txtJetPrice)
                };

                newEngine.SetPressure(GetPositiveDoubleFromTextBox(txtJetPressure));
                newEngine.SetVolume(GetPositiveDoubleFromTextBox(txtJetVolume));

                engines.Add(newEngine);

                DisplayEngines();
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка ввода: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void DisplayEngines()
        {
            lstEngines.Items.Clear();
            foreach (var engine in engines)
            {
                lstEngines.Items.Add(engine.ToString());
            }
        }

        private void btnApplyDiscount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!double.TryParse(txtDiscountPercentage.Text, out double discountPercentage) || discountPercentage < 0 || discountPercentage > 100)
                {
                    MessageBox.Show("Пожалуйста, введите корректный процент скидки между 0 и 100.");
                    return;
                }

                if (lstEngines.SelectedItem is null)
                {
                    MessageBox.Show("Пожалуйста, выберите двигатель из списка.");
                    return;
                }

                string selectedEngineStr = lstEngines.SelectedItem.ToString();
                IEngine selectedEngine = engines.FirstOrDefault(engine => engine.ToString() == selectedEngineStr);

                if (selectedEngine == null)
                {
                    MessageBox.Show("Выбранный двигатель не найден.");
                    return;
                }

                double originalPrice = selectedEngine.GetPrice();
                double discountAmount = (originalPrice * discountPercentage) / 100;
                double newPrice = originalPrice - discountAmount;

                selectedEngine.SetPrice(newPrice);
                DisplayEngines();
                MessageBox.Show("Скидка успешно применена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при применении скидки: {ex.Message}");
            }
        }

        private double GetPositiveDoubleFromTextBox(System.Windows.Controls.TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                throw new FormatException("Поле не может быть пустым.");
            }

            if (!double.TryParse(textBox.Text, out double result) || result < 0 || result >100000)
            {
                throw new FormatException("Ввод должен быть положительным целым числом (от 0 до 100000).");
            }
            return result;
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged_2(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void lstEngines_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }

    public abstract class Engine : IEngine
    {
        public double Horsepower { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }

        public double GetPrice() => Price;

        public void SetPrice(double price) => Price = price;

        public override string ToString()
        {
            return $"Мощность: {Horsepower}, Вес: {Weight}, Цена: {Price}";
        }
    }

    public class TurboEngine : Engine, ITurboEngine
    {
        private double pressure;

        public double GetPressure() => pressure;

        public void SetPressure(double value) => pressure = value;

        public override string ToString()
        {
            return $"Турбодвигатель - Мощность: {Horsepower}, Вес: {Weight}, Цена: {Price}, Давление: {pressure} (Турбированный ДВС)";
        }
    }

    public class IceEngine : Engine, IInternalCombustionEngine
    {
        private double volume;

        public double GetVolume() => volume;

        public void SetVolume(double value) => volume = value;

        public override string ToString()
        {

            return $"Двигатель внутреннего сгорания - Мощность: {Horsepower}, Вес: {Weight}, Цена: {Price}, Объём: {volume} (ДВС)";
        }
    }

    public class JetEngine : Engine, ITurboEngine, IInternalCombustionEngine
    {
        private double pressure;
        private double volume;

        public double GetPressure() => pressure;

        public void SetPressure(double value) => pressure = value;

        public double GetVolume() => volume;

        public void SetVolume(double value) => volume = value;

        public override string ToString()
        {
            return $"Реактивный двигатель - Мощность: {Horsepower}, Вес: {Weight}, Цена: {Price}, Объём: {GetVolume()}, Давление: {GetPressure()} (Реактивный двигатель)";
        }
    }
}
