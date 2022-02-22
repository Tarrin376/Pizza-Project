using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PizzaProgram2
{
    public class Pizza : IPage
    {
        public static decimal[] pizzaCost = new decimal[] { 10.45m, 14.60m, 19.80m }; // Cost of small, medium and large pizzas.
        public static List<string> pizzaSizes = new List<string>() { "Small", "Medium", "Large" }; // The pizza sizes that can be chosen.
        public Dictionary<ComboBox, string> pizzas; // The pizzas that each combo box object is linked to.
        public BrushConverter convert = new BrushConverter(); // Object to convert strings into hexidecimal values.
        private readonly MainWindow page; // Current page instance.

        /// <summary>
        /// Pizza constructor.
        /// </summary>
        /// <param name="page">Current page instance (so elements on the page can be modified)</param>
        public Pizza(MainWindow page)
        {
            this.page = page;
            pizzas = new Dictionary<ComboBox, string>()
            {
                {page.comboBox1, "Pepperoni" }, {page.comboBox2, "Margherita"}, {page.comboBox3, "Chicken BBQ"},
                {page.comboBox4, "Cheeseburger" }, {page.comboBox5, "Meat feast" }, {page.comboBox6, "Vegetable"}
            };
        }

        /// <summary>
        /// Changes the content on the page when the user clicks the 'pizza' page.
        /// Will change images, text and also hides the combo boxes as they are not
        /// needed on this specific page.
        /// </summary>
        public virtual void ChangeContent()
        {
            page.options.Content = "Pizza options:";
            page.ChangeImage(page.image1, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Pizzas\Pepperoni.jfif");
            page.title1.Content = "Pepperoni Pizza";
            page.comboBox1.Visibility = Visibility.Visible;

            page.ChangeImage(page.image2, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Pizzas\Margherita.webp");
            page.title2.Content = "Margherita Pizza";
            page.comboBox2.Visibility = Visibility.Visible;

            page.ChangeImage(page.image3, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Pizzas\Chicken.jpg");
            page.title3.Content = "Chicken BBQ Pizza";
            page.comboBox3.Visibility = Visibility.Visible;

            page.ChangeImage(page.image4, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Pizzas\Burger.jpg");
            page.title4.Content = "Cheeseburger Pizza";
            page.comboBox4.Visibility = Visibility.Visible;

            page.ChangeImage(page.image5, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Pizzas\Meat.jpg");
            page.title5.Content = "Meat feast Pizza";
            page.comboBox5.Visibility = Visibility.Visible;

            page.ChangeImage(page.image6, @"C:\Users\Tarrin\source\repos\PizzaProgram2\PizzaProgram2\Images\Pizzas\Vegetable.jpg");
            page.title6.Content = "Vegetable Pizza";
            page.comboBox6.Visibility = Visibility.Visible;

            ModifyComboBox(page.comboBox1);
            ModifyComboBox(page.comboBox2);
            ModifyComboBox(page.comboBox3);
            ModifyComboBox(page.comboBox4);
            ModifyComboBox(page.comboBox5);
            ModifyComboBox(page.comboBox6);
        }

        /// <summary>
        /// Responsible for retrieving the combo box that is linked to the button
        /// that raised the event. Will then add the item using the combo box in the
        /// 'AddItem' method.
        /// </summary>
        /// <param name="name">The name of the button that raised the event.</param>
        public void GetComboBox(string name)
        {
            ComboBox? comboBox = null;
            if (name.Equals(Buttons.button1.ToString())) comboBox = page.comboBox1;
            if (name.Equals(Buttons.button2.ToString())) comboBox = page.comboBox2;
            if (name.Equals(Buttons.button3.ToString())) comboBox = page.comboBox3;
            if (name.Equals(Buttons.button4.ToString())) comboBox = page.comboBox4;
            if (name.Equals(Buttons.button5.ToString())) comboBox = page.comboBox5;
            if (name.Equals(Buttons.button6.ToString())) comboBox = page.comboBox6;
            if (comboBox != null) AddItem(comboBox);
        }

        /// <summary>
        /// Will modify the items in the combo box with the different sizes (small, med, large) with
        /// their associated prices to each. Will run when the pizza page is opened.
        /// </summary>
        /// <param name="comboBox">The combo box to modify.</param>
        public virtual void ModifyComboBox(ComboBox comboBox)
        {
            comboBox.Text = "Choose size:";
            comboBox.Items.Clear();

            for (int i = 0; i < pizzaSizes.Count; i++)
            {
                comboBox.Items.Add($"{pizzaSizes[i]} - £{pizzaCost[i]}");
            }

            comboBox.IsEnabled = true;
        }

        /// <summary>
        /// Responsible for adding a new pizza to the 'pizza' listbox. Will create a new button
        /// and assign a 'DeleteItem' event to it so the item can be removed when clicked on.
        /// The total amount is also updated to keep track of the user's current spending. 
        /// </summary>
        /// <param name="comboBox">The combo box linked with the button that raised the event.</param>
        public virtual void AddItem(ComboBox? comboBox)
        {
            string size = (comboBox.SelectedIndex >= 0) ? pizzaSizes[comboBox.SelectedIndex] : string.Empty;
            decimal cost = Math.Round((size != string.Empty) ? pizzaCost[comboBox.SelectedIndex] : 0, 2);
            page.totalCost += cost;
            page.UpdateTotal();

            if (size != string.Empty)
            {
                Button newBtn = CreateButton($"{page.toppingPage.numPizzas + 1}: {pizzas[comboBox]} ({size}) - £{cost}");
                page.pizzaOrder.Items.Add(newBtn);
                page.toppingPage.numPizzas++;
            }

            comboBox.SelectedIndex = -1; // Set the selected item in the combo box to nothing.
            comboBox.Text = "Choose size:"; // Reset the text in the combo box.
            comboBox.IsEditable = true;
        }

        /// <summary>
        /// Responsible for creating a new button that has the item's 
        /// name as its content. This button will be added to the user's 
        /// order so he/she can view or delete it later on.
        /// </summary>
        /// <param name="content">The added item's name.</param>
        /// <returns>A new button that contains the item name.</returns>
        public virtual Button CreateButton(string content)
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

        /// <summary>
        /// Responsible for deleting an item from the order list
        /// that the user wants to remove from their order. Will remove
        /// the button object from the list box collection.
        /// </summary>
        /// <param name="sender">Object that raises this event.</param>
        /// <param name="e">Event arguments.</param>
        public virtual void DeleteItem(object sender, RoutedEventArgs? e = null)
        {
            StringBuilder size = new StringBuilder();
            Button? button = sender as Button;
            string name = (string)button.Content;
            bool bracketFound = false;

            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == ')') break;
                if (bracketFound == true) size.Append(name[i]);
                if (name[i] == '(') bracketFound = true;
            }

            name = size.ToString();
            page.totalCost -= pizzaCost[pizzaSizes.IndexOf(name)];
            page.UpdateTotal();
            page.pizzaOrder.Items.Remove(sender as Button);
            SortItems();
            page.toppingPage.numPizzas--;

            if (page.pageNode.curPage.ellipse.Name.Equals(Progress.toppingProcess.ToString()))
            {
                page.toppingPage.ChangeContent();
            }
        }

        /// <summary>
        /// Runs when an item is deleted from the pizza listbox. Sorts the number
        /// of each element to improve ease of use and allow the user to add toppings
        /// to their pizza of choice.
        /// </summary>
        private void SortItems()
        {
            for (int i = 0; i < page.toppingPage.numPizzas - 1; i++)
            {
                int index = 0;
                string? order = ((Button)page.pizzaOrder.Items[i]).Content.ToString();
                while (index < order.Length && int.TryParse(order[index].ToString(), out _))
                {
                    index++;
                }

                string newContent = $"{i + 1}{order[index..]}";
                (page.pizzaOrder.Items[i] as Button).Content = newContent;
            }
        }
    }
}
