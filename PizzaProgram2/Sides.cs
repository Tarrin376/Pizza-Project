using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PizzaProgram2
{
    public class Sides : IPage
    {
        private readonly MainWindow page; // Page instance.
        private readonly string[] sides = new string[] { "Wedges", "Garlic Pizza", "Cheese sticks", "Onion rings", "French fries", "Salad" }; // Side options.
        private readonly decimal[] costs = new decimal[] { 2.30m, 3.00m, 3.20m, 2.78m, 1.78m, 1.34m }; // The costs of each side (links between the 'sides' array).
        public BrushConverter convert = new BrushConverter(); // Object to convert strings into hexidecimal values.

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page">Page instance.</param>
        public Sides(MainWindow page) => this.page = page;

        /// <summary>
        /// This method is responsible for adding the item to the 'sides' listbox 
        /// on the GUI. It also updates the total cost of the order and assigns
        /// a 'DeleteItem' event to each item added to the 'sides' listbox so they
        /// can be deleted when requested.
        /// </summary>
        /// <param name="button">The button that raised the event.</param>
        public void AddItem(Button button)
        {
            int index = page.buttons.ToList().IndexOf(button);
            page.totalCost += costs[index];
            page.UpdateTotal();

            Button newBtn = CreateButton($"{sides[index]} - £{costs[index]}");
            page.sideOrder.Items.Add(newBtn);
        }

        /// <summary>
        /// Changes the content on the page when the user clicks the 'sides' page.
        /// Will change images, text and also hides the combo boxes as they are not
        /// needed on this specific page.
        /// </summary>
        public void ChangeContent()
        {
            page.ChangeImage(page.image1, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Sides\Wedges.jfif");
            page.title1.Content = $"{sides[0]} - £{costs[0]}";
            page.ChangeImage(page.image2, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Sides\GarlicPizza.jpg");
            page.title2.Content = $"{sides[1]} - £{costs[1]}";
            page.ChangeImage(page.image3, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Sides\CheeseSticks.jpg");
            page.title3.Content = $"{sides[2]} - £{costs[2]}";
            page.ChangeImage(page.image4, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Sides\OnionRings.webp");
            page.title4.Content = $"{sides[3]} - £{costs[3]}";
            page.ChangeImage(page.image5, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Sides\Fries.jfif");
            page.title5.Content = $"{sides[4]} - £{costs[4]}";
            page.ChangeImage(page.image6, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Sides\Salad.jpg");
            page.title6.Content = $"{sides[5]} - £{costs[5]}";
            page.options.Content = "Choose your sides:";
            HideComboBoxes();
        }

        /// <summary>
        /// Method that hides each combo box on the page as the
        /// 'sides' page does not need these elements.
        /// </summary>
        private void HideComboBoxes()
        {
            foreach (ComboBox comboBox in page.pizzaPage.pizzas.Keys)
            {
                comboBox.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Method that is used to delete a specific element from the
        /// 'sides' listbox when the user clicks on it. It will update the
        /// total cost and will remove the item from the 'sides' listbox.
        /// </summary>
        /// <param name="sender">Object that raises this event.</param>
        /// <param name="e">Event arguments.</param>
        public void DeleteItem(object sender, RoutedEventArgs? e = null)
        {
            string? name = (sender as Button).Content.ToString();
            page.sideOrder.Items.Remove(sender as Button);

            int index = 0;
            while (index < name.Length && !int.TryParse(name[index].ToString(), out _))
            {
                index++;
            }

            // Deducts the cost of the deleted item from the total.
            page.totalCost -= decimal.Parse(name[index..]);
            page.UpdateTotal();
        }

        /// <summary>
        /// Responsible for creating a new button that has the item's 
        /// name as its content. This button will be added to the user's 
        /// order so he/she can view or delete it later on.
        /// </summary>
        /// <param name="content">The added item's name.</param>
        /// <returns>A new button that contains the item name.</returns>
        public Button CreateButton(string content)
        {
            Button newBtn = new Button();
            newBtn.Width = 240;
            newBtn.Height = 30;
            newBtn.FontSize = 15;
            newBtn.Background = (Brush)convert.ConvertFrom("#242424");
            newBtn.Foreground = Brushes.White;
            newBtn.BorderBrush = (Brush)convert.ConvertFrom("#22dd97");
            newBtn.Content = content;
            newBtn.Click += DeleteItem;
            return newBtn;
        }
    }
}
