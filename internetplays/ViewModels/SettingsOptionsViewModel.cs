using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace internetplays.ViewModels
{
    public class SettingsOptionsViewModel
    {
        public string SelectedGame { get; set; }
        public string SelectedService { get; set; }
        public string Username { get; set; }
        public string mGBAHttpPath { get; set; }
        public string mGBAPath { get; set; }
    }
}
