using System;
using System.Xml.Serialization;

namespace abFinalExam
{
    [XmlInclude(typeof(Tire))]
    [XmlInclude(typeof(Wiper))]
    [XmlInclude(typeof(Battery))]
    public class Item
    {
        private String itemName;
        private int itemNumber;
        private int cost;
        private int weight;

        public Item() { }
        public String ItemName
        {
            get => itemName;
            set => itemName = value;
        }
        public int ItemNumber
        {
            get => itemNumber;
            set => itemNumber = value;
        }
        public int Cost
        {
            get => cost;
            set => cost = value;
        }
        public int Weight
        {
            get => weight;
            set => weight = value;
        }
    }
    interface IShipIt {
        int ShipItem();
        bool Ship
        {
            get;
            set;
        }
    }
    public class Tire : Item {

        private String tireModelName;
        private int wheelDiameter;
        public Tire() {
            this.Cost = 200;
            this.ItemNumber = 111;
            this.Weight = 30;
        }
        public Tire(String tireModelName, int wheelDiameter)
        {
            this.Cost = 200;
            this.ItemNumber = 111;
            this.Weight = 30;
            this.tireModelName = tireModelName;
            this.wheelDiameter = wheelDiameter;
        }
        public String TireModelName 
        {
            get => tireModelName;
            set => tireModelName = value;
        }
        public int WheelDiameter 
        { 
            get => wheelDiameter;
            set => wheelDiameter = value;
        }
    }
    public class Wiper : Item, IShipIt {
        private int wiperLength;
        private bool ship;
        public Wiper() {
            this.Cost = 15;
            this.ItemNumber = 222;
            this.Weight = 1;
        }
        public Wiper(int wiperLength)
        {
            this.Cost = 15;
            this.ItemNumber = 222;
            this.Weight = 1;
            this.wiperLength = wiperLength;
        }
        public int WiperLength 
        {
            get => wiperLength;
            set => wiperLength = value;
        }
        public bool Ship 
        {
            get => ship;
            set => ship = value;
        }
        public int ShipItem() {
            return 10;
        }
    }
    public class Battery : Item, IShipIt {
        private int voltage;
        private bool ship;
        public Battery() {
            this.Cost = 100;
            this.ItemNumber = 333;
            this.Weight = 10;
        }
        public Battery(int voltage)
        {
            this.Cost = 100;
            this.ItemNumber = 333;
            this.Weight = 10;
            this.voltage = voltage;
        }
        public int Voltage 
        {
            get => voltage;
            set => voltage = value;
        }
        public bool Ship 
        {
            get => ship;
            set => ship = value;
        }
        public int ShipItem() 
        {
            return 30;
        }
    }
}
