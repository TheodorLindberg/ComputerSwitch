/*
 
  This Source Code Form is subject to the terms of the Mozilla Public
  License, v. 2.0. If a copy of the MPL was not distributed with this
  file, You can obtain one at http://mozilla.org/MPL/2.0/.
 
  Copyright (C) 2009-2020 Michael Möller <mmoeller@openhardwaremonitor.org>
	
*/

using OpenHardwareMonitor.Hardware;

namespace OpenHardwareServer
{
    public class TypeNode : Node
    {

        private readonly SensorType sensorType;
        private readonly IHardware hardware;
        private readonly Identifier expandedIdentifier;

        public TypeNode(SensorType sensorType, IHardware hardware) : base()
        {
            this.sensorType = sensorType;
            this.hardware = hardware;

            switch (sensorType)
            {
                case SensorType.Voltage:
                    this.Image = "voltage.png";
                    this.Text = "Voltages";
                    break;
                case SensorType.Clock:
                    this.Image = "clock.png";
                    this.Text = "Clocks";
                    break;
                case SensorType.Load:
                    this.Image = "load.png";
                    this.Text = "Load";
                    break;
                case SensorType.Temperature:
                    this.Image = "temperature.png";
                    this.Text = "Temperatures";
                    break;
                case SensorType.Fan:
                    this.Image = "fan.png";
                    this.Text = "Fans";
                    break;
                case SensorType.Flow:
                    this.Image = "flow.png";
                    this.Text = "Flows";
                    break;
                case SensorType.Control:
                    this.Image = "control.png";
                    this.Text = "Controls";
                    break;
                case SensorType.Level:
                    this.Image = "level.png";
                    this.Text = "Levels";
                    break;
                case SensorType.Power:
                    this.Image = "power.png";
                    this.Text = "Powers";
                    break;
                case SensorType.Data:
                    this.Image = "data.png";
                    this.Text = "Data";
                    break;
                case SensorType.SmallData:
                    this.Image = "data.png";
                    this.Text = "Data";
                    break;
                case SensorType.Factor:
                    this.Image = "factor.png";
                    this.Text = "Factors";
                    break;
                case SensorType.Throughput:
                    this.Image = "throughput.png";
                    this.Text = "Throughput";
                    break;
            }

            NodeAdded += new NodeEventHandler(TypeNode_NodeAdded);
            NodeRemoved += new NodeEventHandler(TypeNode_NodeRemoved);

            this.expandedIdentifier = new Identifier(new Identifier(hardware.Identifier,
              sensorType.ToString().ToLowerInvariant()), "expanded");
        }

        private void TypeNode_NodeRemoved(Node node)
        {
            node.IsVisibleChanged -= new NodeEventHandler(node_IsVisibleChanged);
            node_IsVisibleChanged(null);
        }

        private void TypeNode_NodeAdded(Node node)
        {
            node.IsVisibleChanged += new NodeEventHandler(node_IsVisibleChanged);
            node_IsVisibleChanged(null);
        }

        private void node_IsVisibleChanged(Node node)
        {
            foreach (Node n in Nodes)
                if (n.IsVisible)
                {
                    this.IsVisible = true;
                    return;
                }
            this.IsVisible = false;
        }

        public SensorType SensorType
        {
            get { return sensorType; }
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
    }
}
