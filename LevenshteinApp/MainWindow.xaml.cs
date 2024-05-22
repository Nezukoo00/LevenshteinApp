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

namespace LevenshteinApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void CalculateButton_Click (object sender, RoutedEventArgs e)
        {
            string word1 = Word1TextBox.Text;
            string word2 = Word2TextBox.Text;
            if (string.IsNullOrEmpty(word1) || string.IsNullOrEmpty(word2))
            {
                MessageBox.Show("Введите два слова!");
                return;
            }
            int distance = CalculateLevensteinDistance(word1,word2, out List<LevenshteinStep> steps);
            LevenshteinDataGrid.ItemsSource= steps;
           
        }
        private int CalculateLevensteinDistance (string word1,string word2, out List<LevenshteinStep> steps)
        {
            int[,] dp = new int[word1.Length + 1, word2.Length + 1];
            steps = new List<LevenshteinStep>();
            for (int i = 0; i< word1.Length; i++)
            {
                for (int j = 0; j < word2.Length; j++)
                {
                    if ( i == 0)
                    {
                        dp[i,j] = j;
                    }
                    else if ( j == 0)
                    {
                        dp[i,j] = i;
                    }
                    else
                    {
                        int cost = (word1[i - 1] == word2[j-1]) ? 0:1;
                        dp[i,j] = Math.Min(Math.Min(dp[i - 1, j] + 1, dp[i,j-1] +1), dp[i-1,j-1] + cost);
                    }
                     if (i>0 && j>0)
                    {
                        steps.Add(new LevenshteinStep
                        {
                            Index = $"{i},{j}",
                            Word1 = word1.Substring(0, i),
                            Word2 = word2.Substring(0, j),
                            Cost = dp[i,j]
                        });
                    }
                }
            }
            return dp[word1.Length,word2.Length];
        }
    }
    public class LevenshteinStep
    {
        public string Index { get; set; }
        public string Word1 { get; set; }
        public string Word2 { get; set; }
        public int Cost { get; set; }
    }
}
