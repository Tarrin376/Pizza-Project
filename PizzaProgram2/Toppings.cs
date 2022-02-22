using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PizzaProgram2
{
    public class Toppings : Pizza
    {
        public int numPizzas; // The number of pizzas that the user has ordered.
        private readonly MainWindow page; // The current instance (the current page so elements can be modified).
        public Dictionary<string, int> pizzaNumbers; // Stores the associated "pizza number" of each pizza.
        public Dictionary<string, ListBox> toppingLists;
        public static readonly Dictionary<string, decimal> toppings = new Dictionary<string, decimal>() // Toppings.
        {
            {"Pepperoni",  1.20m }, {"Mushrooms", 1.00m }, {"Bacon Rashers", 1.56m },
            {"Ham", 1.25m }, { "Sausage", 1.30m }, {"Onions", 1.00m }
        };

        /// <summary>
        /// Constructor that inherits from 'Pizza' class as it needs access to the 
        /// page's elements.
        /// </summary>
        /// <param name="page">Page instance.</param>
        public Toppings(MainWindow page) : base(page)
        {
            numPizzas = 0;
            this.page = page;
            pizzaNumbers = new Dictionary<string, int>();
            toppingLists = new Dictionary<string, ListBox>();
        }

        /// <summary>
        /// Method that adds the topping to the requested pizza instead the 'toppings'
        /// listbox. It will add the topping to the correct topping list box and update
        /// the total cost of the order. Error checking is done to ensure that the same topping
        /// is not added to a specific pizza more than once. 
        /// </summary>
        /// <param name="comboBox">The combo box that raised the event.</param>
        public void AddItem(ComboBox? comboBox, Button button)
        {
            // The pizza number to recieve extra toppings.
            int selectedPizza = Convert.ToInt32(comboBox.SelectedItem);
            if (selectedPizza > 0)
            {
                string topping = toppings.Keys.ToList()[page.buttons.ToList().IndexOf(button)]; // Topping to be added.
                string pizza = $"Pizza number - {selectedPizza}:"; // Content to be displayed.
                if (pizzaNumbers.ContainsKey(pizza) == false)
                {
                    pizzaNumbers[pizza] = selectedPizza;
                    page.toppingOrder.Items.Add(pizza);
                    CreateToppingsList(pizza);
                }

                // Checks if the topping has already been added to the pizza or not.
                if (!ContainsTopping(toppingLists[pizza], topping))
                {
                    Button newTopping = CreateButton(topping);
                    toppingLists[pizza].Items.Add(newTopping);
                    page.totalCost += toppings[topping];
                    page.UpdateTotal();
                }
                else
                {
                    // Error message to inform the user that the topping has already been added to that pizza.
                    MessageBox.Show($"The topping '{topping}' has already been added to this pizza.");
                }
            }

            comboBox.SelectedIndex = -1; // Resets the selected item in the combo box.
            comboBox.Text = "Choose pizza:"; // Resets the text in the combo box.
        }

        /// <summary>
        /// This anonymous method is responsible for returning a boolean value if the topping
        /// selected by the user has already been added to their chosen pizza. If it has been added,
        /// the method will return "true" and "false" if it has not been added yet.
        /// </summary>
        private Func<ListBox, string, bool> ContainsTopping = (ListBox toppingList, string topping) =>
        {
            foreach (Button item in toppingList.Items) if (item.Content.ToString() == topping) return true;
            return false;
        };

        /// <summary>
        /// Creates the new topping list box so the user can view the toppings
        /// and delete them. (only runs when the pizza chosen does not have any
        /// other extra toppings yet)
        /// </summary>
        /// <param name="pizza">Pizza to recieve extra toppings.</param>
        private void CreateToppingsList(string pizza)
        {
            ListBox newToppingsList = new ListBox();
            newToppingsList.Width = 240;
            newToppingsList.Height = 60;
            newToppingsList.Background = Brushes.Transparent;
            newToppingsList.BorderBrush = Brushes.Transparent;
            page.toppingOrder.Items.Add(newToppingsList);
            toppingLists[pizza] = newToppingsList;
        }

        /// <summary>
        /// Responsible for creating a new button that has the item's 
        /// name as its content. This button will be added to the user's 
        /// order so he/she can view or delete it later on.
        /// </summary>
        /// <param name="content">The added item's name.</param>
        /// <returns>A new button that contains the item name.</returns>
        public override Button CreateButton(string content)
        {
            Button newTopping = new Button();
            newTopping.Width = 200;
            newTopping.Height = 25;
            newTopping.Background = (Brush)convert.ConvertFrom("#242424");
            newTopping.Foreground = Brushes.White;
            newTopping.BorderBrush = (Brush)convert.ConvertFrom("#22dd97");
            newTopping.Content = content;
            newTopping.Click += DeleteItem;
            return newTopping;
        }

        /// <summary>
        /// Changes the content on the page when the user clicks the 'toppings' page.
        /// Will change images, text and also hides the combo boxes as they are not
        /// needed on this specific page.
        /// </summary>
        public override void ChangeContent()
        {
            page.ChangeImage(page.image1, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Toppings\Pepperoni.jpg");
            page.title1.Content = $"Pepperoni - £{toppings["Pepperoni"]}";
            page.ChangeImage(page.image2, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Toppings\Mushroom.jfif");
            page.title2.Content = $"Mushrooms - £{toppings["Mushrooms"]}";
            page.ChangeImage(page.image3, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Toppings\Bacon.jpg");
            page.title3.Content = $"Bacon Rashers - £{toppings["Bacon Rashers"]}";
            page.ChangeImage(page.image4, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Toppings\Ham.jfif");
            page.title4.Content = $"Ham - £{toppings["Ham"]}";
            page.ChangeImage(page.image5, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Toppings\Sausage.jpg");
            page.title5.Content = $"Sausage - £{toppings["Sausage"]}";
            page.ChangeImage(page.image6, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Toppings\Onion.jpg");
            page.title6.Content = $"Onions - £{toppings["Onions"]}";
            page.options.Content = "Extra Topping options:";
            ModifyComboBox(page.comboBox1);
            ModifyComboBox(page.comboBox2);
            ModifyComboBox(page.comboBox3);
            ModifyComboBox(page.comboBox4);
            ModifyComboBox(page.comboBox5);
            ModifyComboBox(page.comboBox6);
            ShowComboBoxes();
        }

        /// <summary>
        /// Gets the combo box that is linked with the button that
        /// called this method. This is so the combo box can be reset after
        /// the item is added to the user's order.
        /// </summary>
        /// <param name="name">Name of the button object.</param>
        /// <param name="button">The button object that raised the event.</param>
        public void GetComboBox(string name, Button button)
        {
            ComboBox? comboBox = null;
            if (name.Equals(Buttons.button1.ToString())) comboBox = page.comboBox1;
            if (name.Equals(Buttons.button2.ToString())) comboBox = page.comboBox2;
            if (name.Equals(Buttons.button3.ToString())) comboBox = page.comboBox3;
            if (name.Equals(Buttons.button4.ToString())) comboBox = page.comboBox4;
            if (name.Equals(Buttons.button5.ToString())) comboBox = page.comboBox5;
            if (name.Equals(Buttons.button6.ToString())) comboBox = page.comboBox6;
            if (comboBox != null) AddItem(comboBox, button);
        }

        /// <summary>
        /// This method is responsible for modifying the content
        /// of the combo box so it suites the current page that 
        /// the user is on.
        /// </summary>
        /// <param name="comboBox">Combo box to modify</param>
        public override void ModifyComboBox(ComboBox comboBox)
        {
            comboBox.Text = "Choose pizza:";
            comboBox.Items.Clear();
            for (int i = 1; i <= numPizzas; i++) comboBox.Items.Add(i);
            if (numPizzas == 0) comboBox.IsEnabled = false;
        }

        /// <summary>
        /// Will make the combo boxes reappear on the screen after 
        /// the user has visited the 'sides' page (the page where the combo boxes
        /// are hidden).
        /// </summary>
        public void ShowComboBoxes()
        {
            foreach (ComboBox comboBox in page.pizzaPage.pizzas.Keys)
            {
                comboBox.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Responsible for deleting a specific topping from the user's requested
        /// pizza. It will loop over each pizza that has extra toppings and check if the
        /// topping object that was clicked is in that pizza's topping list. If it is, the 
        /// topping will be removed and the total cost will be updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void DeleteItem(object sender, RoutedEventArgs? e = null)
        {
            foreach (KeyValuePair<string, ListBox> pair in toppingLists)
            {
                if (pair.Value.Items.Contains(sender as Button))
                {
                    pair.Value.Items.Remove(sender as Button);
                    if (pair.Value.Items.Count == 0)
                    {
                        page.toppingOrder.Items.Remove(pair.Value);
                        page.toppingOrder.Items.Remove(pair.Key);
                        pizzaNumbers.Remove(pair.Key); // Removes the pizza from the list if the pizza now has no extra toppings.
                        toppingLists.Remove(pair.Key); // Removes the pizza from the list if the pizza now has no extra toppings.
                    }

                    string? pizza = (sender as Button).Content.ToString();
                    page.totalCost -= toppings[pizza]; // Removes the cost of that topping from the total amount.
                    page.UpdateTotal(); // Updates the total amount.
                    break;
                }
            }
        }
    }
}
