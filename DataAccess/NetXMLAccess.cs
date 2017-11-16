using BrainStructures;
using System;
//using System.Collections.Generic;
using DataStructures;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace DataAccess
{
    public static class NetXMLAccess
    { 
        class TempNetwork
        {
            public XmlNode NetworkXmlNode { get; set; }
            public NetStructure Network { get; set; }
        }

        //public const string SAVE_PATH = "C:\\Users\\DragonSheep\\Documents\\SavedNets\\";

        public static void SaveNet(String fileName, Collective toSave)
        {
            XmlWriter netWriter = null;
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Indent = true
                };
                netWriter = XmlWriter.Create(AppData.SavedNeuronsPath + fileName, settings);
                netWriter.WriteStartElement("collective");
                netWriter.WriteStartElement("sensory_map");
                for(int i = 0; i<toSave.SensoryMap.getSize(); i++)
                {
                    netWriter.WriteStartElement("sensory");
                    netWriter.WriteAttributeString("key", toSave.SensoryMap[i].ToString());
                    netWriter.WriteStartElement("synapseList");
                    for (int j = 0; j < toSave.SensoryMap.valueAt(i).SynapseList.Length; j++)
                    {
                        var synapse = toSave.SensoryMap.valueAt(i).SynapseList[j];
                        netWriter.WriteStartElement("synapse");
                        netWriter.WriteAttributeString("component_index", synapse.ComponentIndex.ToString());
                        var member = toSave.Components[(int)synapse.ComponentIndex];
                        int? target_index = member.SensoryList.IndexOf((Neuron)synapse.Target);
                        netWriter.WriteAttributeString("target_sensory_index", synapse.TargetSensoryIndex.ToString());
                        netWriter.WriteValue(synapse.Weight.ToString());
                        netWriter.WriteEndElement();
                    }
                    netWriter.WriteEndElement(); //End synapseList
                    netWriter.WriteEndElement(); //End sensory
                }
                netWriter.WriteEndElement();
                netWriter.WriteStartElement("intermediate_list");
                foreach (NetStructure member in toSave.Components)
                {
                    netWriter.WriteStartElement("network");
                    netWriter.WriteAttributeString("network_seq", member.NetworkSeq.ToString());
                    netWriter.WriteAttributeString("role_id", member.RoleID.ToString());
                    netWriter.WriteAttributeString("role_index", member.RoleIndex.ToString());
                    netWriter.WriteStartElement("intermediates");
                    foreach (INode component in member.Intermediates)
                    {
                        netWriter.WriteStartElement("node");
                        if (null != component.MadeFrom)
                        {
                            netWriter.WriteStartAttribute("madeFrom", component.MadeFrom.ToString());

                        }
                        netWriter.WriteStartElement("synapse_list");
                        foreach (Synapse synapse in component.SynapseList)
                        {
                            netWriter.WriteStartElement("synapse");
                            netWriter.WriteAttributeString("motor_index", synapse.MotorIndex.ToString());
                            int? target_index = Array.IndexOf(member.Intermediates,synapse.Target);
                            if(-1!=target_index)
                            {
                                netWriter.WriteAttributeString("target_in", "intermediates");
                                netWriter.WriteAttributeString("target_index", ((int)target_index).ToString());
                            }
                            else if (synapse.Target is Neuron)
                            {
                                target_index = Array.IndexOf(member.MotorList,(Neuron)synapse.Target);
                                netWriter.WriteAttributeString("target_in", "motor");
                                netWriter.WriteAttributeString("target_index", ((int)target_index).ToString());
                            }
                            else
                            {
                                throw new ArgumentException("Corrupted neural net.  Please don't create Skynet.");
                            }
                            netWriter.WriteAttributeString("target_sensory_index", synapse.TargetSensoryIndex.ToString());
                            netWriter.WriteValue(synapse.Weight.ToString());
                            netWriter.WriteEndElement(); //synapse
                        }
                        netWriter.WriteEndElement(); //synapse_list
                        netWriter.WriteEndElement(); //node
                    }
                    netWriter.WriteEndElement(); //intermediates
                    netWriter.WriteStartElement("sensory_list");
                    foreach (Neuron sensory in member.SensoryList)
                    {
                        netWriter.WriteStartElement("sensory");
                        netWriter.WriteStartElement("synapse_list");
                        foreach (Synapse synapse in sensory.SynapseList)
                        {
                            netWriter.WriteStartElement("synapse");
                            netWriter.WriteAttributeString("motor_index", synapse.MotorIndex.ToString());
                            int target_index = Array.IndexOf(member.Intermediates,synapse.Target);
                            if (-1 != target_index)
                            {
                                netWriter.WriteAttributeString("target_in", "intermediates");
                                netWriter.WriteAttributeString("target_index", ((int)target_index).ToString());
                            }
                            else
                            {
                                target_index = Array.IndexOf(member.MotorList, synapse.Target);
                                if (-1 != target_index)
                                {
                                    netWriter.WriteAttributeString("target_in", "motor");
                                    netWriter.WriteAttributeString("target_index", ((int)target_index).ToString());
                                }
                                else {
                                    throw new ArgumentException("Corrupted neural net.  Please don't create Skynet.");
                                }
                            }
                            netWriter.WriteAttributeString("target_sensory_index", synapse.TargetSensoryIndex.ToString());
                            netWriter.WriteValue(synapse.Weight.ToString());
                            netWriter.WriteEndElement(); //synapse
                        }
                        netWriter.WriteEndElement(); //synapse_list
                        netWriter.WriteEndElement(); //sensory
                    }
                    netWriter.WriteEndElement();//sensory_list

                    netWriter.WriteStartElement("motor_count");
                    netWriter.WriteValue(member.MotorList.Length);
                    netWriter.WriteEndElement();//motor_count
                    netWriter.WriteEndElement();//network

                }
                netWriter.WriteEndElement(); //intermediate_list
                netWriter.WriteEndElement(); //collective
                netWriter.Flush();
            }
            catch (DirectoryNotFoundException ex)
            {
            }
            catch
            {
                throw;
            }
            finally
            {
                if(null!=netWriter)
                {
                    netWriter.Close();
                }
            }
        }



        public static Collective LoadNet(String netName)
        {
            var collective = new Collective()
            {
                SensoryMap = new BinarySearchTree<char, Neuron>((chara, charb)=>chara-charb)
            };
            XmlReader netReader = null;
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings()
                {
                    DtdProcessing = DtdProcessing.Parse
                };
                netReader = XmlReader.Create(AppData.SavedNeuronsPath + netName + ".xml", settings);
                var netDocument = new XmlDocument();
                netDocument.Load(netReader);
                var root = netDocument.LastChild;
                var tempNetworkElementTree = new BinarySearchTree<string, TempNetwork>((i, j)=> i.CompareTo(j));
                foreach(XmlNode networkElement in root.SelectNodes("intermediate_list")[0].ChildNodes)
                {
                    tempNetworkElementTree.add(
                        networkElement.Attributes["network_seq"].Value,
                        new TempNetwork()
                        {
                            NetworkXmlNode = networkElement
                        }
                    );
                }
                tempNetworkElementTree = new BinarySearchTree<string, TempNetwork>(tempNetworkElementTree);
                collective.Components = new NetStructure[root.SelectNodes("intermediate_list")[0].ChildNodes.Count];
                for(int i = 0; i < tempNetworkElementTree.getSize();i++)
                {
                    collective.Components[i]=BuildNetwork(tempNetworkElementTree.valueAt(i),tempNetworkElementTree);
                    var currentNet = collective.Components[i];
                    int sensoryIndex = 0;
                    foreach (XmlNode sensoryXmlNode in tempNetworkElementTree.valueAt(i).NetworkXmlNode.SelectNodes("sensory_list")[0].ChildNodes)
                    {
                        foreach (XmlNode synapseXmlNode in sensoryXmlNode.SelectNodes("synapse_list")[0].ChildNodes)
                        {
                            INode target;
                            var target_index = 0;
                            if("intermediates".Equals(synapseXmlNode.Attributes["target_in"].Value))
                            {
                                target_index = Int32.Parse(synapseXmlNode.Attributes["target_index"].Value);
                                target = currentNet.Intermediates[Int32.Parse(synapseXmlNode.Attributes["target_index"].Value)];
                                
                            } else if ("motor".Equals(synapseXmlNode.Attributes["target_in"].Value))
                            {
                                target_index = currentNet.Intermediates.Length + Int32.Parse(synapseXmlNode.Attributes["target_index"].Value);
                                target = currentNet.MotorList[Int32.Parse(synapseXmlNode.Attributes["target_index"].Value)];
                            } else
                            {
                                throw new ArgumentException("Invalid target_in attribute found.  Please do not create Skynet");
                            }
                            currentNet.SensoryList[sensoryIndex].NewSynapse(0, target, Int32.Parse(synapseXmlNode.Attributes["target_sensory_index"].Value),Int32.Parse(synapseXmlNode.InnerXml), target_index);
                        }
                        currentNet.SensoryList[sensoryIndex].HardenNeuron();
                        sensoryIndex++;
                    }

                    var intermediateIndex = 0;
                    foreach (XmlNode intermediateXmlNode in tempNetworkElementTree.valueAt(i).NetworkXmlNode.SelectNodes("intermediates")[0].ChildNodes)
                    {
                        foreach (XmlNode synapseXmlNode in intermediateXmlNode.SelectNodes("synapse_list")[0].ChildNodes)
                        {
                            INode target;
                            var target_index = 0;
                            if ("intermediates".Equals(synapseXmlNode.Attributes["target_in"].Value))
                            {
                                target_index = Int32.Parse(synapseXmlNode.Attributes["target_index"].Value);
                                target = currentNet.Intermediates[Int32.Parse(synapseXmlNode.Attributes["target_index"].Value)];
                            }
                            else if ("motor".Equals(synapseXmlNode.Attributes["target_in"].Value))
                            {
                                target_index = currentNet.Intermediates.Length + Int32.Parse(synapseXmlNode.Attributes["target_index"].Value);
                                target = currentNet.MotorList[Int32.Parse(synapseXmlNode.Attributes["target_index"].Value)];
                            }
                            else
                            {
                                throw new ArgumentException("Invalid target_in attribute found.  Please do not create Skynet");
                            }
                            currentNet.Intermediates[intermediateIndex].NewSynapse(0, target, Int32.Parse(synapseXmlNode.Attributes["target_sensory_index"].Value), Int32.Parse(synapseXmlNode.InnerText),target_index);
                        }
                        currentNet.Intermediates[intermediateIndex].HardenNeuron();
                        intermediateIndex++;
                    }

                }
                foreach (XmlNode sensoryXmlNode in root.SelectNodes("sensory_map")[0].ChildNodes)
                {
                    var newSensory = new Neuron(0);
                    collective.SensoryMap.add(sensoryXmlNode.Attributes["key"].Value[0], newSensory);
                    var synapse_list = sensoryXmlNode.SelectNodes("synapseList")[0].ChildNodes;
                    foreach (XmlNode synapseNode in synapse_list)
                    {
                        var component = collective.Components[Int32.Parse(synapseNode.Attributes["component_index"].Value)];
                        var componentSensory = component.SensoryList[Int32.Parse(synapseNode.Attributes["target_sensory_index"].Value)];
                        newSensory.NewSynapse(0, componentSensory, 0, Int32.Parse(synapseNode.InnerText), Int32.Parse(synapseNode.Attributes["component_index"].Value));
                    }
                    newSensory.HardenNeuron();
                }
            } catch(Exception ex)
            {
                throw ex;
            }
            return collective;
        }

        static NetStructure BuildNetwork(TempNetwork toBuild, BinarySearchTree<string, TempNetwork> collective)
        {
            var networkXMLNode = toBuild.NetworkXmlNode;
            if(null==toBuild.Network)
            {
                toBuild.Network = new NetStructure()
                {
                    NetworkSeq = Int32.Parse(networkXMLNode.Attributes["network_seq"].Value),
                    Intermediates = new INode[toBuild.NetworkXmlNode.SelectNodes("intermediates")[0].ChildNodes.Count],
                    MotorList = new Neuron[Int32.Parse(toBuild.NetworkXmlNode.SelectNodes("motor_count")[0].InnerText)],
                    SensoryList = new LinkedList<Neuron>(),
                    RoleID = Int32.Parse(networkXMLNode.Attributes["role_id"].InnerText),
                    RoleIndex = Int32.Parse(networkXMLNode.Attributes["role_index"].InnerText)
                };
                    for(int motorIndex = 0; motorIndex < toBuild.Network.MotorList.Length; motorIndex++)
                {
                    toBuild.Network.MotorList[motorIndex] = new Neuron(20);
                }
                int intermediateIndex = 0;
                foreach(XmlNode componentXmlNode in networkXMLNode.SelectNodes("intermediates")[0].ChildNodes)
                {
                    if(null==componentXmlNode.Attributes["madeFrom"])
                    {
                        toBuild.Network.Intermediates[intermediateIndex] = new Neuron();
                    }else
                    {
                        toBuild.Network.Intermediates[intermediateIndex] = 
                            BuildNetwork(collective.search(componentXmlNode.Attributes["madeFrom"].InnerText),collective).CreateCompoundNode();
                    }
                    intermediateIndex++;
                }
                int sensoryIndex = 0;
                var toBuildSensory = toBuild.Network.SensoryList;
                foreach(XmlNode sensoryNode in networkXMLNode.SelectNodes("sensory_list")[0].ChildNodes)
                {
                    toBuildSensory[sensoryIndex] = new Neuron(1);
                    sensoryIndex++;
                }
            }
            return toBuild.Network;
        }
    }
}
