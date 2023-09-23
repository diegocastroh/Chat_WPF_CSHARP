using Client.Core;
using Client.MVVM.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Net.Sockets;

namespace Client.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }

        /* Commands */
        public RelayCommand SendCommand { get; set; }
        private ContactModel _selectedContact;

        public ContactModel SelectedContact
        {
            get { return _selectedContact; }
            set { _selectedContact = value;
                OnPropertyChanged();
            }
        }


        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; 
                OnPropertyChanged();
            }
        }




        public MainViewModel() { 
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();


            int puerto = 5050;
            string direccionIP = "127.0.0.1";
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(direccionIP), puerto);
            Socket clienteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SendCommand = new RelayCommand(o =>
            {
                Messages.Add(new MessageModel
                {
                    Message = Message,
                    FirstMessage = false
                });

                Message = "";
            });

            Messages.Add(new MessageModel
            {
                Username = "Allison",
                UsernameColor="#409aff",
                ImageSource = "https://i.imgur.com/vMWvLXd.png",
                Message = "test",
                Time = DateTime.Now,
                IsNativeOrigin = false,
                FirstMessage = true,
            });


            try
            {
                clienteSocket.Connect(endPoint);
                ContactModel userData = new ContactModel()
                {
                    Id = 0,
                    Username = "Diego",
                    ImageSource = "https://beebom.com/wp-content/uploads/2022/10/Chainsaw-Man-Who-is-Pochita-All-You-Need-to-Know.jpg?w=730&h=487&crop=1&quality=75",
                    Messages = null,
                };
                string json = JsonConvert.SerializeObject(userData);
                byte[] data = Encoding.UTF8.GetBytes(json); // Convierte la cadena JSON en un arreglo de bytes
                clienteSocket.Send(data);

                Console.WriteLine("Conectado al servidor en {0}:{1}", direccionIP, puerto);

                // Iniciar la comunicación desde el cliente
                System.Threading.Thread servidorThread = new System.Threading.Thread(() =>
                {
                    while (true)
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRecibidos = clienteSocket.Receive(buffer);
                        string respuesta = Encoding.ASCII.GetString(buffer, 0, bytesRecibidos);
                        Console.WriteLine("Servidor dice: " + respuesta);
                    }
                });

                servidorThread.Start();

                while (true)
                {
                    Console.Write("Cliente: ");
                    string mensajeCliente = Console.ReadLine();
                    byte[] datosEnviar = Encoding.ASCII.GetBytes(mensajeCliente);
                    clienteSocket.Send(datosEnviar);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                clienteSocket.Close();
            }


            for (int i = 0; i < 3; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "Allison",
                    UsernameColor="#409aff",
                    ImageSource = "https://i.imgur.com/vMWvLXd.png",
                    Message = "test",
                    Time = DateTime.Now,
                    IsNativeOrigin = false,
                    FirstMessage = true,
                });
            }

            for (int i = 0; i < 4; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "Bunny",
                    UsernameColor="#409aff",
                    ImageSource = "https://i.imgur.com/vMWvLXd.png",
                    Message = "Test",
                    Time = DateTime.Now,
                    IsNativeOrigin = true,
                });
            }
            Messages.Add(new MessageModel
            {
                Username = "Bunny",
                UsernameColor="#409aff",
                ImageSource = "https://i.imgur.com/vMWvLXd.png",
                Message = "Last",
                Time = DateTime.Now,
                IsNativeOrigin = true,
            });

            Contacts.Add(new ContactModel
            {
                Username = $"Allison",
                ImageSource = "https://i.imgur.com/vMWvLXd.png",
                Messages = Messages,
            });

        }
    }
}
