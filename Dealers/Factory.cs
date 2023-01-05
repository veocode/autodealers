using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using GTA.Math;
using GTA.UI;
using VeoAutoMod.Dealers.DTO;

namespace VeoAutoMod.Dealers
{
    class Factory
    {
        public static List<Dealer> CreateDealers(string dataFolderPath, string dealersXmlFileName)
        {
            List<Dealer> autoDealers = new List<Dealer>();

            XmlDocument xmlDealers = new XmlDocument();
            xmlDealers.Load(dataFolderPath + "\\" + dealersXmlFileName);

            try
            {

                XmlElement xmlRoot = xmlDealers.DocumentElement;
                if (xmlRoot != null)
                {
                    foreach (XmlElement xmlDealerNode in xmlRoot.GetElementsByTagName("dealer"))
                    {
                        string name = parseStringAttr(xmlDealerNode, "name");
                        Vector3 position = parsePositionAttrs(xmlDealerNode);
                        string fileName = parseStringAttr(xmlDealerNode, "file");
                        string filePath = dataFolderPath + "\\" + fileName;

                        DealerConfig config = LoadAutoDealerConfig(filePath);
                        Dealer dealer = new Dealer(name, position, config);

                        autoDealers.Add(dealer);
                    }
                }

            } catch (Exception e)
            {
                Notification.Show($"~r~Auto Dealer failed to load:\n~w~{e.Message}");
            }

            return autoDealers;
        }

        public static DealerConfig LoadAutoDealerConfig(string filePath)
        {
            string plateText = "";
            List<SlotDTO> slots = new List<SlotDTO>();
            List<VehicleDTO> vehicles = new List<VehicleDTO>();
            List<EntityDTO> entities = new List<EntityDTO>();
            List<RemovalDTO> removals = new List<RemovalDTO>();

            XmlDocument xmlDealer = new XmlDocument();
            xmlDealer.Load(filePath);

            XmlElement xmlRoot = xmlDealer.DocumentElement;
            if (xmlRoot != null)
            {
                plateText = xmlRoot.GetAttribute("license-plate");

                foreach (XmlElement xmlSlotNode in xmlRoot.GetElementsByTagName("slot"))
                {
                    Vector3 position = parsePositionAttrs(xmlSlotNode);
                    float rotation = parseFloatAttr(xmlSlotNode, "rotation");

                    SlotDTO slot = new SlotDTO(position, rotation);
                    slots.Add(slot);
                }

                foreach (XmlElement xmlVehicleNode in xmlRoot.GetElementsByTagName("vehicle"))
                {
                    int id = parseIntAttr(xmlVehicleNode, "id");
                    string name = parseStringAttr(xmlVehicleNode, "name");
                    int price = parseIntAttr(xmlVehicleNode, "price");

                    VehicleDTO vehicle = new VehicleDTO(id, name, price);
                    vehicles.Add(vehicle);
                }

                foreach (XmlElement xmlEntityNode in xmlRoot.GetElementsByTagName("entity"))
                {
                    int id = parseIntAttr(xmlEntityNode, "id");
                    Vector3 position = parsePositionAttrs(xmlEntityNode);
                    Vector3 rotation = parseRotationAttrs(xmlEntityNode);
                    bool isDynamic = parseBoolAttr(xmlEntityNode, "dynamic");

                    EntityDTO entity = new EntityDTO(id, position, rotation, isDynamic);
                    entities.Add(entity);
                }

                foreach (XmlElement xmlEntityNode in xmlRoot.GetElementsByTagName("entity-remove"))
                {
                    int id = parseIntAttr(xmlEntityNode, "id");
                    Vector3 position = parsePositionAttrs(xmlEntityNode);

                    RemovalDTO removal = new RemovalDTO(id, position);
                    removals.Add(removal);
                }
            }

            return new DealerConfig(filePath, slots, vehicles, entities, removals, plateText);
        }

        private static string parseStringAttr(XmlElement node, string attrName)
        {
            return node.Attributes.GetNamedItem(attrName).Value;
        }

        private static int parseIntAttr(XmlElement node, string attrName)
        {
            return int.Parse(node.GetAttribute(attrName));
        }

        private static float parseFloatAttr(XmlElement node, string attrName)
        {
            return float.Parse(node.Attributes.GetNamedItem(attrName).Value, CultureInfo.InvariantCulture.NumberFormat);
        }        
        
        private static bool parseBoolAttr(XmlElement node, string attrName)
        {
            return node.Attributes.GetNamedItem("dynamic").Value == "true";
        }

        private static Vector3 parsePositionAttrs(XmlElement node)
        {
            float x = parseFloatAttr(node, "x");
            float y = parseFloatAttr(node, "y");
            float z = parseFloatAttr(node, "z");
            return new Vector3(x, y, z);
        }

        private static Vector3 parseRotationAttrs(XmlElement node)
        {
            float rx = parseFloatAttr(node, "rx");
            float ry = parseFloatAttr(node, "ry");
            float rz = parseFloatAttr(node, "rz");
            return new Vector3(rx, ry, rz);
        }
    } 
}
