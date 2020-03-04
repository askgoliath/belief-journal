using System;
using System.Collections.Generic;
using System.Text;

namespace HeartBelief.Models
{
    public enum MenuItemType
    {
        About, 
        Journal
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
