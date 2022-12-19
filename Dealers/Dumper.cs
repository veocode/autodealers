using System.Xml;
using GTA.Math;
using VeoAutoMod.Dealers.DTO;


namespace VeoAutoMod.Dealers
{
    static class Dumper
    {
        private static XmlDocument xmlDoc;

        public static void DumpDealerToFile(Dealer dealer, string filePath)
        {
            xmlDoc = new XmlDocument();
            XmlNode dealerNode = xmlDoc.CreateElement("dealer");
            XmlNode slotsNode = xmlDoc.CreateElement("slots");
            XmlNode vehiclesNode = xmlDoc.CreateElement("vehicles");
            XmlNode entitiesNode = xmlDoc.CreateElement("entitites");

            XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(declaration);

            if (dealer.GetConfig().PlateText != DealerConfig.DefaultPlateText)
            {
                AddAttr(dealerNode, "license-plate", dealer.GetConfig().PlateText);
            }

            foreach (SlotDTO slot in dealer.GetConfig().Slots)
            {
                XmlNode slotNode = xmlDoc.CreateElement("slot");
                AddPositionAttrs(slotNode, slot.Position);
                AddFloatAttr(slotNode, "rotation", slot.Rotation);
                slotsNode.AppendChild(slotNode);
            }            
            
            foreach (VehicleDTO vehicle in dealer.GetConfig().Vehicles)
            {
                XmlNode vehicleNode = xmlDoc.CreateElement("vehicle");
                AddIntAttr(vehicleNode, "id", vehicle.Id);
                AddAttr(vehicleNode, "name", vehicle.Name);
                AddIntAttr(vehicleNode, "price", vehicle.Price);
                vehiclesNode.AppendChild(vehicleNode);
            }
                        
            foreach (EntityDTO entity in dealer.GetConfig().Entities)
            {
                XmlNode entityNode = xmlDoc.CreateElement("entity");
                AddIntAttr(entityNode, "id", entity.Id);
                AddPositionAttrs(entityNode, entity.Position);
                AddRotation3dAttrs(entityNode, entity.Rotation);
                AddBoolAttr(entityNode, "dynamic", entity.IsDynamic);
                entitiesNode.AppendChild(entityNode);
            }         
            
            foreach (RemovalDTO removal in dealer.GetConfig().Removals)
            {
                XmlNode removalNode = xmlDoc.CreateElement("entity-remove");
                AddPositionAttrs(removalNode, removal.Position);
                AddIntAttr(removalNode, "id", removal.EntityId);
                entitiesNode.AppendChild(removalNode);
            }

            dealerNode.AppendChild(slotsNode);
            dealerNode.AppendChild(vehiclesNode);
            dealerNode.AppendChild(entitiesNode);
            xmlDoc.AppendChild(dealerNode);
            xmlDoc.Save(filePath);
        }

        private static void AddAttr(XmlNode node, string attrName, string attrValue)
        {
            XmlAttribute attr = xmlDoc.CreateAttribute(attrName);
            attr.Value = attrValue;
            node.Attributes.Append(attr);
        }

        private static void AddBoolAttr(XmlNode node, string attrName, bool attrValue)
        {
            XmlAttribute attr = xmlDoc.CreateAttribute(attrName);
            attr.Value = attrValue.ToString().ToLower();
            node.Attributes.Append(attr);
        }

        private static void AddIntAttr(XmlNode node, string attrName, int attrValue)
        {
            XmlAttribute attr = xmlDoc.CreateAttribute(attrName);
            attr.Value = attrValue.ToString();
            node.Attributes.Append(attr);
        }

        private static void AddFloatAttr(XmlNode node, string attrName, float attrValue)
        {
            XmlAttribute attr = xmlDoc.CreateAttribute(attrName);
            attr.Value = attrValue.ToString().Replace(",", ".");
            node.Attributes.Append(attr);
        }

        private static void AddPositionAttrs(XmlNode node, Vector3 position)
        {
            AddFloatAttr(node, "x", position.X);
            AddFloatAttr(node, "y", position.Y);
            AddFloatAttr(node, "z", position.Z);
        }

        private static void AddRotation3dAttrs(XmlNode node, Vector3 rotation3d)
        {
            AddFloatAttr(node, "rx", rotation3d.X);
            AddFloatAttr(node, "ry", rotation3d.Y);
            AddFloatAttr(node, "rz", rotation3d.Z);
        }

    } 
}
