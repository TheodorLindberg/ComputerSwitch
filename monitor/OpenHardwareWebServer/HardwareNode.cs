/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2020 Michael Möller <mmoeller@openhardwaremonitor.org>
	
*/

using System;
using System.Collections.Generic;
using OpenHardwareMonitor.Hardware;

namespace OpenHardwareServer
{
    public class HardwareNode : Node
    {
        private readonly IHardware hardware;
        private readonly Identifier expandedIdentifier;

        private List<TypeNode> typeNodes = new List<TypeNode>();
       
        public HardwareNode(IHardware hardware) : base()
        {
            this.hardware = hardware;
            this.Image = GetHardwareImageName(hardware.HardwareType);

            foreach (SensorType sensorType in Enum.GetValues(typeof(SensorType)))
                typeNodes.Add(new TypeNode(sensorType, hardware));

            foreach (ISensor sensor in hardware.Sensors)
                SensorAdded(sensor);

            hardware.SensorAdded += new SensorEventHandler(SensorAdded);
            hardware.SensorRemoved += new SensorEventHandler(SensorRemoved);

            this.expandedIdentifier = new Identifier(hardware.Identifier, "expanded");
        }

        public override string Text
        {
            get { return hardware.Name; }
            set { hardware.Name = value; }
        }

        public IHardware Hardware
        {
            get { return hardware; }
        }

        public override bool IsExpanded
        {
            get
            {
                return base.IsExpanded;
            }
            set
            {
                if (base.IsExpanded != value)
                {
                    base.IsExpanded = value;
                }
            }
        }

        private void UpdateNode(TypeNode node)
        {
            if (node.Nodes.Count > 0)
            {
                if (!Nodes.Contains(node))
                {
                    int i = 0;
                    while (i < Nodes.Count &&
                      ((TypeNode)Nodes[i]).SensorType < node.SensorType)
                        i++;
                    Nodes.Insert(i, node);
                }
            }
            else
            {
                if (Nodes.Contains(node))
                    Nodes.Remove(node);
            }
        }

        private void SensorRemoved(ISensor sensor)
        {
            foreach (TypeNode typeNode in typeNodes)
                if (typeNode.SensorType == sensor.SensorType)
                {
                    SensorNode sensorNode = null;
                    foreach (Node node in typeNode.Nodes)
                    {
                        SensorNode n = node as SensorNode;
                        if (n != null && n.Sensor == sensor)
                            sensorNode = n;
                    }
                    if (sensorNode != null)
                    {
                        sensorNode.PlotSelectionChanged -= SensorPlotSelectionChanged;
                        typeNode.Nodes.Remove(sensorNode);
                        UpdateNode(typeNode);
                    }
                }
            if (PlotSelectionChanged != null)
                PlotSelectionChanged(this, null);
        }

        private void InsertSorted(Node node, ISensor sensor)
        {
            int i = 0;
            while (i < node.Nodes.Count &&
              ((SensorNode)node.Nodes[i]).Sensor.Index < sensor.Index)
                i++;
            SensorNode sensorNode = new SensorNode(sensor);
            sensorNode.PlotSelectionChanged += SensorPlotSelectionChanged;
            node.Nodes.Insert(i, sensorNode);
        }

        private void SensorPlotSelectionChanged(object sender, EventArgs e)
        {
            if (PlotSelectionChanged != null)
                PlotSelectionChanged(this, null);
        }

        private void SensorAdded(ISensor sensor)
        {
            foreach (TypeNode typeNode in typeNodes)
                if (typeNode.SensorType == sensor.SensorType)
                {
                    InsertSorted(typeNode, sensor);
                    UpdateNode(typeNode);
                }
            if (PlotSelectionChanged != null)
                PlotSelectionChanged(this, null);
        }

        private static string GetHardwareImageName(HardwareType type)
        {

            switch (type)
            {
                case HardwareType.CPU:
                    return "cpu.png";
                case HardwareType.GpuNvidia:
                    return "nvidia.png";
                case HardwareType.GpuAti:
                    return "ati.png";
                case HardwareType.HDD:
                    return "hdd.png";
                case HardwareType.Heatmaster:
                    return "bigng.png";
                case HardwareType.Mainboard:
                    return "mainboard.png";
                case HardwareType.SuperIO:
                    return "chip.png";
                case HardwareType.TBalancer:
                    return "bigng.png";
                case HardwareType.RAM:
                    return "ram.png";
                default:
                    return "cpu.png";
            }

        }

        public event EventHandler PlotSelectionChanged;
    }
}
