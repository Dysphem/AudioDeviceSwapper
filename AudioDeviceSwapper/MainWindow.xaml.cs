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
using CoreAudioApi;
using System.Configuration;

namespace AudioDeviceSwapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string SpeakerFriendlyName = ConfigurationManager.AppSettings["SpeakersName"];
        public string HeadSetFriendlyName = ConfigurationManager.AppSettings["HeadSetName"];
        public MainWindow()
        {
            InitializeComponent();

        }


        //private void ListAudioDevices(object sender, RoutedEventArgs e)
        //{
        //    MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
        //    MMDeviceCollection devices = DevEnum.EnumerateAudioEndPoints(EDataFlow.eRender, EDeviceState.DEVICE_STATE_ACTIVE);
        //    var devicelist = new List<string>();
        //    for (int i = 0; i < devices.Count; i++)
        //    {
        //        devicelist.Add(devices[i].FriendlyName);
        //        //WriteObject(new AudioDevice(i, devices[i]));
        //    }
        //    OutputBox.Text = string.Join(Environment.NewLine, devicelist); 
        //}

        private void SetSpeakerAsDefault(object sender, RoutedEventArgs e)
        {
            SetDefaultDevice(SpeakerFriendlyName);
        }

        private void SetHeadSetAsDefault(object sender, RoutedEventArgs e)
        {
            SetDefaultDevice(HeadSetFriendlyName);
        }

        private void SetDefaultDevice(string name)
        {
            int index = 0;
            bool deviceFound = false;
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            MMDeviceCollection devices = DevEnum.EnumerateAudioEndPoints(EDataFlow.eRender, EDeviceState.DEVICE_STATE_ACTIVE);

            PolicyConfigClient client = new PolicyConfigClient();

            if (!string.IsNullOrEmpty(name))
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    if (string.Compare(devices[i].FriendlyName, name, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        index = i;
                        deviceFound = true;
                        break;
                    }
                }
            }

            //if (inputObject != null)
            //{
            //    for (int i = 0; i < devices.Count; i++)
            //    {
            //        if (devices[i].ID == inputObject.Device.ID)
            //        {
            //            index = i;
            //            break;
            //        }
            //    }
            //}

            if (deviceFound)
            {
                client.SetDefaultEndpoint(devices[index].ID, ERole.eCommunications);
                client.SetDefaultEndpoint(devices[index].ID, ERole.eMultimedia);
            }
            else
            {
                //Warn
            }
        }
    }
}
