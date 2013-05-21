using System;
using System.Windows;
using System.Windows.Controls;

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
            FindableObjectsListBox.ItemsSource = selectedRoom.containedObjects;
        }

        private void addItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRoom != null)
            {
                FindableObject newObject = new FindableObject();
                //SelectedRoom.Add(newObject);
                findableObjectTextBox.Clear();
                findableObjectTextBox.Focus();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*foreach (Room r in assetServer.RoomList)
            {
                Console.Write(Room.Serialize(r));//.InnerXml.ToString());
            }*/
            string s = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n";
            //XmlDocument d = RoomList[0].Serialize(RoomList);

            foreach (Room r in assetServer.RoomList)
            {
                string temp = "KAK";//Room.Serialize(r);
                if (temp.Contains("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n"))
                    temp = temp.Substring(temp.IndexOf("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n") + "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n".Length);
                s += temp + "\r\n";
            }

            Console.WriteLine(s);
        }

        private void startWebserverButton_Click(object sender, RoutedEventArgs e)
        {
            assetServer.Start();
        }
    }
}
