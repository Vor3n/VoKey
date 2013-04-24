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
using AssetServer.Entities;
using System.Collections.ObjectModel;

namespace AssetServerGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AssetServer.AssetServer assetServer;
        private Room SelectedRoom
        {
            get
            {
                return RoomsListBox.SelectedItem as Room;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            assetServer = new AssetServer.AssetServer();
            RoomsListBox.ItemsSource = assetServer.RoomList;
            RoomsListBox.DisplayMemberPath = "Name";
            FindableObjectsListBox.DisplayMemberPath = "color";
        }

        private void addRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Room newObject = new Room(roomNameTextBox.Text.ToString());
            assetServer.RoomList.Add(newObject);
            roomNameTextBox.Clear();
            roomNameTextBox.Focus();
        }

        private void RoomsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lb = sender as ListBox;
            var selectedRoom = lb.SelectedItem as Room;
            FindableObjectsListBox.ItemsSource = selectedRoom.objects;
        }

        private void addItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRoom != null)
            {
                FindableObject newObject = new FindableObject(findableObjectTextBox.Text.ToString());
                SelectedRoom.Add(newObject);
                findableObjectTextBox.Clear();
                findableObjectTextBox.Focus();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Room r in assetServer.RoomList)
            {
                Console.Write(r.Serialize(r).InnerXml.ToString());
            }
        }

        private void startWebserverButton_Click(object sender, RoutedEventArgs e)
        {
            assetServer.Start();
        }
    }
}
