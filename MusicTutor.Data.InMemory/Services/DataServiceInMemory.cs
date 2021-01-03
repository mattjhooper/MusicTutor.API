using System;
using System.Collections.Generic;
using MusicTutor.Core.Models;
using MusicTutor.Core.Services;

namespace MusicTutor.Data.InMemory.Services
{
    public class DataServiceInMemory : IDataService
    {
        internal  HashSet<Instrument> Instruments { get; init; }

        public DataServiceInMemory()
        {
            this.Instruments = new HashSet<Instrument>();
        }

        public void SetupDataStore()
        {
            Console.WriteLine("****** SeedDatabase **********");                        
            Instruments.Add(Instrument.CreateInstrument("Local1"));
            Instruments.Add(Instrument.CreateInstrument("Local2"));
        }
    }
}