using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Client.MVVM.Model;
using Newtonsoft.Json;

namespace Client.Core
{
    class Client
    {
        private int puerto;
        private string direccionIP;
        IPEndPoint endPoint;
        Socket clienteSocket;

        public Client()
        {
            this.puerto = 5050;
            this.direccionIP = "127.0.0.1";
            this.endPoint = new IPEndPoint(IPAddress.Parse(direccionIP), puerto);
            this.clienteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void connectServer() {

            try
            {
                clienteSocket.Connect(endPoint);
                ContactModel userData = new ContactModel() {
                    Id = 0,
                    Username = "Diego",
                    ImageSource = "https://beebom.com/wp-content/uploads/2022/10/Chainsaw-Man-Who-is-Pochita-All-You-Need-to-Know.jpg?w=730&h=487&crop=1&quality=75",
                    Messages = null,
                };
                string json = JsonConvert.SerializeObject(userData);
                byte[] data = Encoding.UTF8.GetBytes(json); // Convierte la cadena JSON en un arreglo de bytes
                clienteSocket.Send(data);

                Console.WriteLine("Conectado al servidor en {0}:{1}", this.direccionIP, this.puerto);

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
        }
    }
}
