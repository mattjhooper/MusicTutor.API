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
            Instruments.Add(new Instrument("Local1"));
            Instruments.Add(new Instrument("Local2"));
        }
    }
}