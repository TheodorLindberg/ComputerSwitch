using System;
using System.Collections.ObjectModel;
using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;
using System.Threading;



namespace OpenHardwareServer // Note: actual namespace depends on the project name.
{
    internal class Program
    {


        static void Main(string[] args)
        {

            Data data = new Data();
            data.Load();

            Console.WriteLine("Hello World!");
        }
    }
    class Data
    {
        private UpdateVisitor updateVisitor = new UpdateVisitor();
        public Node root;
        public TreeModel treeModel;
        public Computer computer;
        public void Load()
        {

            treeModel = new TreeModel();
            root = new Node(System.Environment.MachineName);
            root.Image = "computer.png";
            


            treeModel.Nodes.Add(root);

            computer = new Computer();

            computer.HardwareAdded += new HardwareEventHandler(HardwareAdded);
            computer.HardwareRemoved += new HardwareEventHandler(HardwareRemoved);
            computer.Open();

            computer.MainboardEnabled = true;
            computer.CPUEnabled = true;
            computer.RAMEnabled = true;
            computer.GPUEnabled = true;
            computer.FanControllerEnabled = true;
            computer.HDDEnabled = true;

            Console.WriteLine("Test");

            HttpServer server = new HttpServer(root, 3001);
      if (server.StartHTTPListener()) {
        Console.WriteLine("Starting server");
      }else {

        Console.WriteLine("Error starting server");
      }
            

            while(true)
            {

                computer.Accept(updateVisitor);
                Thread.Sleep(100);

            }




        }

        private void InsertSorted(Collection<Node> nodes, HardwareNode node)
        {
            int i = 0;
            while (i < nodes.Count && nodes[i] is HardwareNode &&
              ((HardwareNode)nodes[i]).Hardware.HardwareType <=
                node.Hardware.HardwareType)
                i++;
            nodes.Insert(i, node);
        }

        private void SubHardwareAdded(IHardware hardware, Node node)
        {
            HardwareNode hardwareNode =
              new HardwareNode(hardware);

            InsertSorted(node.Nodes, hardwareNode);

            foreach (IHardware subHardware in hardware.SubHardware)
                SubHardwareAdded(subHardware, hardwareNode);
        }

        private void HardwareAdded(IHardware hardware)
        {
            SubHardwareAdded(hardware, root);
        }

        private void HardwareRemoved(IHardware hardware)
        {
            List<HardwareNode> nodesToRemove = new List<HardwareNode>();
            foreach (Node node in root.Nodes)
            {
                HardwareNode hardwareNode = node as HardwareNode;
                if (hardwareNode != null && hardwareNode.Hardware == hardware)
                    nodesToRemove.Add(hardwareNode);
            }
            foreach (HardwareNode hardwareNode in nodesToRemove)
            {
                root.Nodes.Remove(hardwareNode);
            }
        }
    }
}
