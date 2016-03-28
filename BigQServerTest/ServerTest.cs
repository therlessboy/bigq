﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigQ;

namespace BigQServerTest
{
    class ServerTest
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(@" $$\       $$\                      ");
            Console.WriteLine(@" $$ |      \__|                     ");
            Console.WriteLine(@" $$$$$$$\  $$\  $$$$$$\   $$$$$$\   ");
            Console.WriteLine(@" $$  __$$\ $$ |$$  __$$\ $$  __$$\  ");
            Console.WriteLine(@" $$ |  $$ |$$ |$$ /  $$ |$$ /  $$ | ");
            Console.WriteLine(@" $$ |  $$ |$$ |$$ |  $$ |$$ |  $$ | ");
            Console.WriteLine(@" $$$$$$$  |$$ |\$$$$$$$ |\$$$$$$$ | ");
            Console.WriteLine(@" \_______/ \__| \____$$ | \____$$ | ");
            Console.WriteLine(@"               $$\   $$ |      $$ | ");
            Console.WriteLine(@"               \$$$$$$  |      $$ | ");
            Console.WriteLine(@"                \______/       \__| ");
            Console.ResetColor();

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("BigQ Server");
            Console.WriteLine("");

            //
            // initialize
            // to enable debug, set the second boolean parameter to true
            //
            BigQServer server = new BigQServer(null, 8000, false, false, true, true, true);

            // callbacks
            server.MessageReceived = MessageReceived;
            server.ServerStopped = ServerStopped;
            server.ClientConnected = ClientConnected;
            server.ClientLogin = ClientLogin;
            server.ClientDisconnected = ClientDisconnected;
            // server.LogMessage = LogMessage;

            bool RunForever = true;
            while (RunForever)
            {
                // Console.WriteLine("34567890123456789012345678901234567890123456789012345678901234567890123456789");
                Console.WriteLine("---");
                Console.WriteLine("Commands: q quit cls listchannels listchannelsubscribers listclients");
                Console.Write("Command: ");
                string cmd = Console.ReadLine();
                if (String.IsNullOrEmpty(cmd)) continue;

                string guid = "";
                
                switch (cmd.ToLower())
                {
                    case "q":
                    case "quit":
                        RunForever = false;
                        break;

                    case "c":
                    case "cls":
                        Console.Clear();
                        break;

                    case "listchannels":
                        List<BigQChannel> channels = server.ListChannels();
                        if (channels != null)
                        {
                            foreach (BigQChannel curr in channels)
                            {
                                if (curr.Private == 1)
                                {
                                    Console.WriteLine("  " + curr.Guid + ": " + curr.ChannelName + " (owner " + curr.OwnerGuid + ") [priv]");
                                }
                                else
                                {
                                    Console.WriteLine("  " + curr.Guid + ": " + curr.ChannelName + " (owner " + curr.OwnerGuid + ") [pub]");
                                }                                
                            }
                        }
                        else
                        {
                            Console.WriteLine("(null)");
                        }
                        break;

                    case "listchannelsubscribers":
                        Console.Write("Channel GUID: ");
                        guid = Console.ReadLine();
                        List<BigQClient> subscribers = server.ListChannelSubscribers(guid);
                        if (subscribers != null)
                        {
                            foreach (BigQClient curr in subscribers)
                            {
                                Console.WriteLine("  " + curr.SourceIp + ":" + curr.SourcePort + "  " + curr.Email + "  [" + curr.ClientGuid + "]");
                            }
                        }
                        else
                        {
                            Console.WriteLine("(null)");
                        }
                        break;

                    case "listclients":
                        List<BigQClient> clients = server.ListClients();
                        if (clients != null)
                        {
                            foreach (BigQClient curr in clients)
                            {
                                Console.Write("  " + curr.SourceIp + ":" + curr.SourcePort + " ");

                                if (String.IsNullOrEmpty(curr.ClientGuid)) Console.Write("[login pending] ");
                                else Console.Write(curr.Email + " [" + curr.ClientGuid + "] ");

                                if (curr.Client == null) Console.Write("[null TCP!] ");
                                Console.WriteLine("");
                            }
                        }
                        else
                        {
                            Console.WriteLine("(null)");
                        }
                        break;
                        
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }

        #region Delegates

        static bool MessageReceived(BigQMessage msg)
        {
            // Console.WriteLine(msg.ToString());
            return true;
        }

        static bool ServerStopped()
        {
            // restart
            Console.WriteLine("*** Server stopped, attempting to restart ***");
            BigQServer server = new BigQServer(null, 8000, false, false, true, true, true);
            return true;
        }

        static bool ClientConnected(BigQClient client)
        {
            // client disconnected
            Console.WriteLine("ClientConnected received notice of connect from " + client.IpPort());
            return true;
        }

        static bool ClientLogin(BigQClient client)
        {
            // client disconnected
            Console.WriteLine("ClientConnected received notice of connect of client GUID " + client.ClientGuid + " from " + client.IpPort());
            return true;
        }

        static bool ClientDisconnected(BigQClient client)
        {
            // client disconnected
            Console.WriteLine("ClientDisconnected received notice of disconnect of client GUID " + client.ClientGuid + " from " + client.IpPort());
            return true;
        }

        static bool LogMessage(string msg)
        {
            Console.WriteLine("BigQServer message: " + msg);
            return true;
        }

        #endregion
    }
}