﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;

namespace MauiSampleApp
{
    internal class ChartsPageViewModel : ViewModel
    {
        private IEnumerable<DataEntry> _data = new List<DataEntry>();

        public IEnumerable<DataEntry> Data
        {
            get { return _data; }
            set { _data = value; OnPropertyChanged(nameof(Data)); }
        }

        public ChartsPageViewModel()
        {
            Data = new List<DataEntry>()
                {
                    new DataEntry()
                    {
                        Percent = 91,
                        Label = "CO2",
                        //Color = Color.Red,
                    },
                    new DataEntry()
                    {
                        Percent = 29.5,
                        Label = "TVOC",
                    },
                    new DataEntry()
                    {
                        Percent = 85.2,
                        Label = "PM 2.5",
                    },
                    new DataEntry()
                    {
                        Percent = 45.6,
                        Label = "Nox",
                        //Color = Col
                    },
                    // new DataEntry()
                    //{
                    //    Percent = 12,
                    //    Label = "Nox",
                    //},
                    //  new DataEntry()
                    //{
                    //    Percent = 12,
                    //    Label = "Nox",
                    //},
                    //   new DataEntry()
                    //{
                    //    Percent = 12,
                    //    Label = "Nox",
                    //},
                    //new DataEntry()
                    //{
                    //    Percent = 12,
                    //    Label = "Nox",
                    //},
                    //  new DataEntry()
                    //{
                    //    Percent = 12,
                    //    Label = "Nox",
                    //},
                    //   new DataEntry()
                    //{
                    //    Percent = 12,
                    //    Label = "Nox",
                    //},

                };
        }
    }
}
