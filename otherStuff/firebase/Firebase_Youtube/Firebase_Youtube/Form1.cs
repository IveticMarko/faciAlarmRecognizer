using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace Firebase_Youtube
{
    public partial class Form1 : Form
    {
        //IFirebaseConfig config = new FirebaseConfig
        //{
        //    AuthSecret = "x4rK6C9Azsx3IZbnCobWpooeyGxtKDJxMD9vqqDc",
        //    BasePath = "https://fir-2da0c.firebaseio.com/"
        //};

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "aGr320uEXJMvpV0efTCLFy9CEJ5plXcyoudwjPke",
            BasePath = "https://testnotifying-7f790.firebaseio.com/"
        };

        IFirebaseClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);

            if (client != null)
            {
                MessageBox.Show("Connection is established");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var data = new Data
            {
                Id = textBox1.Text,
                Name = textBox2.Text,
                Address = textBox3.Text,
                Age = textBox4.Text
            };

            SetResponse response = await client.SetTaskAsync("Information/"+textBox1.Text, data);
            Data result = response.ResultAs<Data>();

            MessageBox.Show("Data inserted " + result.Id);

        }

        //private async void button2_Click(object sender, EventArgs e)
        //{
        //    FirebaseResponse response = await client.GetTaskAsync("Information/" + textBox1.Text);
        //    Data obj = response.ResultAs<Data>();

        //    textBox1.Text = obj.Id;
        //    textBox2.Text = obj.Name;
        //    textBox3.Text = obj.Address;
        //    textBox4.Text = obj.Age;

        //    MessageBox.Show("Data retrieved successfully");
        //}

        private async void button2_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.GetTaskAsync("tokens/-LaX5XDEmA321T6H0GuU");
            DataNotifications obj = response.ResultAs<DataNotifications>();

            textBox5.Text = obj.deviceName;
            textBox6.Text = obj.tokenID;

            MessageBox.Show("Data retrieved successfully");
        }
    }
}
